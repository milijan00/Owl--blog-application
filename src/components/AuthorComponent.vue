<template>
	<section class="row col-12 col-md-7 px-0 mx-0 justify-content-center justify-content-md-start">
		<article class="col-5 col-md-1 mb-3 mb-md-0 ">
			<img :src="'http://localhost:5000/images/' + image" alt="" class="img-fluid rounded-circle border"/>
		</article>
		<article class="col-12 col-md-8 ">
			<article class=" text-center text-md-start" >
				<router-link :to="'/profile/' + id" class="   light-grey-fg " v-if="username" @click.prevent="onRedirectToUsersProfile(id)">{{username}}</router-link>
			</article>
			<article v-if="username && firstname && lastname" >
				<p class="text-secondary   text-center text-md-start">
					{{firstname}} {{lastname}}
				</p>
			</article>
			<article v-else class="text-center text-md-start">
				<router-link :to="'/profile/' + id" class="   light-grey-fg " @click.prevent="onRedirectToUsersProfile(id)">{{firstname}} {{lastname}}</router-link>
			</article>
		</article>
	</section>
</template>

<script>
export default {
	props : ["image", "username", "id", "firstname", "lastname"],
	methods : {
		onRedirectToUsersProfile(id){
			this.onHideModal();
			this.$router.push("/profile/" + id);
		},
		onHideModal(){
			this.$emit("hide-modal");
		}
	}
}
</script>
