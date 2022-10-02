import { createRouter, createWebHistory } from 'vue-router'
import RegisterView from "../views/RegisterView.vue";
import HomeView from "../views/HomeView.vue";
import ProfileView from "@/views/ProfileView.vue";
import CreatePostView from "@/views/CreatePostView.vue";
import ShowPostView from "@/views/ShowPostView.vue";
import EditPostView from "@/views/EditPostView.vue";
import EditProfileView from "@/views/EditProfileView.vue"
import BrowseView from "@/views/BrowseView.vue";
import AdminPanelView from "@/views/AdminPanelView.vue";
import CreateRecordView from "@/views/CreateRecordView.vue" ;
import EditRecordView from "@/views/EditRecordView.vue" ;
import NotFoundView from "@/views/NotFoundView.vue";
const routes = [
  {
    path: '/register',
    name: 'register',
    component:RegisterView ,
    meta : {requiresUnauth : true}
  },
  {
    path: '/not-found',
    name: 'not-found',
    component: NotFoundView ,
    meta : {requiresAuth : true}
  },
  {
    path: '/',
    name: 'home',
    component: HomeView ,
    meta : {requiresUnauth : true}
  },
  {
    path: '/profile/:id',
    name: 'profile',
    component: ProfileView ,
    meta : {requiresAuth : true}
  },
  {
    path: '/browse',
    name: 'browse',
    component: BrowseView ,
    meta : {requiresAuth : true}
  },
  {
    path: '/posts/create/',
    name: 'create post',
    component: CreatePostView ,
    meta : {requiresAuth : true}
  },
  {
    path: '/posts/:id/',
    name: 'show post',
    component: ShowPostView ,
    meta : {requiresAuth : true}
  },
  {
    path: '/posts/edit/:id/',
    name: 'edit post',
    component: EditPostView ,
    meta : {requiresAuth : true}
  },
  {
    path: '/profile/edit/:id/',
    name: 'edit profile',
    component: EditProfileView ,
    meta : {requiresAuth : true}
  },
  {
    path: '/admin-panel',
    name: 'admin panel',
    component: AdminPanelView ,
    meta : {requiresAuth : true, requiresAdmin : true}
  },
  {
    path: '/admin-panel/create/:table',
    name: 'admin panel create category',
    component: CreateRecordView ,
    meta : {requiresAuth : true, requiresAdmin : true}
  },
  {
    path: '/admin-panel/edit/:table/:id',
    name: 'admin panel edit category',
    component: EditRecordView ,
    meta : {requiresAuth : true, requiresAdmin : true}
  },
  // {
  //   path: '/about',
  //   name: 'about',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
  //   component: () => import(/* webpackChunkName: "about" */ '../views/AboutView.vue')
  // }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
