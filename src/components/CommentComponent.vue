<template>
	<section class="col-12 mb-3  border-rounded  box-shadow comment" :class="parentComment? 'parent-comment' : 'child-comment'">
		<section class="w-100 row  m-0 justify-content-between flex-column flex-md-row">
			<AuthorComponent :image="comment.profilePicture"  :id="comment.userId" :username="comment.username"></AuthorComponent>
			<div v-if="comment.userId.toString() == token.get().UserId"  class="dropdown col-12 col-md-2 text-end">
  <button class="btn btn-light dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-three-dots-vertical" viewBox="0 0 16 16">
  <path d="M9.5 13a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0zm0-5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0zm0-5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0z"/>
</svg>
  </button>
  <ul class="dropdown-menu">
    <li><router-link class="dropdown-item" to="#" @click.prevent="contentNotEditing = false">Edit</router-link></li>
    <li><a class="dropdown-item" href="#" @click.prevent="showModal = true;">Delete</a></li>
  </ul>
</div>
		</section>

		<article v-if="contentNotEditing">
			<p class=" ">{{comment.content}}</p>
		</article>
	<article v-else>
		<form action="#" method="POST" name="formEditComment" @submit.prevent="editComment">
			<section class="row">
				<article class="col-12 mb-3">
					<textarea rows="3" cols="20" class="form-control p-3" v-model="newContent" ></textarea>
					<ErrorBoxComponent v-if="newContentErrorNumber" :messages="this.validationManager.errors.messages.newContent"></ErrorBoxComponent>
				</article>
				<article class="col-12 mb-3">
					<input type="submit" value="Edit" class="btn btn-blue"/>
				</article>
			</section>
		</form>
	</article>
	<article>
		<p>{{this.comment.likes.length}}
<a href="#" v-if="userDislikedComment" @click.prevent="likeComment"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-heart" viewBox="0 0 16 16">
  <path d="m8 2.748-.717-.737C5.6.281 2.514.878 1.4 3.053c-.523 1.023-.641 2.5.314 4.385.92 1.815 2.834 3.989 6.286 6.357 3.452-2.368 5.365-4.542 6.286-6.357.955-1.886.838-3.362.314-4.385C13.486.878 10.4.28 8.717 2.01L8 2.748zM8 15C-7.333 4.868 3.279-3.04 7.824 1.143c.06.055.119.112.176.171a3.12 3.12 0 0 1 .176-.17C12.72-3.042 23.333 4.867 8 15z"/>
</svg>
</a>
<a href="#" v-else @click.prevent="dislikeComment">
<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-heart-fill" viewBox="0 0 16 16">
  <path fill-rule="evenodd" d="M8 1.314C12.438-3.248 23.534 4.735 8 15-7.534 4.736 3.562-3.248 8 1.314z"/>
</svg>
</a>
		<!-- <a href="#" class="  mx-3" @click.prevent="toggleShowingHidingCommentsAndForm" ref="reply">Reply</a> -->
	</p>
	</article>
		<section class="row"  v-if="parentComment">
			<CommentsWithFormComponent :parentId="comment.id" :comments="comment.childComments" @refresh-comments="refreshComments"></CommentsWithFormComponent>	
			<section v-if="childComments.length" >
				<CommentComponent v-for="c in childComments" :key="c" :comment="c" @comment-deleted="refreshComments" @comment-updated="refreshComments" @reload-comments="refreshComments" @server-error="serverError"></CommentComponent>	
			</section>
			<!-- <article v-else>
			<p class=" ">Nobody has commented yet.</p>
		</article>  -->
		</section>
	</section>
	
	<YesNoModalComponent v-if="showModal" title="Delete comment" message="Do You really want to delete this comment?" @yes="deleteComment" @no="showModal=false" ></YesNoModalComponent>
</template>

<script>
import AuthorComponent from './AuthorComponent.vue';
import YesNoModalComponent from "./YesNoModalComponent.vue";
import axios from "axios";
import ErrorBoxComponent from "./ErrorBoxComponent.vue";
import CommentsWithFormComponent from './CommentsWithFormComponent.vue';
	export default{
		inject : ["token", "baseURL", "validationManager"],
    props: ["comment", "parentComment"],
    components: { AuthorComponent, YesNoModalComponent, ErrorBoxComponent, CommentsWithFormComponent },
		data(){
			return {
				showModal :false ,
				contentNotEditing: true,
				newContent: "",
				newContentErrorNumber : 0,
				showChildComments : false,
				childComments : []
			}
		},
		computed: {
			userDislikedComment(){
				return this.comment.likes.find(x => x.userId.toString() == this.token.get().UserId) == undefined ;
			}
		},
		created(){
			this.validationManager.errors.messages = {
				newContent : []
			};

			this.newContent = this.comment.content;
			this.childComments = this.comment.childComments;
			// console.log(this.comment);
		},
		methods: {
			refreshComments(){
				this.$emit("child-created")				;
			},
			toggleShowingHidingCommentsAndForm(){
				if(this.showChildComments){
					this.hideCommentsAndForm();
				}else{
					this.showCommentsAndForm();
				}
			},
			showCommentsAndForm(){
				this.showChildComments = true;
				this.$refs.reply.textContent="Cancel";
			},
			hideCommentsAndForm(){
				this.showChildComments = false;
				this.$refs.reply.textContent="Reply";
			},
			likeComment(){
				axios.post(this.baseURL + "likedComments", {
					userId : this.token.get().UserId,
					commentId: this.comment.id
				})
				.then(res => {
					if(res.status == 201){
						this.getLikes();
					}
				})
				.catch(()=>{
					this.$emit("server-error");
				});
			},
			dislikeComment(){
				axios({
					method: "delete",
					url : this.baseURL + "likedComments",
					data : {
						userId : this.token.get().UserId,
						commentId: this.comment.id
					}
				})
				.then(res => {
					if(res.status == 204){
						this.getLikes();
					}
				})
				.catch(()=>{
					this.$emit("server-error");
				});
			},
			getLikes(){
				this.$emit("reload-comments");
			},
			deleteComment(){
				axios.delete(this.baseURL + "comments/" + this.comment.id)
				.then(res => {
					if(res.status == 204){
						this.showModal=false;
						this.$emit("comment-deleted");
					}
				})
				.catch(()=>{
					this.$emit("server-error");
				});
			},
			valid(){
				this.validationManager.errors.reset();
				this.newContentErrorNumber = 0;
				if(!this.newContent) this.validationManager.errors.messages.newContent.push("Content is required.");

				if(this.validationManager.validationInvalid()){
					this.newContentErrorNumber = this.validationManager.errors.messages.newContent.length;
					return false;
				}
				return true;
			},
			sendRequest(){
				axios.put(this.baseURL + "comments", {
					id : this.comment.id,
					content : this.newContent
				})
				.then(res => {
					if(res.status == 204){
						this.$emit("comment-updated");
						this.contentNotEditing = true;
					}
				})
				.catch(()=>{
					this.$emit("server-error");
				});
			},
			editComment(){
				if(this.valid()){
					this.sendRequest();
				}
			}
		},
}
</script>

<style>
	.comment{
		padding: 20px;
		border-radius: 50px;
	}

	.parent-comment{
		background: #fff;
	}

	.child-comment{
		box-shadow: none;
		background-color: rgba(248, 249, 250);
	}
	
	@media screen and (min-width: 720px){
		.child-comment{
			padding-left: 40px;
		}
	}
</style>