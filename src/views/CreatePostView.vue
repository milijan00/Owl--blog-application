<template>
<NavComponent></NavComponent>
<section class="container mx-auto vh80 row py-3">
	<article class="col-12">
		<h1 class="post-title ">
			Create a new post.
		</h1>
	</article>	
	<section class="col-12">
		<form method="POST" name="createPostForm" action="#" @submit.prevent="onSubmit">
			<section class="row">
				<section class="col-12 row mx-0">
					<article class="col-12 col-md-6 mb-3">
						<input type="text" placeholder="Title *" class="form-control" v-model="title"/>
						<ErrorBoxComponent v-if="titleErrorNumber" :messages="validationManager.errors.messages.title" ></ErrorBoxComponent>
					</article>
					<article class="col-12 col-md-6 mb-3">
						<select v-model="categoryId"  class="form-control">
							<option value="0">Choose a category *</option>
							<option v-for="c in categories" :key="c" :value="c.id">{{c.name}}</option>
						</select>
						<ErrorBoxComponent v-if="categoryErrorNumber" :messages="validationManager.errors.messages.category"></ErrorBoxComponent>
					</article>
				</section>
				<article class="col-12">
					<h2 class="small-title">Please enter a body. <span class="text-danger">*</span></h2>
				</article>
				<article class="col-12 mb-5">
					<!-- Place your Editor here -->
					<EditorComponent v-model="content"/>
					<ErrorBoxComponent v-if="bodyErrorNumber" :messages="validationManager.errors.messages.body"></ErrorBoxComponent> 
				</article>
				<article class="col-12 mb-3 text-end">
					<router-link :to="'/profile/'+ token.get().UserId" class="btn btn-light  ">Go back</router-link>
					<input type="submit" value="Create" class="btn btn-blue mx-2"  />
				</article>
			</section>
		</form>
	</section>
</section>
<FooterComponent></FooterComponent>
<ModalComponent v-if="showModal" title="Post creation" successMessage="You have created a new post successfully." failureMessage="There was an error processing Your request." :outcome="outcome" @hide-modal="showModal=false"></ModalComponent>
<LoadigComponent v-if="loading"></LoadigComponent>
</template>

<script>
import ErrorBoxComponent from '@/components/ErrorBoxComponent.vue';
import axios from "axios";
import EditorComponent from "../components/EditorComponent.vue";
	export default {
    components: { ErrorBoxComponent, EditorComponent },
    inject: ["token", "validationManager", "baseURL"],
    data() {
        return {
			loading : false,
			showModal : false,
			outcome : false,
			categories : [],
			categoryId : 0,
            title: "",
            content: "",
            titleErrorNumber: 0,
            bodyErrorNumber: 0,
			categoryErrorNumber : 0,
			delta : undefined
        };
    },
    created() {
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
		})
		.catch((err)=>{
			if(err.response.status != 401){
				this.showModal = true;
				this.outcome = false;
			}
		});
    },
    methods: {
        setErrorNumbers() {
            this.titleErrorNumber = this.validationManager.errors.messages.title.length;
            this.bodyErrorNumber = this.validationManager.errors.messages.body.length;
            this.categoryErrorNumber = this.validationManager.errors.messages.category.length;
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
			if(this.categoryId == 0)this.validationManager.errors.messages.category.push("Category is required."); 
			if(!this.content ) this.validationManager.errors.messages.body.push("Body is required");
			if(this.validationManager.validationInvalid()){
				this.setErrorNumbers();
				return false;
			}
			// console.log(this.$refs.editor.quill.getContents());
			return true;
		},
		sendRequest(){
			axios.post(this.baseURL + "posts", {
				userId : this.token.get().UserId,
				title : this.title,
				content : this.content,
				categoryId : this.categoryId
			}, {
				onUploadProgress : ()=>{
					this.loading = true;
				}
			})
			.then(res => {
				if(res.status == 201){
					this.loading = false;
					// show mdal
					this.outcome = true;
					this.showModal = true;
					this.clearForm();
				}
			})
			.catch((err)=>{
				this.loading = false;
				if(err.response.statu != 401){
					this.outcome = false;
					this.showModal = true;
				}
			});
		},
        onSubmit() {
			if(this.valid()){
				this.sendRequest();
			}
        },
		clearForm(){
			this.content = "";
			this.title = "";
			this.categodyId = 0;
		}
    }
}
</script>

<style>

	.ql-editor {
		min-height: 42vh;
	}
</style>