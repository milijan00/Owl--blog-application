<template>
<NavComponent></NavComponent>
<section class="container mx-auto py-3 vh80">
	<section class="row justify-content-center">
		<article class="col-12">
			<h1 class="post-title text-center">Edit Your profile</h1>
		</article>
		<article class="col-12 col-md-6">
			<form method="POST" action="#" name="formEditProfile" >
				<section class="row justify-content-center">
					<figure class="col-6 col-md-3  " v-if="currentImage">
						<img :src="'http://localhost:5000/images/' + currentImage" class="img-fluid rounded-circle" alt="profile picture"/>
					</figure>
					<article  class="col-12 mb-3">
						<section class="w-100 d-flex">
							<input type="text" placeholder="Firstname" class="form-control" v-model="firstname"/>
							<input type="button" class="btn btn-primary mx-2 " value="Edit" @click="onSubmit('firstname', {id: token.get().UserId, firstname : firstname})"/>
						</section>
						<ErrorBoxComponent v-if="firstnameErrorNumber" :messages="validationManager.errors.messages.firstname"></ErrorBoxComponent>
					</article>
					<article  class="col-12 mb-3">
						<section class="d-flex w-100">
							<input type="text" placeholder="Lastname" class="form-control" v-model="lastname"/>
						<input type="button" class="btn btn-primary mx-2" value="Edit" @click="onSubmit('lastname', {id: token.get().UserId, lastname : lastname})"/>
						</section>
						<ErrorBoxComponent v-if="lastnameErrorNumber" :messages="validationManager.errors.messages.lastname"></ErrorBoxComponent>
					</article>
					<article  class="col-12 mb-3">
						<section class="w-100 d-flex">
							<input type="text" placeholder="Username" class="form-control" v-model="username"/>
							<input type="button" class="btn btn-primary mx-2 " value="Edit" @click="onSubmit('username', {id: token.get().UserId, username : username})"/>
						</section>
						<ErrorBoxComponent v-if="usernameErrorNumber" :messages="validationManager.errors.messages.username"></ErrorBoxComponent>
					</article>
					<article  class="col-12 mb-3">
						<section class="d-flex w-100">
							<input type="email" placeholder="Email" class="form-control" v-model="email"/>
							<input type="button" class="btn btn-primary mx-2 " value="Edit" @click="onSubmit('email', {id: token.get().UserId, email : email})"/>
						</section>
						<ErrorBoxComponent v-if="emailErrorNumber" :messages="validationManager.errors.messages.email"></ErrorBoxComponent>
					</article>
					<article  class="col-12 mb-3">
						<section class="row mx-0 px-0">
							<article class="col-11 px-0">
								<input :type="passwordType" placeholder="Password" class="form-control" v-model="password"/>
							</article>
							<ShowHidePasswordComponent @hide="passwordType = 'password'" @show="passwordType='text'"></ShowHidePasswordComponent>	
						</section>
						<ErrorBoxComponent v-if="passwordErrorNumber" :messages="validationManager.errors.messages.password"></ErrorBoxComponent>
					</article>
					<article  class="col-12 mb-3">
						<section class="row mx-0 px-0">
							<article class="col-11 px-0">
								<input :type="newPasswordType" placeholder="New password" class="form-control" v-model="newPassword"/>
							</article>
							<ShowHidePasswordComponent @hide="newPasswordType = 'password'" @show="newPasswordType='text'"></ShowHidePasswordComponent>	
						</section>
						<ErrorBoxComponent v-if="newPasswordErrorNumber" :messages="validationManager.errors.messages.newPassword"></ErrorBoxComponent>
					</article>
					<article class="col-12 mb-3">
						<input type="button" class="btn btn-primary mx-2 " value="Edit" @click="onSubmit('password', {id: token.get().UserId, password : newPassword, previousPassword: password})"/>
					</article>					
					<article class="">
						<p class="small-title">Change your profile picture.</p>
					</article>
					<article class="col-12 mb-3">
						<input type="file" class="form-control" @change="onFileSelected"  />
						<ErrorBoxComponent v-if="selectedFileErrorNumber" :messages="validationManager.errors.messages.selectedFile"></ErrorBoxComponent>
					</article>
					<article  class="col-12 mb-3 text-end">
						<input type="button" value="Delete profile" class="btn btn-danger   mx-2" @click="deleteProfile"/>
						<router-link :to="'/profile/' + token.get().UserId" class="btn btn-light  ">Go back</router-link>
					</article>
				</section>
			</form>
		</article>
		<section v-if="src" class="col-12 d-flex flex-column align-items-center">
			<article>
				<h3>Image preview</h3>
			</article>
			<ImagePreviewComponent  url="users/image/" :src="src" @remove-image="src=''" @server-error="serverError"></ImagePreviewComponent>
			<UploadImageComponent :sources="[src]" :current="currentImage" url="users/" @image-uploaded="onImageUploaded" @server-error="serverError"></UploadImageComponent>
		</section>
	</section>
