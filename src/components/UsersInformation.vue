<template>
	<section class="w-100 box-shadow border-radius-7 p-3"> 
		<!-- Header -->
		<section class="row justify-content-center">
			<figure class="col-6  text-center">
				<img :src="'http://localhost:5000/images/' + user.profilePicture" class="img-fluid rounded-circle border" alt="profile-picture"/>
			</figure>
			<article class="col-12">
				<p class=" text-center  ">{{user.firstname}} {{user.lastname}}<br/>{{user.username}}</p>
			</article>
		</section>
		<!-- Header -- end -->
		<section class="w-100  d-flex justify-content-around">
			<article>
				<p class="  cursor-pointer" @click="showFollowers" v-if="user.followers.length">Followers</p>
				<p class="  "  v-else>Followers</p>
				<div><p>{{user.followers.length}}</p></div>
			</article>
			<article>
				<p class="  cursor-pointer" @click="showFollowing" v-if="user.following.length">Following</p>
				<p class="  "  v-else>Following</p>
				<div><p>{{user.following.length}}</p></div>
			</article>
		</section>
		<article>
			<p class="small-title text-center  ">Total posts: {{user.posts.length}}</p>
		</article>
		<article class="text-center">
			<!-- The Button I need to create here for every kind of an  user. -->
			<router-link :to="'/profile/edit/' + this.token.get().UserId" v-if="profileOwnerAccessedThisProfile" class="btn btn-blue" >Edit profile</router-link>
			<a href="#" v-else-if="visitorWithinFollowers" class="btn btn-blue" @click.prevent="unfollow">Unfollow</a>
			<a href="#" v-else class="btn btn-blue" @click.prevent="follow">Follow</a>
		</article>
	</section>
<FollowersModalComponent v-if="showModal" @server-error="serverError" @get-followers-following="getFollowers"  @hide-modal="showModal = false;" :title="title" :followers="followers"></FollowersModalComponent>
</template>

<script>
import FollowersModalComponent from "./FollowersModalComponent.vue"
export default {
	inject : ["token"],
	props : ["user"],
	components : {FollowersModalComponent},
	data(){
		return {
			showModal : false,
			title: "",
			followers : [],
			displayFollowers : true
		}
	},
	computed: {
		profileOwnerAccessedThisProfile(){
			const id = this.$route.params.id;
			return this.token.get().UserId == id;
		},
		visitorWithinFollowers(){
			return this.user.followers.find(f => f.id.toString() == this.token.get().UserId) != undefined;
		}
	},
	methods: {
		serverError(){
			this.$emit("server-error");
		},
		getFollowers(){
			this.$emit("get-followers");
		},
		unfollow(){
			this.$emit("unfollow")	;
		},
		follow(){
			this.$emit("follow");
		},
		showFollowers(){
			this.showModal  =true;
			this.followers = this.user.followers;
			this.title = "Followers";
		},
		showFollowing(){
			this.showModal  =true;
			this.followers = this.user.following;
			this.title = "Following"
		}
	}
}
</script>