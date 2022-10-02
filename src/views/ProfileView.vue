<template>
	<NavComponent></NavComponent>
	<section class="container mx-auto py-4 vh100">
		<section class="row ">
			<section class="col-12 col-md-3">
				<UsersInformation :user="user" @follow="follow" @unfollow="unfollow" @server-error="showModalWithFalseOutcome" @get-followers="getFollowers"></UsersInformation>
			</section>
			<section class="col-12 col-md-9 p-2 row mx-0">
				<article class="col-12 mb-3">
					<h1 class="post-title">Your posts</h1>
					<span>
					<router-link class="btn btn-blue" v-if="userIsOnHisProfile" title="Create a new post" to="/posts/create"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-plus-lg text-white" viewBox="0 0 16 16">
  <path fill-rule="evenodd" d="M8 2a.5.5 0 0 1 .5.5v5h5a.5.5 0 0 1 0 1h-5v5a.5.5 0 0 1-1 0v-5h-5a.5.5 0 0 1 0-1h5v-5A.5.5 0 0 1 8 2Z"/>
</svg></router-link>
					</span>
				</article>
				<section class="col-12" v-if="user.posts.length">
				 <PostComponent v-for="p in user.posts" :key="p" :post="p"  @refresh-posts="loadUserData"></PostComponent>	 
				</section>
				<article v-else class="col-12">
					<p class="  text-center">Currently You don't have any posts.</p>
				</article>
			</section>
		</section>
	</section>
	<FooterComponent></FooterComponent>
	<ModalComponent v-if="showModal" @hide-modal="showModal=false" title="Server Error" failureMessage="An error ocurred while proccessing Your request." :outcome="outcome"></ModalComponent>
	<LoadingComponent v-if="loading"></LoadingComponent>
</template>

<script>
import UsersInformation from "../components/UsersInformation.vue";
import PostComponent from "@/components/PostComponent.vue";
import axios from "axios";
export default {
	inject: ["baseURL", "token"],
    components: { UsersInformation, PostComponent },
	data(){
		return {
			loading: false,
			user : {
				posts : [],
				followers : [],
				following :[]
			},
			userId : 0,
			showModal : false,
			outcome : false
		}
	},
	watch : {
		userId(){
			this.loadUserData();
		}
	},
	created(){
		this.userId = this.$route.params.id;
	},
	updated(){
		this.userId = this.$route.params.id;
	},
	computed : {
		userIsOnHisProfile(){
			return this.token.get().UserId == this.$route.params.id.toString();
		}
	},
	methods : {
		loadUserData(){
		axios.get(this.baseURL + "users/" + this.userId, {
			onDownloadProgress : () => {
				this.loading = true;
			}
		})
		.then(res => {
			if(res.status == 200 && res.data){
				this.loading = false;
				this.user = res.data;
			}
		})
		.catch((err) => {
			this.loading = false;
			if(err.response.status == 404){
				this.$router.push("/not-found");
			}
			else if(err.response.status != 401){
				this.showModal = true;
			}
		});

		},
		showModalWithFalseOutcome(){
			this.showModal = true;
			this.outcome = false;
		},
		getFollowers(){
					axios.get(this.baseURL + "followers/" + this.userId)
					.then(res => {
						if(res.status == 200 && res.data){
							this.user.followers = res.data.followers;
							this.user.following = res.data.following;
						}
					})
					.catch((err)=>{
						if(err.response.status != 401){
							this.showModalWithFalseOutcome();
						}
					});
		},
		follow(){
			axios.post(this.baseURL + "followers", {
				userId : this.userId,
				followerId :  this.token.get().UserId
			})
			.then(res => {
				if(res.status == 201){
					this.getFollowers();
				}
			}).catch((err)=>{
				if(err.response.status  != 401){
					this.showModalWithFalseOutcome();
				}
			});
		},
		unfollow(){
			axios.delete(this.baseURL + "followers/user/" + this.userId  + "/follower/" + this.token.get().UserId	)
			.then(res => {
				if(res.status == 204){
					this.getFollowers();
				}
			})
			.catch((err)=>{
				if(err.response.status != 401){
					this.showModalWithFalseOutcome();
				}
			});
		}
	}
}
</script>