</section>
<FooterComponent></FooterComponent>
<ModalComponent v-if="showModal" @hide-modal="showModal = false;" title="Edit profile" successMessage="You have edited Your profile successfully." :failureMessage="failureMessage" :outcome="outcome"></ModalComponent>
<LoadingComponent v-if="loading"></LoadingComponent>
</template>

<script>
import axios from "axios";
import ErrorBoxComponent from "@/components/ErrorBoxComponent.vue";
import ShowHidePasswordComponent from "@/components/ShowHidePasswordComponent.vue";
import ImagePreviewComponent from "@/components/ImagePreviewComponent.vue";
import UploadImageComponent from "@/components/UploadImageComponent.vue";
export default {
	components : { ErrorBoxComponent, ShowHidePasswordComponent, ImagePreviewComponent,UploadImageComponent },
	inject: ["token", "baseURL", "validationManager", "errorMessages"]	,
	data(){
		return {
			loading: false,
			iWantToUpdatePassword : false,
			firstname: "",
			lastname : "",
			username : "",
			email : "",
			password : "",
			newPassword : "",
			showModal : false,
			outcome : false,
			firstnameErrorNumber: 0,
			lastnameErrorNumber: 0,
			usernameErrorNumber : 0,
			emailErrorNumber: 0,
			passwordErrorNumber: 0,
			newPasswordErrorNumber: 0,
			passwordType : "password",
			newPasswordType : "password",
			selectedFile : null,
			selectedFileErrorNumber : 0,
			currentImage : "",
			src : "",
			failureMessage : ""
		}
	},
	created(){
		this.validationManager.errors.messages = {
			firstname : [],
			lastname : [],
			username : [],
			email : [],
			password : [],
			newPassword : [],
			selectedFile : []
		};

		axios.get(this.baseURL + "users/short/" + this.token.get().UserId, {
			onDownloadProgress : ()=>{
				this.loading = true;
			}
		})
		.then(res => {
			if(res.status == 200 && res.data){
				this.loading = false;
				this.firstname = res.data.firstname;
				this.lastname = res.data.lastname;
				this.email = res.data.email;
				this.username = res.data.username;
				this.password = res.data.password;
				this.newPassword = res.data.passwordAgain;
				this.currentImage = res.data.imageSource;
			}
		})
		.catch((err)=>{
			this.loading = false;
			if(err.response.status  != 401){
				this.serverError();
			}
		});
	},
	methods : {
		onImageUploaded(){
			this.src = "";
			this.outcomeSuccessfull();
		},
		outcomeSuccessfull(){
			this.showModal = true;
			this.outcome = true;
		},
		error422(error){
			this.showModal = true;
			this.outcome = false;
			this.failureMessage = error.errorMessage;
		},
		serverError(){
			this.showModal = true;
			this.outcome = false;
			this.failureMessage = "Something went wrong";
		},
		onFileSelected(event){
			
			this.selectedFile = event.target.files[0];
			if(this.selectedFile && this.valid("file")){
				this.selectedFileErrorNumber = 0;
				var form = new FormData();
				form.append("image", this.selectedFile, this.selectedFile.name);
				axios.post(this.baseURL + "users/image", form)
				.then(res => {
					this.src = res.data.image;
				}).
				catch((err)=>{
					if(err.response.status != 401){
						this.serverError();
					}
				});
			}
		},
		deleteProfile(){
			const id = this.$route.params.id;
			axios.delete(this.baseURL + "users/" + id)
			.then(res => {
				if(res.status == 204){
					this.$router.push("/");
					localStorage.removeItem("token");
					localStorage.removeItem("refresh");
				}
			});
		},
		valid(entry){
			this.validationManager.errors.reset();
			this.resetErrorNumbers();
			
			if(entry === "firstname" && !this.firstname.match(this.validationManager.regexes.firstOrLastname))	this.validationManager.errors.messages.firstname.push(this.errorMessages.firstOrLastname);

			if( entry == "lastname" && !this.lastname.match(this.validationManager.regexes.firstOrLastname))	this.validationManager.errors.messages.lastname.push(this.errorMessages.firstOrLastname);

			if( entry == "username" && !this.username.match(this.validationManager.regexes.username))	this.validationManager.errors.messages.username.push(this.errorMessages.username);

			if( entry == "email" && !this.email.match(this.validationManager.regexes.email))	this.validationManager.errors.messages.email.push(this.errorMessages.email);

			if(entry == "password" && !this.password.match(this.validationManager.regexes.password))	this.validationManager.errors.messages.password.push("Password  is in wrong format.");
			if(entry == "password" && !this.newPassword.match(this.validationManager.regexes.password))	this.validationManager.errors.messages.newPassword.push("New password is in wrong format.");

			if(this.newPassword == this.password)	this.validationManager.errors.messages.newPassword.push("Both passwords must be different.");
			if(entry == "file" && this.selectedFile.type != "image/jpeg"){
				this.validationManager.errors.messages.selectedFile.push("The selected file's extension has to be one of these : .jpg, .jpeg or .png .");
			}
			
			if(this.validationManager.validationInvalid()){
				this.setErrorNumbers();
				return false;
			}
			return true
		},
		sendRequest(body){
			axios.patch(this.baseURL + "users", body, {
				onUploadProgress : ()=>{
					this.loading  =true;
				}
			})
			.then(res =>{
				if(res.status == 204){
					this.loading = false
					this.outcomeSuccessfull();
				}
			})
			.catch((err)=>{
				this.loading = false;
				if(err.response.status == 422){
					this.error422(err.response.data[0]);
				}
				else if(err.response.status != 401){
					this.serverError();
				}
			})
		},
		onSubmit(entry, body){
			if(this.valid(entry)){
				this.sendRequest(body);
			}
		},
		clearForm(){
			this.firstname = "";
			this.lastname = "";
			this.email = "",
			this.username = "",
			this.password = ""
			this.newPassword = "";
		},
		setErrorNumbers(){
			this.firstnameErrorNumber = this.validationManager.errors.messages.firstname.length;
			this.lastnameErrorNumber = this.validationManager.errors.messages.lastname.length;
			this.usernameErrorNumber = this.validationManager.errors.messages.username.length;
			this.emailErrorNumber = this.validationManager.errors.messages.email.length;
			this.passwordErrorNumber = this.validationManager.errors.messages.password.length;
			this.newPasswordErrorNumber = this.validationManager.errors.messages.newPassword.length;
			this.selectedFileErrorNumber = this.validationManager.errors.messages.selectedFile.length;
		},
		resetErrorNumbers(){
			this.firstnameErrorNumber = 0;
			this.lastnameErrorNumber = 0;
			this.usernameErrorNumber = 0;
			this.emailErrorNumber = 0;
			this.passwordErrorNumber =0;
			this.newPasswordErrorNumber = 0;
			this.selectedErrorNumber = 0;
		}

	}
}
</script>
