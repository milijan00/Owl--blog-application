<template>
	<section class="border-radius-7 box-shadow p-3 mb-3">
		<section class="post-header row justify-content-between">
			<AuthorComponent :image="post.profilePicture" :username="post.username" :id="post.userId" ></AuthorComponent>
			<article class="col-12 col-md-3 my-3 my-md-0">
				<p class="text-center text-md-end  ">{{post.createdAt}}</p>	
			</article>
			<div v-if="post.userId.toString() == token.get().UserId" class="dropdown col-12 col-md-2 text-end">
  <button class="btn btn-light dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-three-dots-vertical" viewBox="0 0 16 16">
  <path d="M9.5 13a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0zm0-5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0zm0-5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0z"/>
</svg>
  </button>
  <ul class="dropdown-menu">
    <li><router-link class="dropdown-item" :to="'/posts/edit/' + post.id">Edit</router-link></li>
    <li><a class="dropdown-item" href="#" @click.prevent="showModal = true;">Delete</a></li>
  </ul>
</div>
		</section>
		<section class="post-body py-3">
			<article>
				<h1 class="post-title text-start">{{post.title}}</h1>
			</article>
			<CategoryComponent :category="post.category"></CategoryComponent>
		</section>
		<section class="post-footer d-flex justify-content-between">
			<div class="d-flex ">
			<article class="mx-1">
				<p class="mb-0 pb-0">{{post.numberOfLikes}}</p>
			</article>
			<article>
				<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-heart-fill ml-2" viewBox="0 0 16 16">
  <path fill-rule="evenodd" d="M8 1.314C12.438-3.248 23.534 4.735 8 15-7.534 4.736 3.562-3.248 8 1.314z"/>
</svg>
			</article>
			</div>
			<article>
				<router-link :to="'/posts/' + post.id" class="btn btn-light  ">Show me more</router-link>
			</article>
		</section>	
	</section>
	<YesNoModalComponent v-if="showModal" message="Do You want to delete this post?" title="Delete post" @no="showModal = false;" @yes="deletePost"></YesNoModalComponent>
</template>

<script>
import AuthorComponent from './AuthorComponent.vue';
import axios from "axios";
import YesNoModalComponent from "./YesNoModalComponent.vue";
import CategoryComponent from './CategoryComponent.vue';
export default {
	   data(){
		return {
			showModal : false
		}
	   },
    components: { AuthorComponent, YesNoModalComponent, CategoryComponent },
	props: ["post" ],
	inject : ["token", "baseURL"],
	methods: {
		deletePost(){
			this.showModal = false;
			axios.delete(this.baseURL + "posts/" + this.post.id)
			.then(res => {
				if(res.status == 204){
					this.$emit("refresh-posts");
				}
			});
		}
	}
}
</script>


<style>

/* .box-shadow,
.post{
	box-shadow: 5px 3px 20px #e2e2e2;
} */
.post{
	border-radius: 7px;
}
/* .post{

} */

</style>