import NavigationComponent from "@/components/NavigationComponent.vue";

export default {
	install(app){
		app.component("NavComponent", NavigationComponent);
	}
}