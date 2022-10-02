<template>
	<NavComponent></NavComponent>
	<section class="container mx-auto vh80  py-3 d-flex flex-column justify-content-center">
		<EditCategoryComponent v-if="userWantsToEdit('categories')"  @load="loading = true" @stop-loading="loading = false"></EditCategoryComponent>	
		<EditNavigationLinkComponent v-else-if="userWantsToEdit('navigationLinks')" @load="loading = true" @stop-loading="loading = false"></EditNavigationLinkComponent>
		<EditRoleComponent v-if="userWantsToEdit('roles')" @load="loading = true" @stop-loading="loading = false"></EditRoleComponent>
		<EditUseCaseComponent v-if="userWantsToEdit('usecase')" @load="loading = true" @stop-loading="loading = false"></EditUseCaseComponent>
	</section>
	<FooterComponent></FooterComponent>
	<LoadingComponent v-if="loading"></LoadingComponent>
</template>

<script>
import EditCategoryComponent from '@/components/EditCategoryComponent.vue';
import EditNavigationLinkComponent from "@/components/EditNavigationLinkComponent.vue";
import EditRoleComponent from '@/components/EditRoleComponent.vue';
import EditUseCaseComponent from '@/components/EditUseCaseComponent.vue';
export default{
	components : { EditCategoryComponent, EditNavigationLinkComponent, EditRoleComponent, EditUseCaseComponent },
	data(){
		return {
			loading : false
		}
	},
	methods : {
		userWantsToEdit(table){
			return this.$route.params.table == table;
		}
	}
}
</script>