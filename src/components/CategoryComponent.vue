<template>
	<span class="category   my-2">
		{{category}}
	</span>
</template>

<script>
export default {
	props: ["category"]
}
</script>

<style>
	.category{
		background-color: #e2e2e2;
		color: #3e4042;
		padding: 5px;
		border-radius: 7px;
	}
</style>