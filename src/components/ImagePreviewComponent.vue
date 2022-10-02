<template>
	<section class="col-5 row ">
		<figure class="col-8 col-md-3">
			<img :src="'http://localhost:5000/images/'+src" class="img-fluid" alt="preview"/>
		</figure>
		<article class="col-2 ">
			<a href="#" class="btn btn-danger border-radius-50" @click.prevent="onRemoveImageFromServer">&times;</a>
		</article>
	</section>
</template>	

<script>
	import axios from "axios";
	export default {
		inject : ["baseURL"],
		data(){
			return {};
		},
		props : ["src", "url"],
		methods : {
			onRemoveImageFromServer(){
				axios.delete(this.baseURL + this.url + this.src)
				.then(res => {
					if(res.status == 204){
						this.$emit("remove-image");
					}
				})
				.catch(()=>{
					this.$emit("server-error");
				});
			}
		}
	}
</script>
<style>
</style>
		