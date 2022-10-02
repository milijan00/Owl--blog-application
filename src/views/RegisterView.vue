<template>
	<section class="container-fluid vh100 d-flex justify-content-center align-items-center py-4" >
		<section class="container mx-auto ">
			<article>
				<h1 class="post-title">Create an account</h1>
			</article>
			<article>
				<form name="formregister" method="POST" action="#" @submit.prevent="onSubmit">
					<section class="row ">
						<section class="col-12 row mx-0 px-0 mb-3 justify-content-between">
							<article class="col-12 col-md-6 pl-0 mb-3 mb-md-0">
								<input type="text" placeholder="Firstname" class="form-control" v-model="firstname"/>
								<ErrorBoxComponent v-if="firstnameErrorNumber" :messages="this.validationManager.errors.messages.firstname"></ErrorBoxComponent>
							</article>
							<article class="col-12 col-md-6 pr-0">
								<input type="text" placeholder="Lastname" class="form-control" v-model="lastname"/>
								<ErrorBoxComponent v-if="lastnameErrorNumber" :messages="this.validationManager.errors.messages.lastname"></ErrorBoxComponent>
							</article>
						</section>
						<article class="col-12 mb-3">
							<input type="text" placeholder="Username" class="form-control" v-model="username"/>
								<ErrorBoxComponent v-if="usernameErrorNumber" :messages="this.validationManager.errors.messages.username"></ErrorBoxComponent>
						</article>
						<article class="col-12 mb-3">
							<input type="email" placeholder="Email" class="form-control" v-model="email"/>
								<ErrorBoxComponent v-if="emailErrorNumber" :messages="this.validationManager.errors.messages.email"></ErrorBoxComponent>
						</article>
						<article class="col-12 mb-3">
							<section class="row mx-0 px-0">
								<article class="col-11 px-0">
									<input :type="passwordType" placeholder="Password" class="form-control" v-model="password"/>
								</article>
								<ShowHidePasswordComponent @hide="passwordType='password'" @show="passwordType='text'"></ShowHidePasswordComponent>
							</section>
								<ErrorBoxComponent v-if="passwordErrorNumber" :messages="this.validationManager.errors.messages.password"></ErrorBoxComponent>
						</article>
						<article class="col-12 mb-3">
							<section class="row mx-0 px-0">
								<article class="col-11 px-0 ">
									<input :type="passwordAgainType" placeholder="Password again" class="form-control" v-model="passwordAgain"/>
								</article>
								<ShowHidePasswordComponent @hide="passwordAgainType='password'" @show="passwordAgainType='text'"></ShowHidePasswordComponent>
							</section>
								<ErrorBoxComponent v-if="passwordAgainErrorNumber" :messages="this.validationManager.errors.messages.passwordAgain"></ErrorBoxComponent>
						</article>
						<article class="col-12 mb-3 d-flex justify-content-end">
							<input type="submit" value="Done" class="btn btn-blue"/>
						</article>
					</section>
				</form>
				<article>
						<p class=" ">
							Do You have an account? Sign in 
							<router-link to="/">here</router-link>.
						</p>
				</article>
			</article>
		</section>
	</section>
	<ModalComponent v-if="showModal" title="Create a new account" successMessage="You have created your account successfully." failureMessage="There was an error processing Your request." :outcome="outcome" @hide-modal="showModal=false" ></ModalComponent>
	<loadingComponent v-if="loading"></loadingComponent>
</template>


<script>
import ErrorBoxComponent from '@/components/ErrorBoxComponent.vue';
import axios from "axios";
import ShowHidePasswordComponent from '@/components/ShowHidePasswordComponent.vue';
export default {
	components : {
    ErrorBoxComponent,
    ShowHidePasswordComponent,
},
	inject : ["validationManager", "baseURL", "errorMessages", ],
	data(){
		return {
			loading: false,
			firstname : "",
			lastname : "",
			username : "",
			email : "",
			password : "",
			passwordAgain : "",
			usernameErrorNumber : 0,
			firstnameErrorNumber : 0,
			lastnameErrorNumber : 0,
			emailErrorNumber : 0,
			passwordErrorNumber : 0,
			passwordAgainErrorNumber : 0,
			showModal : false,
			outcome : true,
			passwordType : "password",
			passwordAgainType : 'password'
		}
	},
	created(){
		this.initializeErrorMessages();
	},
	methods : {
		initializeErrorMessages(){
			this.validationManager.errors.messages = {
				firstname : [],
				lastname : [],
				email : [],
				password : [],
				passwordAgain : [],
				username : []
			};
		},
		setErrorNumbers(){
			this.firstnameErrorNumber = this.validationManager.errors.messages.firstname.length;
			this.lastnameErrorNumber = this.validationManager.errors.messages.lastname.length;
			this.emailErrorNumber = this.validationManager.errors.messages.email.length;
			this.passwordErrorNumber = this.validationManager.errors.messages.password.length;
			this.passwordAgainErrorNumber = this.validationManager.errors.messages.passwordAgain.length;
			this.usernameErrorNumber = this.validationManager.errors.messages.username.length;

		},
		resetErrorNumbers(){
			this.firstnameErrorNumber = 0;
			this.lastnameErrorNumber = 0;
			this.emailErrorNumber = 0;
			this.passwordErrorNumber = 0;
			this.passwordAgainErrorNumber = 0;
			this.usernameErrorNumber =   0;
		},
		clearForm(){
			this.firstname = "";
			this.lastname = "";
			this.email = "";
			this.password = "";
			this.passwordAgain = "";
			this.username = "";
		},
		valid(){
			this.validationManager.errors.reset();
			this.resetErrorNumbers();
			if(!this.firstname.match(this.validationManager.regexes.firstOrLastname)) this.validationManager.errors.messages.firstname.push(this.errorMessages.firstOrLastname);
			

			if(!this.lastname.match(this.validationManager.regexes.firstOrLastname)) this.validationManager.errors.messages.lastname.push(this.errorMessages.firstOrLastname);

			if(!this.email.match(this.validationManager.regexes.email)) this.validationManager.errors.messages.email.push(this.errorMessages.email);

			if(!this.password.match(this.validationManager.regexes.password)) this.validationManager.errors.messages.password.push(this.errorMessages.password);

			if(!this.passwordAgain.match(this.validationManager.regexes.password)) this.validationManager.errors.messages.passwordAgain.push(this.errorMessages.password);
		
			if(!this.username.match(this.validationManager.regexes.username)) this.validationManager.errors.messages.username.push(this.errorMessages.username);
			
			if(this.password != this.passwordAgain) this.validationManager.errors.messages.passwordAgain.push("Both passwords must be the same.");
		
			if(this.validationManager.validationInvalid()){
				this.setErrorNumbers();
				return false;
			}
			return true;
		},
		sendRequest(){
			axios.post(this.baseURL + "users", {
				firstname : this.firstname,
				lastname : this.lastname,
				username : this.username,
				email : this.email,
				password : this.password
			}, {
				onUploadProgress : ()=>{
					this.loading = true;
				}
			})
			.then(res => {
				if(res.status == 201){
					this.loading = false;
					this.outcome = true;
					this.showModal = true;
					this.clearForm();
				}
			}).
			catch(err =>{
				this.loading  =false;
				this.outcome = false;
				this.showModal = true;
				console.error(err);
			});
		},
		onSubmit(){
			if(this.valid()){
				this.sendRequest();
			}
		}
	}
}
</script>