<template>
 <article class="mb-3">
	<h2 class="post-title">Edit a navigation link.</h2>
 </article>
	<article>
		<form method="POST" action="#" name="formEditNavigationLink" @submit.prevent="onSubmit">
			<section class="row">
				<article class="col-12 mb-3">
					<input type="text" placeholder="Name" class="form-control" v-model="name"/>
					<ErrorBoxComponent v-if="nameErrorNumber" :messages="validationManager.errors.messages.name"></ErrorBoxComponent>
				</article>
				<article class="col-12 mb-3">
					<input type="text" placeholder="Route" class="form-control" v-model="route"/>
					<ErrorBoxComponent v-if="routeErrorNumber" :messages="validationManager.errors.messages.route"></ErrorBoxComponent>
				</article>
				<article class="col-12 mb-3 text-end">
					<input type="submit" value="Edit" class="btn btn-primary"/>
				</article>
			</section>
		</form>
	</article>
<ModalComponent v-if="showModal" title="Edit a  navigation link" successMessage="You have edited a  navigation link." :failureMessage="failureMessage" @hide-modal="showModal = false" :outcome="outcome"></ModalComponent>
</template>

<script>
import ErrorBoxComponent from "@/components/ErrorBoxComponent.vue";
import axios from "axios";
export default{
	inject : ["token", "baseURL", "validationManager", "errorMessages"],
	components : {ErrorBoxComponent},
	data(){
		return {
			name : "",
			nameErrorNumber : 0,
			route : "",
			routeErrorNumber : 0,
			showModal : false,
			outcome : true,
			failureMessage : ''
		}
	},
	created(){
		axios.get(this.baseURL + "navigationLinks/" + this.$route.params.id)
		.then(res =>{
			if(res.status == 200){
				this.name = res.data.name;
				this.route = res.data.route;
			}
		})
		this.validationManager.errors.messages = {
			name : [],
			route : []
		};
	},
	methods :{
		setErrorNumbers(){
			this.nameErrorNumber = this.validationManager.errors.messages.name.length;
			this.routeErrorNumber = this.validationManager.errors.messages.route.length;
		},
		resetErrorNumbers(){
			this.nameErrorNumber  = 0;
			this.routeErrorNumber = 0;
		},
		onSubmit(){
			if(this.valid()){
				this.sendRequest();
			}
		},
		sendRequest(){
			axios.put(this.baseURL + this.$route.params.table, {
				id : this.$route.params.id,
				name : this.name,
				route : this.route
			},{
				onUploadProgress : ()=>{
					this.$emit("load");
				}
			})
			.then(res => {
				if(res.status == 204){
					this.$emit("stop-loading");
					this.showModal= true;
					this.outcome = true;
				}
			}).catch((err)=>{
					this.$emit("stop-loading");
					if(err.response.status == 422){
						this.showModal= true;
						this.outcome = false;
						this.failureMessage = err.response.data.map( o => o.errorMessage).join("\n");
					}
					else if(err.response.status != 401){
						this.showModal= true;
						this.outcome = false;
						this.failureMessage = "An error ocurred while proccessing Your request.";
					}
			});
		},
		valid(){
			this.validationManager.errors.reset();
			this.resetErrorNumbers();
			if(!this.name.match(this.validationManager.regexes.category)) this.validationManager.errors.messages.name.push(this.errorMessages.category);

			if(!this.route.match(this.validationManager.regexes.route)) this.validationManager.errors.messages.route.push(this.errorMessages.route);

			if(this.validationManager.validationInvalid()){
				this.setErrorNumbers();
				return false;
			}
			return true;
		}
	}
}
</script>