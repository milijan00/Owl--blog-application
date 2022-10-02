<template>
		<article class="col-12">
			<form method="POST" action="#" name="formCreateComment" @submit.prevent="onSubmit">
				<section class="row justify-content-center">
					<article class="col-11  mb-3">
						<textarea placeholder="What's on Your mind?" rows="2" cols="20" class="form-control child-comment-control" v-model="content"></textarea>
						<ErrorBoxComponent v-if="contentErrorNumber" :messages="this.validationManager.errors.messages.content"></ErrorBoxComponent>
					</article>	
					<article class="col-1 mb-3 text-start" >
						<input type="submit" value="Done" class="btn btn-blue child-comment-control"/>
					</article>
				</section>
			</form>
		</article>
</template>

<script>
// import CommentComponent from './CommentComponent.vue';
import ErrorBoxComponent from "./ErrorBoxComponent.vue";
import axios from "axios";
	export default {
    inject: ["token", "baseURL", "validationManager"],
	data(){
		return {
			content: "",
			contentErrorNumber : 0,
			passedComments : this.comments
		}
	},
    // props: ["comments"],
	props : {
		parentId: undefined,
		comments : {
			type : [],
			default(){
				return [];
			} 
		}
	},
	created(){
		this.validationManager.errors.messages = {
			content : []
		}	;
		// this.passedComments = this.comments;
	},
    methods: {
		serverError(){
			this.$emit("server-error");
		},
		setErrorNumbers(){
			this.contentErrorNumber = this.validationManager.errors.messages.content.length;
		},
		resetErrorNumbers(){
			this.contentErrorNumber = 0;
		},
		clearForm(){
			this.content= "";
		},
		refresh(){
			this.$emit("refresh-comments");
		},
		valid(){
			this.validationManager.errors.reset();
			this.resetErrorNumbers();
			if(!this.content) this.validationManager.errors.messages.content.push("Content is required.");

			if(this.validationManager.validationInvalid()){
				this.setErrorNumbers();
				return false;
			}
			return true;
		},
		sendRequest(){
			const id = this.$route.params.id;
			// alert(this.parentId);
			axios.post(this.baseURL + "comments", {
				content: this.content,
				userId : this.token.get().UserId,
				postId : id,
				parentId : this.parentId
			})
			.then(res =>{
				if(res.status ==  201){
					this.clearForm();
					this.refresh();
				}
			})
			.catch(()=>{
				this.serverError();
			})
		},
		onSubmit(){
			if(this.valid()){
				this.sendRequest();
			}
		},
        hide() {
            this.$emit("hide-comments");
        }
    },
    components: { 
		// CommentComponent,
		ErrorBoxComponent
	 }
}
</script>

<style>
.child-comment-control{
	border-radius: 50px;

}
</style>