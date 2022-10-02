
<template>
	<article>
		<a href="#" class="btn btn-primary" @click.prevent="onUploadAllImages">Upload</a>
	</article>
</template>	
<script>
	import axios from "axios";
	export default {
		inject : ["baseURL", "token"],
		props : ["sources", "url", "current"],
		methods : {
			uploadPostsImage(s){
				alert(s);
			},
			uploadUsersImage(s){
				axios.patch(this.baseURL + this.url, {
					id : this.token.get().UserId,
					profilePicture : s,
					previousProfilePicture : this.current
				})
				.then(res => {
					if(res.status == 204){
						this.$emit("image-uploaded");
					}
				}).
				catch(()=>{
					this.$emit("server-error");
				});
			},
			uploadImage(s){
				// PATCH users/ {profilePicture : src}
				// 
				switch(this.url){
					case "users/" : {
						this.uploadUsersImage(s);
						break;
					}
					case "postsimages/" : {
						this.uploadPostsImage(s);
						break;
					}
				}
			},
			onUploadAllImages(){
				for(let s of this.sources){
					this.uploadImage(s)	;
				}
			}
		}
	}
</script>
<style>
</style>
		