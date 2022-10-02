<template>
	<div class="container-fluid vh-100  align-items-center justify-content-center d-flex"  id="modal">
		 <section id="modal__content" class="container bg-white py-3 ">
			<div class="d-flex justify-content-between" id="modal__content__header">
				<h5 class=" " id="modal__content__header__title">{{title}}</h5>
				<a  href="#" class="btn-close"  @click.prevent="hide" ></a>
			</div>
			<div class="" id="modal__content__body">
				<p class=" ">{{message}}</p>
			</div>
			<div class="d-flex justify-content-end" id="modal__content__footer">
				<article class="mx-2">
					<a href="#" class="btn btn-blue  " @click.prevent="yes">Yes</a>
				</article>
				<article>
					<a href="#"  class="btn btn-danger  "  @click.prevent="hide">No</a>
				</article>
				
			</div>
		 </section>
	</div>
</template>

<script>
export default {
	data(){
		return{
		}
	},
	props : ["message", 'title'  ],
	methods : {
		hide(){
			this.$emit("no");
		},
		yes(){
			this.$emit("yes");
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