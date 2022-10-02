<template>
	<NavComponent></NavComponent>
<section class="container mx-auto vh100 row py-3">
	<article class="col-12">
		<h1 class="post-title ">
			Edit the  post.
		</h1>
	</article>	
	<section class="col-12">
		<form method="POST" name="createPostForm" action="#" @submit.prevent="onSubmit">
			<section class="row">
				<section class="row mx-0">
					<article class="col-12 col-md-6 mb-3">
						<input type="text" placeholder="Title *" class="form-control" v-model="title"/>
						<ErrorBoxComponent v-if="titleErrorNumber" :messages="validationManager.errors.messages.title" ></ErrorBoxComponent>
					</article>
					<article class="col-12 col-md-6 mb-3">
						<select v-model="categoryId" class="form-control">
							<option value="0">Choose a category *</option>
							<option v-for="c in categories" :key="c" :value="c.id">{{c.name}}</option>
						</select>
						<ErrorBoxComponent v-if="categoryErrorNumber" :messages="validationManager.errors.messages.category"></ErrorBoxComponent>
					</article>
				</section>
				<article class="col-12">
					<h2 class="small-title">Please enter a body. <span class="text-danger">*</span></h2>
				</article>
				<article class="col-12 mb-3">
					<EditorComponent v-model="body"></EditorComponent>
					<ErrorBoxComponent v-if="bodyErrorNumber" :messages="validationManager.errors.messages.body"></ErrorBoxComponent>
				</article>
				<article class="col-12 mb-3 text-end">
					<router-link :to="'/profile/'+ token.get().UserId" class="btn btn-light">Go back</router-link>
					<input type="submit" value="Update" class="btn btn-blue mx-2"  />
				</article>
			</section>
		</form>
	</section>
</section>
	<FooterComponent></FooterComponent>
	<ModalComponent v-if="showModal" @hide-modal="showModal = false;" title="Edit post" successMessage="You have update the post successfully." failureMessage="There was an error proccessing Your request." :outcome="outcome"></ModalComponent>
	<LoadingComponent v-if="loading"></LoadingComponent>
</template>

<script>
import axios from "axios";
import ErrorBoxComponent from "@/components/ErrorBoxComponent.vue";
import EditorComponent from "@/components/EditorComponent.vue";

export default {
	components : {
    ErrorBoxComponent,
    EditorComponent
},
	inject : ["baseURL", "validationManager", "token"],
	data(){
		return {
			loading : false,
			title: "",
			body: "",
			categoryId : 0,
			categories : [],
			showModal : false,
			outcome : false,
            titleErrorNumber: 0,
            bodyErrorNumber: 0,
			categoryErrorNumber : 0
		}
	},
	created(){
		this.loadPost();
        this.validationManager.errors.messages = {
            title: [],
            body: [],
			category : []
        };

		axios.get(this.baseURL + "categories")
		.then(res => {
			if(res.status == 200 && res.data){
				this.categories = res.data;
			}
		});
	},
	methods: {
		loadPost(){
			const id = this.$route.params.id;
			axios.get(this.baseURL + "posts/" + id, {
				onDownloadProgress: ()=>{
					this.loading  = true;
				}
			})
			.then(res => {
				if(res.status == 200 && res.data){
					this.loading = false;
					this.title = res.data.title;
					this.body = res.data.content;
					this.categoryId = res.data.categoryId;
				}
			})
			.catch((err)=>{
				this.loading = false;
				if(err.response.statu != 401){
					this.showModal = true;
					this.outcome = false;
				}
			});
		},
        setErrorNumbers() {
            this.titleErrorNumber = this.validationManager.errors.messages.title.length;
            this.bodyErrorNumber = this.validationManager.errors.messages.body.length;
            this.categoryErrorNumber = this.validationManager.errors.messages.body.length;
        },
        resetErrorNumbers() {
            this.titleErrorNumber = 0;
            this.bodyErrorNumber = 0;
            this.categoryErrorNumber = 0;
        },
		valid(){
            this.validationManager.errors.reset();
			this.resetErrorNumbers();
			if(!this.title) this.validationManager.errors.messages.title.push("Title is required.");
			if(!this.body) this.validationManager.errors.messages.body.push("Body is required.");
			if(this.categoryId == 0)this.validationManager.errors.messages.category.push("Category is required."); 
			if(this.validationManager.validationInvalid()){
				this.setErrorNumbers();
				return false;
			}
			return true;
		},
		sendRequest(){
			const id = this.$route.params.id;
			axios.patch(this.baseURL + "posts", {
				id: id,
				title : this.title,
				content : this.body,
				categoryId : this.categoryId
			}, {
				onUploadProgress : ()=>{
					this.loading = true;
				}
			})
			.then(res => {
				if(res.status == 204){
					this.loading  =false;
					this.outcome = true;
					this.showModal = true;
				}
			})
			.catch((err)=>{
				this.loading  = false;
				if(err.response.status != 401){
					this.outcome = false;
					this.showModal = true;
				}
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
