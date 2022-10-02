<template>
	<NavComponent></NavComponent>
	<section class="container mx-auto vh100 py-3 ">
		<section class="row justify-content-between">
			<section class="col-12 col-md-2 ">
				<form action="#" method="POST" name="formSearchPosts" @submit.prevent="onSubmit">
					<section class="row">
						<article class="col-12 mb-3">
							<label class=" " for="search">Search for posts</label>
						</article>
						<article class="col-12 mb-3">
							<input type="text" placeholder="Search"  class="form-control" id="search" v-model="search"/>
						</article>
						<article class="col-12 mb-3">
							<label class=" " for="categories">Categories</label>
						</article>
						<article v-for="c in categories" :key="c" class="col-12 mb-3">
							<input type="checkbox" name="categories" :value="c.id" @click="toggleAdditionAndRemovalOfId(c.id)"/> {{c.name}}
						</article>
						<article class="col-12 mb-3">
							<label class=" " for="search">How many posts per page?</label>
						</article>
						<article class="col-12 mb-3">
							<input type="number" placeholder="Per page"  class="form-control" id="search" v-model.number="perPage"/>
						</article>
						<article class="col-12 mb-3 text-end">
							<input type="submit" value="Search" class="btn btn-primary"/>
						</article>
					</section>
				</form>
			</section>
			<section class="col-12 col-md-9">
				<article>
					<h1 class="title">Posts</h1>
				</article>
				<section class="row mx-0 " v-if="posts.data && posts.data.length">
					<PostComponent v-for="p in posts.data" :key="p" :post="p" ></PostComponent>
				</section>
				<article v-else>
					<p>There are no posts that match the given filter.</p>
				</article>
			</section>
		</section>
		<section class="col-12 d-flex justify-content-center" v-if="posts.data && posts.data.length">
			<article v-for="l in pageLinks" :key="l">
				<a href="#" class="btn btn-light" @click.prevent="nextPage(l)">{{l}}</a>
			</article>
		</section>
	</section>
	<FooterComponent></FooterComponent>
<ModalComponent v-if="showModal" title="Server error" @hide-modal="showModal = false" failureMessage="An error ocurred while proccessing Your request." :outcome="false"></ModalComponent>
<LoadingComponent v-if="loading"></LoadingComponent>
</template>

<script>
 import axios from "axios";
 import PostComponent from "@/components/PostComponent.vue";
	export default{
		inject: ["token", "baseURL"],
		components: {PostComponent, },
		data(){
			return {
				loading: false,
				perPage : 20,
				posts: [],
				categories : [],
				chosenCategories : [],
				search : "",
				showModal : false,
			}
		},
		created(){
			this.getCategories();
			this.getPosts();
		},
		computed : {
			categoriesQuery(){
				return "categories=" + this.chosenCategories.join("&categories=");
			},
			pageLinks(){
				let links = [];
				for(let i = 1; i <= this.posts.totalPages; i++){
					links.push(i);
				}
				return links;
			}
		},
		methods: {
			getCategories(){
				axios.get(this.baseURL + "categories")	
				.then(res => {
					if(res.status == 200 && res.data){
						this.categories = res.data;
					}
				})
				.catch((err)=>{
					if(err.response.status != 401){
						this.showModal = true;
					}
				});
			},
			getPosts(query = ""){
				axios.get(this.baseURL + "posts" + query, {
					onDownloadProgress : ()=>{
						this.loading = true;
					}
				})
				.then(res => {
					if(res.status == 200 && res.data){
						this.loading = false;
						this.posts = res.data;
					}
				})
				.catch((err)=>{
						this.loading = false;
					if(err.response.status != 401){
						this.showModal = true;
					}
				});
			},
			nextPage(page){
				let url = this.retrieveURLFromForm();
				url +="&page=" + page;
				this.getPosts(url);

			},
			toggleAdditionAndRemovalOfId(id){
				if(this.chosenCategories.find(c => c == id) != undefined){
					this.removeCategory(id);
				}else{
					this.addCategory(id);
				}
			},
			removeCategory(id){
				this.chosenCategories = this.chosenCategories.filter(c => c != id);
			},
			addCategory(id){
				this.chosenCategories.push(id)		;
			},
			onSubmit(){
				// alert(url);
				let url = this.retrieveURLFromForm();
				this.getPosts(url);
			},
			retrieveURLFromForm(){
				let url =  "/?";
				let firstVariableAdded= false;
				if(this.search){
					url += "keyword=" + this.search;
					firstVariableAdded = true;
				}
				if(this.chosenCategories.length > 0){
					if(firstVariableAdded){
						url += "&" +this.categoriesQuery ;
					}else{
						url += this.categoriesQuery ;
						firstVariableAdded = true;
					}
				}
				if(this.perPage > 0){
					if(firstVariableAdded){
						url += "&perpage=" + this.perPage; 
					}else {
						url += "perpage=" + this.perPage; 
						firstVariableAdded = true;
					}
				}
				return url;
			}
		}
	}
</script>