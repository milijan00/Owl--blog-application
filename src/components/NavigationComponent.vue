<template>

<nav class="navbar navbar-expand-lg bg-white box-shadow">
  <div class="container mx-auto  ">
    <router-link class="navbar-brand" :to="'/profile/' + token.get().UserId">
      <figure id="logo-wrapper">
        <img src="@/assets/images/logo.png" class="img-fluid" alt="logo" title="Profile"/>
      </figure>
      </router-link>
    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navbarSupportedContent">
      <ul class="navbar-nav me-auto mb-2 mb-lg-0">
        <li v-for="l in links" :key="l">
          <router-link v-if="l.name == 'Profile'" class="nav-link" :to="l.route + '/' + token.get().UserId">Profile</router-link>
          <router-link v-else :to="l.route" class="nav-link">{{l.name}}</router-link>
        </li>
        <li v-if="token.get().Role == 'Administrator'">
          <router-link to="/admin-panel" class="nav-link" >Admin panel</router-link>
        </li>
        <li >
          <router-link to="/" class="nav-link" @click.prevent="logout">Logout</router-link>
        </li>
      </ul>
    </div>
	</div>
	</nav>
  <ModalComponent v-if="showModal" @hide-modal="showModal = false" title="Server error" failureMessage="An error ocurred while processing Your request." :outcome="false"></ModalComponent>
</template>

<script>
import axios from "axios";
export default {
  inject : ["baseURL", "token"],
  data(){
    return  {
      links : [],
      showModal : false
    }
  },
  created(){
    this.loadNavLinks();    
  },
  methods : {
    logout(){
      localStorage.removeItem("token");
      localStorage.removeItem("refresh");
      this.$router.push("/");
    },
    loadNavLinks(){
      axios.get(this.baseURL + "navigationLinks")
      .then(res =>{
        if(res.status == 200 && res.data){
          this.links = res.data;
        }
      })
      .catch(()=>{
        this.showModal = true;
      })
    }

  }
}

</script>

<style>
#logo-wrapper{
  width : 50px;
}
.navbar{
  position:sticky;
  top:0;
  z-index: 19;
}
</style>