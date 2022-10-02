<template>
	<NavComponent></NavComponent>
	<section class="container mx-auto vh100 py-3">
		<section class="mb-3 d-flex flex-column ">
			<div><h1 class="title">{{post.title}}<span class="small-title m-0"> </span></h1></div>
			<AuthorComponent :id="post.userId" :image="post.profilePicture" :firstname="post.firstname" :lastname="post.lastname"></AuthorComponent>
		</section>
		<article>
			<p class="text-secondary  ">Created:{{post.createdAt}}</p>
		</article>
		<section class="d-flex ">
		<article class="mx-2">
			<p class=" ">Likes : {{post.likes.length}} <a href="#" v-if="userDidntLikePost" @click.prevent="likePost"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-heart" viewBox="0 0 16 16">
  <path d="m8 2.748-.717-.737C5.6.281 2.514.878 1.4 3.053c-.523 1.023-.641 2.5.314 4.385.92 1.815 2.834 3.989 6.286 6.357 3.452-2.368 5.365-4.542 6.286-6.357.955-1.886.838-3.362.314-4.385C13.486.878 10.4.28 8.717 2.01L8 2.748zM8 15C-7.333 4.868 3.279-3.04 7.824 1.143c.06.055.119.112.176.171a3.12 3.12 0 0 1 .176-.17C12.72-3.042 23.333 4.867 8 15z"/>
</svg>
</a>
<a href="#" v-else @click.prevent="dislikePost">
<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-heart-fill" viewBox="0 0 16 16">
  <path fill-rule="evenodd" d="M8 1.314C12.438-3.248 23.534 4.735 8 15-7.534 4.736 3.562-3.248 8 1.314z"/>
</svg>
</a>
</p>	
		</article>
		<span v-if="post">
			<p class="  cursor-pointer"  ><a href="#comment-section">Comments</a>: {{post.comments.length + numberOfChildComments}}</p>
		</span>
		</section>
			<CategoryComponentVue :category="post.category"></CategoryComponentVue>
		<article class="pt-2">
			<p class="  post-content" ref="postBody"></p>
		</article>
	</section>
	<CommentSectionComponent  @server-error="showModal=true; outcome=false;" @refresh-comments="refreshComments" @hide-comments="showComments = false" :comments="post.comments"></CommentSectionComponent>
	<FooterComponent></FooterComponent>
	<ModalComponent v-if="showModal" @hide-modal="showModal = false" title="Server error" failureMessage="An error ocurred while proccessing Your request." :outcome="false" ></ModalComponent>
</template>

<script>
import CategoryComponentVue from "@/components/CategoryComponent.vue";
import CommentSectionComponent from "@/components/CommentSectionComponent.vue";
 import axios from "axios";
import AuthorComponent from "@/components/AuthorComponent.vue";
	export default {
		components: {
    CategoryComponentVue,
    CommentSectionComponent,
    AuthorComponent
},
		inject: ["baseURL", "token"],
		data(){
			return {
				showModal : false,
				showComments : false,
				post : {
					comments: [],
					likes : []
				}
			}
		},
		computed: {
			userDidntLikePost(){
				return this.post.likes.find(x => x.userId.toString() == this.token.get().UserId) == undefined;
			},
			numberOfChildComments(){
				var num = 0;
				// this.posts.comments.forEach(c => num += c.childComments.length);
				for(let c of this.post.comments){
					num += c.childComments.length;
				}
				return num;
			}
		},
		methods :{
			getLikes(){
				const id = this.$route.params.id;
				axios.get(this.baseURL + "likedposts/" + id)
				.then(res => {
					if(res.status == 200 && res.data){
						this.post.likes = res.data;
					}
				})
				.catch((err )=>{
					if(err.response.status != 401){
						this.showModal = true;
					}
				})
			},
			dislikePost(){
				const id = this.$route.params.id;
				axios({
					method: "delete",
					url : this.baseURL + "likedposts",
					data : {
						userId : this.token.get().UserId,
						postId : id
					}
				})
				.then(res => {
					if(res.status == 204){
						this.getLikes();
					}
				})
				.catch((err)=>{
					if(err.response.status != 401){
						this.showModal = true;
					}
				})
			},
			likePost(){
				const id = this.$route.params.id;
				axios.post(this.baseURL + "likedposts", {
					userId : this.token.get().UserId,
					postId : id
				})
				.then(res => {
					if(res.status == 201){
						this.getLikes();
					}
				})
				.catch((err)=>{
					if(err.response.status != 401){
						this.showModal = true;
					}
				})
			},
			showCommentSection(){
				this.showComments = true;
			},
			refreshComments(){
				const id = this.$route.params.id;
				axios({
					method: 'get',
					url: this.baseURL + 'comments/?postId='+ id
				})
				.then( (res)=> {
					if(res.status== 200 && res.data){
						this.post.comments = res.data;
					}
				})
				.catch((err)=>{
					if(err.response.status != 401){
						this.showModal = true;
					}
				})
			},
			loadPostData(){
				const id = this.$route.params.id;
				axios.get(this.baseURL + "posts/" + id)
				.then(res => {
					if(res.status == 200 && res.data){
						this.post = res.data;
						this.$refs.postBody.innerHTML = res.data.content;
						this.refreshComments();
					}
				})
				.catch((err)=>{
					if(err.response.status != 401){
						this.showModal = true;
					}
				})
			}
		},
		created(){
			this.loadPostData();
		}
	}
