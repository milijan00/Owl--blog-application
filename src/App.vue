<template>
  <router-view/> 
</template>

<style>
/* @import url('https://fonts.googleapis.com/css2?family=Roboto+Slab&display=swap');

@import url('https://fonts.googleapis.com/css2?family=Merriweather&display=swap'); */

@import url('https://fonts.googleapis.com/css2?family=Libre+Baskerville&display=swap');

@import url('https://fonts.googleapis.com/css2?family=Poppins&display=swap');

*{
  font-family : 'Poppins', sans-serif;

}

/* #app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
} */
.vh100{
 min-height: 100vh;
}

.vh80{
  min-height: 80vh;
}


nav {
  padding: 30px;
  text-align: center;
}
a{
  text-decoration: none;
}

nav a {
  font-weight: bold;
  color: #2c3e50;
}

nav a.router-link-exact-active {

  color: #2f6fde;
}
.light-grey-bg {
  background-color: #e8e9eb;
}

.btn.btn-blue{
  background-color: #2f6fde;
  color: #fff;
}

.btn.btn-dark-grey{
  background-color: #323436;
  color: #fff;
}

.light-grey-fg,
.title,
.post-title{
  color: #3e4042;
}

.title{
  font-size: 3rem;
}

.post-title{
  font-size: 2rem;
}

.small-title{
  font-size: 1.5rem;
}


.box-shadow{

	box-shadow: 5px 3px 20px #b6b6b8;
}

.border-radius-7{
  border-radius: 7px;
}



/* . , 
.title,
.post-title,
 nav a,
 .small-title,
 .form-control,
 .dropdown-item,
 .btn-blue {
  font-family: 'Libre Baskerville', serif;
} */
.text-justify,
.post-content{
  text-align: justify;
}
.post-content{
  line-height: 2;
}

.cursor-pointer{
  cursor: pointer;
}

.border-radius-50, 
.btn,
.form-control
{
  border-radius :50px;
}
</style>
<script>
export default {
  components : {
}
} 
</script>