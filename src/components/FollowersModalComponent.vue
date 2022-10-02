<template>
	<div class="container-fluid vh-100  align-items-center justify-content-center d-flex"  id="modal">
		 <section id="modal__content" class="container bg-white py-3 ">
			<div class="d-flex justify-content-between" id="modal__content__header">
				<h5 class=" " id="modal__content__header__title">{{title}}</h5>
				<a  href="#" class="btn-close"  @click.prevent="hide" ></a>
			</div>
			<div class="row justify-content-between" id="modal__content__body" v-if="passedFollowers.length" >
				<article v-for="f in passedFollowers" :key="f" class="col-12 row justify-content-between">
					<AuthorComponentVue  :image="f.profilePicture" :username="f.username" :id="f.id" :firstname="f.firstname" :lastname="f.lastname" @hide-modal="hide"></AuthorComponentVue>
					<article class="col-12 col-md-2 text-center text-md-end" v-if="userIsOnHisProfile">
						<a href="#" class="btn btn-light" v-if="title == 'Followers'" @click.prevent="removeFollower(f.id)">
							Remove
						</a>
						<a href="#" class="btn btn-light" v-else @click.prevent="removeFollowing(f.id)">
							Remove
						</a>
					</article>
				</article>
			</div>
			<div v-else>
				<p class=" ">The list is empty.</p>
			</div>
		 </section>
	</div>
</template>

<script>
import AuthorComponentVue from './AuthorComponent.vue';
import axios from "axios";
export default {
	inject : ["token", "baseURL"],
	components : {AuthorComponentVue},
	data(){
		return{
			passedFollowers : this.followers
		}
	},
	props : ["followers", "title" ],
	computed : {
		userIsOnHisProfile(){
			return this.token.get().UserId == this.$route.params.id.toString();
		}
	},
	methods : {
		hide(){
			this.$emit("hide-modal");
		},
		removeUserFromModal(id){
			this.passedFollowers = this.passedFollowers.filter(x => x.id != id);
		},
		remove(url, id){
			axios.delete(url)
			.then(res => {
				if(res.status == 204){
					this.$emit("get-followers-following");
					this.removeUserFromModal(id)
				}
			})
			.catch(()=>{
				this.$emit("server-error");
			});
		},
		removeFollower(id){
			const url = this.baseURL + "followers/user/" + this.token.get().UserId  + "/follower/"+ id;
			this.remove(url, id);
		},
		removeFollowing(id){
			const url = this.baseURL + "followers/user/" +id   + "/follower/"+ this.token.get().UserId;
			this.remove(url, id);
		}
	}
}
</script>

<style>
	#modal{
		position:fixed;
		z-index : 20;
		top:0;
		left: 0;
		background-color: rgba(0, 0, 0, 0.5);
		overflow: hidden;
	}
	#modal__content{
		border: 1px solid #fff;
		border-radius: 5px;
	}
	.vh-100{
		height: 100vh;
	}
	#modal__content__header__title{
		font-weight: bold;
	}
</style>