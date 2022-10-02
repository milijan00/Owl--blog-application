<template>
 <article class="mb-3">
	<h2 class="post-title">Create new category.</h2>
 </article>
	<article>
		<form method="POST" action="#" name="formCreateCategory" @submit.prevent="onSubmit">
			<section class="row">
				<article class="col-12 mb-3">
					<input type="text" placeholder="Name" class="form-control" v-model="name"/>
					<ErrorBoxComponent v-if="nameErrorNumber" :messages="validationManager.errors.messages.name"></ErrorBoxComponent>
				</article>
				<article class="col-12 mb-3 text-end">
					<input type="submit" value="Create" class="btn btn-primary"/>
				</article>
			</section>
		</form>
	</article>
<ModalComponent v-if="showModal" title="Create  new category" successMessage="You have created a new category." :failureMessage="failureMessage" @hide-modal="showModal = false" :outcome="outcome"></ModalComponent>
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
			showModal : false,
			outcome : true,
			failureMessage : ""
		}
	},
	created(){
		this.validationManager.errors.messages = {
			name : []
		};
	},
	methods :{
		setErrorNumbers(){
			this.nameErrorNumber = this.validationManager.errors.messages.name.length;
		},
		resetErrorNumbers(){
			this.nameErrorNumber  = 0;
		},
		onSubmit(){
			if(this.valid()){
				this.sendRequest();
			}

		},
		sendRequest(){
			axios.post(this.baseURL + this.$route.params.table, {
				name : this.name
			}, {
				onUploadProgress : ()=>{
					this.$emit("load");
				}
			})
			.then(res => {
				if(res.status == 201){
					this.$emit("stop-loading");
					this.showModal= true;
					this.outcome = true;
				}
			}).catch((err)=>{
				this.$emit("stop-loading");
				if(err.response.status == 422){
					this.failureMessage = err.response.data[0].errorMessage;
					this.showModal= true;
					this.outcome = false;
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

			if(this.validationManager.validationInvalid()){
				this.setErrorNumbers();
				return false;
			}
			return true;
		}
	}
}
</script>