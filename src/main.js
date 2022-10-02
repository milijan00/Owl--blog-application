import  "bootstrap/dist/css/bootstrap.css";
import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import NavigationPlugin from "./plugins/NavigationPlugin";
import FooterPlugin from "./plugins/FooterPlugin";
import ValidationPlugin from "./plugins/ValidationPlugin";
import ModalPlugin from "./plugins/ModalPlugin";
import ErrorMessagesPlugin from "./plugins/ErrorsPlugin";
import LoadingPlugin from "./plugins/LoadingPlugin";
import axios from "axios";

const app =createApp(App); 

app.use(router);
app.use(NavigationPlugin);
app.use(FooterPlugin);
app.use(ValidationPlugin);
app.use(ModalPlugin);
app.use(ErrorMessagesPlugin);
app.use(LoadingPlugin);

const baseURL = "http://localhost:5000/api/";
const token = {
	get : ()=>{
		let token = localStorage.getItem("token") || "";
		if(!token) return null;
		let atobData = atob(token.split('.')[1]) ;
		return  JSON.parse(atobData);
	},
	isValid : function(){
		const expiresIn = this.get().exp;
		return Math.floor((new Date).getTime()/ 1000) < expiresIn;
	}
};
const refreshToken = {
	get : ()=>{
		let token = localStorage.getItem("refresh") || "";
		if(!token) return null;
		let atobData = atob(token.split('.')[1]);
		return  JSON.parse(atobData);
	},
	isValid : function(){
		const expiresIn = this.get().exp;
		return Math.floor((new Date).getTime()/ 1000) < expiresIn;
	}
};
const removeTokens = ()=>{
	localStorage.removeItem("token");
	localStorage.removeItem("refresh");
}
const redirectUserToLoginPage = () =>{
	removeTokens();
	router.push("/");
}
const storeTokens = (data)=>{
	localStorage.setItem("token", data.access);
	localStorage.setItem("refresh", data.refresh);
}
axios.interceptors.request.use(
	function(config){
		if(token.get()){
			config.headers.common["Authorization"] = "Bearer " + (localStorage.getItem("token") || "");
		}
		return config;
	},
	function(error){
		return Promise.reject(error);
	}
);
axios.interceptors.response.use(
	function(response){
		return response;
	},
	function(error){
		console.log(error);
		if(error.response.status == 401 ) {
			let refreshToken = localStorage.getItem("refresh");
			if(refreshToken){
				if(!tokensAreValid()){
					axios.post(baseURL + "tokens/refresh", {value : refreshToken})
					.then(res => {
						if(res.status == 200 && res.data){
							storeTokens(res.data);
						}else if(res.status == 400){
							redirectUserToLoginPage();
						}
					});
				}
			}else {
				redirectUserToLoginPage();
			}
		}
		return Promise.reject(error);
	}
)

const determineUsersAccessabilityToASpecificRoute = (next, path, callback)=>{
		if(callback()){
			next();
		}else {
			next({path : path});
		}
}
const tokensAreValid = ()=>{
	return token.isValid() && refreshToken.isValid();
}
const tokensDontExist = ()=>{
	return !token.get() && !refreshToken.get();
}
router.beforeEach((to ,from, next) =>{
	if(to.meta.requiresUnauth && token.get()){
		const userId = token.get().UserId;
		determineUsersAccessabilityToASpecificRoute(next, "/profile/" + userId, ()=>{ return token.get() == null});
	}
	else if(to.meta.requiresAuth ){
		if(tokensDontExist() || !tokensAreValid()  ){
			removeTokens();
			next({path : "/"});
		}else {
			const userId = token.get().UserId;
			if(to.meta.requiresAdmin){
				determineUsersAccessabilityToASpecificRoute(next, "/profile/" + userId, ()=>{ return  token.get().Role == 'Administrator' });
			}else {
				determineUsersAccessabilityToASpecificRoute(next, "/", ()=>{ return token.get()});
			}
		}
	}else {
		next();
	}
} );


app.provide("baseURL", baseURL);
app.provide("token", token);

app.mount('#app');

import  "bootstrap/dist/js/bootstrap.js";