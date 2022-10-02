<template>
  <div class="container mx-auto row vh100 mx-0 align-items-center justify-content-center ">
  <LoginComponent @show-modal="showModalSetOutcome"></LoginComponent>
  <div class="col-12 col-md-4 row  flex-column align-items-center justify-content-start">
    <figure class="col-12 col-md-5">
      <img src="@/assets/images/logo.png" title="Owl"/>
    </figure>
  </div>
  </div>
  <ModalComponent v-if="showModal" @hide-modal="showModal=false" title="Sign in" successMessage="You signed in successfully." failureMessage="Given user doesn't exist" :outcome="outcome"></ModalComponent>
</template>

<script>
// @ is an alias to /src
import LoginComponent from "@/components/LoginComponent.vue";
export default {
  name: 'HomeView',
  components: {
    LoginComponent,
},
  data(){
    return {
      showModal : false,
      outcome :false 
    }
  },
  methods : {
    showModalSetOutcome(outcome){
      this.outcome = outcome;
      this.showModal = true;
    }
  }
}
</script>
