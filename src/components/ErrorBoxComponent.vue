<template>
	<section class="alert alert-danger mt-2 border-radius-50">
		<article v-for="m in messages" :key="m">
			<p>{{m}}</p>
		</article>
	</section>
</template>

<script>
export default{
	props : ["messages"]
}
</script>