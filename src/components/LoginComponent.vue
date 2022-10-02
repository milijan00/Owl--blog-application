<template>
	<section class="col-12 col-md-6 py-3 " id="login-wrapper">
		<article>
			<h1 class="post-title">Sign in</h1>
		</article>
		<article>
			<form name="formlogin" method="POST" action="#" @submit.prevent="onSubmit">
				<section class="row ">
					<article class="col-12 mb-3">
						<input type="text" placeholder="Email" class="form-control" v-model="email"/>
						<ErrorBoxComponent v-if="emailErrorNums" :messages="this.validationManager.errors.messages.email"></ErrorBoxComponent>
					</article>
					<article class="col-12 mb-3">
						<section class="row mx-0 px-0">
						<article class="col-11 px-0">
							<input :type="passwordType" placeholder="Password" class="form-control" v-model="password"/>
						</article>
						<ShowHidePasswordComponent @hide="passwordType='password'" @show="passwordType='text'"></ShowHidePasswordComponent>
						</section>
						<ErrorBoxComponent v-if="passwordErrorNums" :messages="this.validationManager.errors.messages.password"></ErrorBoxComponent>
					</article>
					<article class="col-12 mb-3">
						<input type="submit" value="Sign in" class="btn btn-blue"/>
					</article>
				</section>
			</form>
			<article>
				<p class=" ">If You don't have an account sing up <router-link to="/register">here</router-link>.</p>
			</article>
		</article>
	</section>
  <LoadingComponent v-if="loading"></LoadingComponent>
</template>

<script>
import ErrorBoxComponent from './ErrorBoxComponent.vue';
import axios from "axios";
import ShowHidePasswordComponent from './ShowHidePasswordComponent.vue';

export default {
	components: {
    ErrorBoxComponent,
    ShowHidePasswordComponent
},
	inject : ['validationManager', "baseURL", "token", "errorMessages" ],
	data(){
		return {
			email : "",
			password : "",
			passwordType : "password",
			emailErrorNums : 0,
			passwordErrorNums : 0,
			loading: false
		}
	},
	created(){
		this.initializeErrorMesages();
	},
	methods : {
		initializeErrorMesages(){
			this.validationManager.errors.messages = {
				email : [],
				password : []
			};
		},
		clearForm(){
			this.email = "";
			this.pasword = "";
		},
		setErrorNumbers(){
			this.emailErrorNums = this.validationManager.errors.messages.email.length;
			this.passwordErrorNums = this.validationManager.errors.messages.password.length;
		},
		resetErrorNumbers(){
			this.emailErrorNums = 0;
			this.passwordErrorNums = 0;
		},
		valid(){
			this.validationManager.errors.reset();
			this.resetErrorNumbers();
			if(!this.email.match(this.validationManager.regexes.email)) this.validationManager.errors.messages.email.push(this.errorMessages.email);
			
			if(!this.password.match(this.validationManager.regexes.password)) this.validationManager.errors.messages.password.push(this.errorMessages.password);
			if(this.validationManager.validationInvalid()){
				this.setErrorNumbers();
				return false;
			}
			return true;
		},
		sendRequest(){
			axios.post(this.baseURL + "tokens",{
				email : this.email,
				password : this.password
			}, {
				onUploadProgress : ()=>{
					this.loading = true;
				}
			}).then(res => {
				if(res.status == 200 && res.data){
					this.loading = false;
					localStorage.setItem("token", res.data.access);
					localStorage.setItem("refresh", res.data.refresh);
					this.$emit("show-modal", true);
					this.clearForm();
					this.$router.push("/profile/" + this.token.get().UserId);
				}
			}).catch(() => {
				this.loading  =false;
				this.$emit("show-modal", false);
			});
		},
		onSubmit(){
			if(this.valid()){
				this.sendRequest();
			}
		}
	}
}
</script>

<style>
#login-wrapper{
	border-radius: 7px;
	border-width: 1px;
}
</style>