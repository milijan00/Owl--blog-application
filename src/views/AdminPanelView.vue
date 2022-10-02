<template>
	<NavComponent></NavComponent>
		<section class="container mx-auto vh100 py-3 ">
			<section class="row">
				<article class="col-12 mb-3">
					<h1 class="title">System data</h1>
				</article>
				<section class="col-12 mb-3 row">
					<article v-for="t in tables" :key="t" class="col-12 col-md-3 my-1">
						<a href="#" class="btn btn-light" @click.prevent="fetchData(t.path)">{{t.name}}</a>
					</article>
				</section>
				<article class="col-12 mb-3" v-if="data">
					<article>
						<h2 class="post-title">{{table.name}}</h2>
					</article >
					<article class='mb-3' v-if="insertionAllowed">
						<router-link :to="'/admin-panel/create/' + table.path" class="btn btn-primary"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-plus-lg" viewBox="0 0 16 16">
  <path fill-rule="evenodd" d="M8 2a.5.5 0 0 1 .5.5v5h5a.5.5 0 0 1 0 1h-5v5a.5.5 0 0 1-1 0v-5h-5a.5.5 0 0 1 0-1h5v-5A.5.5 0 0 1 8 2Z"/>
</svg></router-link>
					</article>
					<table class="table table-stripped  ">
						<thead>
							<tr>
								<th v-for="h in tableHeadings" :key="h">{{h[0].toUpperCase() + h.slice(1)}}</th>
								<th v-if="table.settings.adminCanEdit">Edit</th>
								<th v-if="table.settings.adminCanDelete">Delete</th>
							</tr>
						</thead >
						<tbody>
							<tr v-for="r in data" :key="r">
								<td v-for="p in objectValues(r)" :key="p">{{p}}</td>
								<td v-if="table.settings.adminCanEdit">
								<router-link :to="'/admin-panel/edit/'+ table.path + '/' + r.id">
<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-vector-pen" viewBox="0 0 16 16">
  <path fill-rule="evenodd" d="M10.646.646a.5.5 0 0 1 .708 0l4 4a.5.5 0 0 1 0 .708l-1.902 1.902-.829 3.313a1.5 1.5 0 0 1-1.024 1.073L1.254 14.746 4.358 4.4A1.5 1.5 0 0 1 5.43 3.377l3.313-.828L10.646.646zm-1.8 2.908-3.173.793a.5.5 0 0 0-.358.342l-2.57 8.565 8.567-2.57a.5.5 0 0 0 .34-.357l.794-3.174-3.6-3.6z"/>
  <path fill-rule="evenodd" d="M2.832 13.228 8 9a1 1 0 1 0-1-1l-4.228 5.168-.026.086.086-.026z"/>
</svg>
								</router-link>	
								</td>
								<td v-if="table.settings.adminCanDelete">
								<a href="#" @click.prevent="displayModalAndStoreId(r.id)">
<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash-fill" viewBox="0 0 16 16">
  <path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z"/>
</svg>
								</a>
								</td>
							</tr>
						</tbody>
					</table>
				</article>
				<article class="col-12 mb-3" v-else>
					<p class=" ">There are no data at the moment.</p>
				</article>
			</section>
		</section>
	<FooterComponent></FooterComponent>
	<YesNoModalComponent v-if="showModal" title="Delete record" message="Do You really want to delete this record?" @yes="deleteRecord" @no="showModal = false;"></YesNoModalComponent>
	<ModalComponent v-if="showRegularModal" @hide-modal="showRegularModal = false" title="Server error" failureMessage="An error ocurred while proccessing Your request." :outcome="false"></ModalComponent>
	<LoadingComponent v-if="loading"></LoadingComponent>
</template>

<script>
 import axios from "axios";
import YesNoModalComponent from "@/components/YesNoModalComponent.vue";
	function Table(name, path, settings){
		this.name = name;
		this.path = path;
		this.settings = settings;
	}

export default{
	inject : ['baseURL'],
	components : { YesNoModalComponent},
	data(){
		return {
			loading : false,
			tables : [
				new Table("Users management", "users", {adminCanEdit: false, adminCanDelete: true}) , 
				new Table("Categories management", "categories",{adminCanEdit: true, adminCanDelete: true}), 
				new Table("Navigation Links management","navigationLinks",{adminCanEdit: true, adminCanDelete: true}), 
				new Table("Roles management", "roles", {adminCanEdit: true, adminCanDelete: true}),
				new Table("Usecases management", "usecase", {adminCanEdit: true, adminCanDelete : true})
				],
			path : "",
			data : null,
			forbiddenColumnsToDisplay : ['id', "userId", "postId", 'profilePicture'],
			tablesThatAreAllowedToInsertNewRecord : ["Categories management", "Navigation Links management", "Roles management", "Usecases management"],
			showModal : false,
			recordId : 0,
			showRegularModal : false
		}
	},
	created(){
		this.path = this.tables[0].path;
		this.fetchData();
	},
	methods: {
		displayModalAndStoreId(id){
			this.showModal = true;
			this.recordId = id;
		},
		deleteRecord(){
			axios.delete(this.baseURL + this.table.path + "/" + this.recordId)
			.then(res => {
				if(res.status == 204){
					this.fetchData();
					this.showModal = false;
				}
			})
			.catch(()=>{
				this.showRegularModal = true;
			});
		},
		fetchData(path = null){
			this.path = path;
			if(!this.path){this.data= null; return ;} 
			axios.get(this.baseURL + this.path, {
				onDownloadProgress : ()=>{
					this.loading = true;
				}
			})
			.then(res => {
				if(res.status == 200 && res.data){
					this.loading = false;
					this.data = res.data;
				}
			})
			.catch(()=>{
				this.loading = false;
				this.showRegularModal = true;
			});
		},
		objectValues(obj){
			let values = [];
			for(let i in obj){
				if(!this.forbiddenColumnsToDisplay.includes(i)){
					values.push(obj[i]);
				}
			}
			return values;
		},
	},
	computed : {
		tableHeadings(){
			let headings = [];
			for(let i in this.data[0]){
				if(!this.forbiddenColumnsToDisplay.includes(i)){
					headings.push(i);
				}
			}
			return headings;
		},
		table(){
			return this.tables.find(t => t.path == this.path);
		},
		insertionAllowed(){
			return this.tablesThatAreAllowedToInsertNewRecord.includes(this.table.name);
		}
	}
}
</script>