import LoadingComponentVue from "../components/LoadingComponent.vue";
export default {
	install(app){
		app.component("LoadingComponent", LoadingComponentVue);
	}
}
	