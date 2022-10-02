
<template>
<section class="container-fluid vh100 d-flex align-items-center justify-content-center flex-column"  id="loading--wrapper">
	<!-- <article class="" id="bar">
		<div class="" id="loading"></div>
	</article> -->
	<article id="spinner"></article>
</section>
</template>	
<script>
	export default {
		data(){
			return {};
		}
	}
</script>
<style>
	#loading--wrapper{
		position:fixed;
		top: 0;
		z-index: 101;
		/* background-color: rgba(0, 0, 0, 0.5); */
		background-color: rgba(250, 250, 250, 1);
	}
	#loading--wrapper p{
		color: #fff;
	}
	#loading{
		width: 30px;
		height: 5px;
		position: relative;
		background-color:#2f6fde;
		animation: movingAnimation;
		animation-duration: 5s;
		animation-iteration-count: infinite;
	}
	@media screen and (max-width: 320px){
		#bar{
			width: 50%;
		}
	}
	@media screen and (min-width: 321px){
		#bar {
			width: 5%;
		}
	}

	#spinner{
		width: 250px;
		height: 250px;
		background-image : url("../assets/images/spinner.gif");
		background-position: center;
		background-size: cover;
	}
</style>
		