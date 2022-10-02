<template>
 <article class="mb-3">
	<h2 class="post-title">Edit a role.</h2>
 </article>
	<article>
		<form method="POST" action="#" name="formEditCategory" @submit.prevent="onSubmit">
			<section class="row">
				<article class="col-12 mb-3">
					<input type="text" placeholder="Name" class="form-control" v-model="name"/>
					<ErrorBoxComponent v-if="nameErrorNumber" :messages="validationManager.errors.messages.name"></ErrorBoxComponent>
				</article>
				<section class="row col-12 mx-0 px-0">
					<article class="usecase   col-12 col-md-6 mb-3 " v-for="u in allUsecases" :key="u">
						<div class="input-group border border-radius-7">
							<div class="input-group-text">
								<input class="form-check-input mt-0" type="checkbox" :value="u.id" aria-label="Checkbox for following text input" :checked="usecaseAssignedToTheRole(u.id)" @click="updateUsecaseList(u.id)"/>
							</div>
							<span class="px-2 py-2"><p class="  mb-0">{{u.name}}</p></span>
						</div>
					</article>
				</section>
				<article class="col-12 mb-3 text-end">
					<input type="submit" value="Edit" class="btn btn-primary"/>
				</article>
			</section>
		</form>
	</article>
<ModalComponent v-if="showModal" title="Edit a role" successMessage="You have edited  a role." :failureMessage="failureMessage" @hide-modal="showModal = false" :outcome="outcome"></ModalComponent>
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
			failureMessage : "",
			allUsecases : [],
			usecases : []

		}
	},
	created(){
		axios.get(this.baseURL + "roles/" + this.$route.params.id)
		.then(res => {
			if(res.status == 200 && res.data){
				this.name = res.data.name;
				this.usecases = res.data.usecases;
				this.loadUsecases();
			}
		});
		this.validationManager.errors.messages = {
			name : []
		};
	},
	methods :{
		loadUsecases(){
			axios.get(this.baseURL + "usecase")
			.then(res => {
				if(res.status == 200 && res.data){
					this.allUsecases = res.data;
				}
			});
		},
		usecaseAssignedToTheRole(id){
			return this.usecases.find(u => u.usecaseId == id) != undefined;
		},
		updateUsecaseList(id){
			const usecase = this.usecases.find(x => x.usecaseId == id);
			if(usecase){
				this.usecases = this.usecases.filter(x => x.usecaseId != id);
			}else {
				this.usecases.push({
					roleId: Number(this.$route.params.id),
					usecaseId : id
				});
			}
		},
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
			axios.put(this.baseURL + this.$route.params.table + '/' + this.$route.params.id, {
				name : this.name,
				usecases : this.usecases
			}, {
				onUploadProgress : ()=>{
					this.$emit("load");
				}
			})
			.then(res => {
				if(res.status == 204){
					this.$emit("stop-loading");
					this.showModal= true;
					this.outcome = true;
					this.loadUsecases();
				}
			}).catch((err)=>{
					this.$emit("stop-loading");
					if(err.response.status == 422){
						this.showModal= true;
						this.outcome = false;
						this.failureMessage = err.response.data.map(o => o.errorMessage).join("\n");
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

<style>
</style>