</script>


<!-- Lorem ipsum dolor sit amet consectetur adipisicing elit. Magni eveniet eaque doloremque quod ipsam esse quos natus possimus odio, unde nesciunt vel deserunt error iusto explicabo nisi. Recusandae illum rerum sint autem dolor nihil vitae! Officiis a omnis dolores dignissimos dolor sapiente vitae debitis nobis doloribus. Assumenda tenetur quae dolorum architecto atque ut non, repudiandae quam sit minus quisquam vero sed blanditiis suscipit ab soluta necessitatibus alias earum dignissimos possimus velit officiis? Veritatis esse, sed sequi excepturi aut debitis numquam soluta tempore quod ipsam consequatur autem, iste modi dignissimos, expedita at facere laudantium nulla voluptatum nobis delectus eligendi hic inventore? Mollitia fuga autem cum magni similique aspernatur, sunt deserunt dolor! Cupiditate, reprehenderit nisi saepe quis molestiae odio est impedit. Expedita repellat veniam totam perferendis est ipsum dolor beatae odit saepe. Dolorum laboriosam quia debitis ducimus incidunt ipsam maiores ullam magni labore asperiores iusto nam autem ab officiis, perspiciatis assumenda ipsum. Cupiditate, doloribus officia? Omnis excepturi possimus repudiandae doloribus ipsa consequuntur illo ad. Sapiente numquam tempora aspernatur dolor, non asperiores qui velit quas, maiores officia eum deserunt nihil eius quo omnis praesentium quod. Voluptates, neque veniam optio recusandae, aliquid ex eaque voluptatem expedita fugiat accusantium ducimus id esse. Velit, voluptates reiciendis nihil a, et quos necessitatibus doloremque eum adipisci eius excepturi maiores temporibus iure odit itaque ullam! Rerum, itaque. Cupiditate corrupti ut a sed optio repellat ducimus tempora necessitatibus veniam aperiam ipsa, quam ea laborum quidem odio debitis dignissimos voluptates accusamus dolore facere. Dolor necessitatibus repellat non voluptas est minus voluptatibus nulla veritatis iste rem maxime ab aliquam id fugiat deserunt optio tempora doloribus obcaecati, debitis aperiam culpa? Possimus praesentium molestias iusto cupiditate et. Nostrum incidunt laboriosam voluptatibus molestiae ut possimus unde. Voluptatibus eaque amet itaque ipsum at qui facilis sed nostrum perferendis nam eum praesentium porro culpa reiciendis iste perspiciatis tempora impedit, assumenda in commodi dicta. Eos nobis aspernatur ipsum. Architecto natus dignissimos deserunt temporibus. Laboriosam dolorum veritatis qui alias ducimus. Eos consequatur in commodi, odit placeat minima quisquam enim est delectus tempora deserunt quaerat id laborum neque corporis esse libero quia aliquam inventore expedita quod necessitatibus. Similique pariatur recusandae, quod labore quos consectetur mollitia deleniti aut accusantium magni voluptatibus ut modi maxime qui eius ipsum corrupti ad aspernatur numquam quia illum dolore, quae ipsam voluptatem! Aspernatur omnis et recusandae veritatis. Dolorem reiciendis optio tempora cum at vero tempore voluptate dolorum. Placeat assumenda explicabo natus, aut ex enim nam fugiat deleniti. Esse consequatur ex, suscipit nam qui excepturi deserunt dolor tempora deleniti repudiandae ratione unde voluptates dolorem voluptas. A libero, accusamus excepturi sunt molestiae perferendis hic saepe voluptatum quis nesciunt voluptatem cumque placeat unde distinctio labore ipsam. Minima ad asperiores assumenda suscipit laborum non nobis vel dolorum voluptatum ipsum corrupti culpa sint magnam quod nesciunt ipsam, quibusdam eum numquam nihil cumque repellendus rerum labore odio! Cum autem, impedit veniam exercitationem ipsa mollitia dignissimos vitae commodi esse quas accusamus nesciunt aspernatur odio deleniti alias doloribus, id perspiciatis officia provident facilis culpa nostrum voluptate! Neque magnam quaerat iste eum error, ad quidem. -->