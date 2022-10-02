<template>
	<NavComponent></NavComponent>
	<section class="container mx-auto vh80  py-3 d-flex flex-column justify-content-center">
		<CreateCategoryComponent v-if="userWantsToCreate('categories')" @load="loading = true" @stop-loading="loading = false"></CreateCategoryComponent>
		<CreateNavigationLinkComponent v-if="userWantsToCreate('navigationLinks')" @load="loading = true" @stop-loading="loading = false"></CreateNavigationLinkComponent>
		<CreateRoleComponent v-if="userWantsToCreate('roles')" @load="loading = true" @stop-loading="loading = false"></CreateRoleComponent>
		<CreateUseCaseComponent v-if="userWantsToCreate('usecase')" @load="loading = true" @stop-loading="loading = false"></CreateUseCaseComponent>
	</section>
	<FooterComponent></FooterComponent>
	<LoadingComponent v-if="loading"></LoadingComponent>
</template>

<script>
import CreateCategoryComponent from '@/components/CreateCategoryComponent.vue';
import CreateNavigationLinkComponent from '@/components/CreateNavigationLinkComponent.vue';
import CreateRoleComponent from '@/components/CreateRoleComponent.vue';
import CreateUseCaseComponent from '@/components/CreateUseCaseComponent.vue';
import LoadingComponent from '@/components/LoadingComponent.vue';
export default{
	components: { CreateCategoryComponent, CreateNavigationLinkComponent, CreateRoleComponent, CreateUseCaseComponent, LoadingComponent },
	data() {
		return {
			loading: false
		}
	},
	methods : {
		userWantsToCreate(table){
			return this.$route.params.table == table;
		}
	}
}
</script>