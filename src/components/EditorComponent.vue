<template>
	<div v-if="editor">
		<button @click.prevent="editor.chain().focus().toggleBold().run()" :class="editor.isActive('bold') ? 'btn btn-primary' : 'btn btn-light'" >
			<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-type-bold" viewBox="0 0 16 16">
  <path d="M8.21 13c2.106 0 3.412-1.087 3.412-2.823 0-1.306-.984-2.283-2.324-2.386v-.055a2.176 2.176 0 0 0 1.852-2.14c0-1.51-1.162-2.46-3.014-2.46H3.843V13H8.21zM5.908 4.674h1.696c.963 0 1.517.451 1.517 1.244 0 .834-.629 1.32-1.73 1.32H5.908V4.673zm0 6.788V8.598h1.73c1.217 0 1.88.492 1.88 1.415 0 .943-.643 1.449-1.832 1.449H5.907z"/>
</svg>
    </button>
    <button @click.prevent="editor.chain().focus().toggleItalic().run()" :class="editor.isActive('italic') ? 'btn btn-primary' : 'btn btn-light'" >
		<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-type-italic" viewBox="0 0 16 16">
  <path d="M7.991 11.674 9.53 4.455c.123-.595.246-.71 1.347-.807l.11-.52H7.211l-.11.52c1.06.096 1.128.212 1.005.807L6.57 11.674c-.123.595-.246.71-1.346.806l-.11.52h3.774l.11-.52c-1.06-.095-1.129-.211-1.006-.806z"/>
</svg>
    </button>
    <button @click.prevent="editor.chain().focus().toggleStrike().run()" :class="editor.isActive('strike') ? 'btn btn-primary' : 'btn btn-light'">
		<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-type-strikethrough" viewBox="0 0 16 16">
  <path d="M6.333 5.686c0 .31.083.581.27.814H5.166a2.776 2.776 0 0 1-.099-.76c0-1.627 1.436-2.768 3.48-2.768 1.969 0 3.39 1.175 3.445 2.85h-1.23c-.11-1.08-.964-1.743-2.25-1.743-1.23 0-2.18.602-2.18 1.607zm2.194 7.478c-2.153 0-3.589-1.107-3.705-2.81h1.23c.144 1.06 1.129 1.703 2.544 1.703 1.34 0 2.31-.705 2.31-1.675 0-.827-.547-1.374-1.914-1.675L8.046 8.5H1v-1h14v1h-3.504c.468.437.675.994.675 1.697 0 1.826-1.436 2.967-3.644 2.967z"/>
</svg>
    </button>
	<button @click.prevent="editor.chain().focus().setParagraph().run()" :class="editor.isActive('paragraph') ? 'btn btn-primary' : 'btn btn-light'">
		<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-text-paragraph" viewBox="0 0 16 16">
  <path fill-rule="evenodd" d="M2 12.5a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7a.5.5 0 0 1-.5-.5zm0-3a.5.5 0 0 1 .5-.5h11a.5.5 0 0 1 0 1h-11a.5.5 0 0 1-.5-.5zm0-3a.5.5 0 0 1 .5-.5h11a.5.5 0 0 1 0 1h-11a.5.5 0 0 1-.5-.5zm4-3a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7a.5.5 0 0 1-.5-.5z"/>
</svg>
    </button>
	<button @click.prevent="editor.chain().focus().toggleHeading({ level: 1 }).run()" :class="editor.isActive('heading', {level: 1}) ? 'btn btn-primary' : 'btn btn-light'">
      h1
    </button>
    <button @click.prevent="editor.chain().focus().toggleHeading({ level: 2 }).run()" :class="editor.isActive('heading', {level: 2}) ? 'btn btn-primary' : 'btn btn-light'">
      h2
    </button>
    <button @click.prevent="editor.chain().focus().toggleHeading({ level: 3 }).run()" :class="editor.isActive('heading', {level: 3}) ? 'btn btn-primary' : 'btn btn-light'">
      h3
    </button>
    <button @click.prevent="editor.chain().focus().toggleHeading({ level: 4 }).run()" :class="editor.isActive('heading', {level: 4}) ? 'btn btn-primary' : 'btn btn-light'">
      h4
    </button>
    <button @click.prevent="editor.chain().focus().toggleHeading({ level: 5 }).run()" :class="editor.isActive('heading', {level: 5}) ? 'btn btn-primary' : 'btn btn-light'">
      h5
    </button>
    <button @click.prevent="editor.chain().focus().toggleHeading({ level: 6 }).run()" :class="editor.isActive('heading', {level: 6}) ? 'btn btn-primary' : 'btn btn-light'">
      h6
    </button>
	<button @click.prevent="editor.chain().focus().toggleBulletList().run()" :class="editor.isActive('bulletList') ? 'btn btn-primary' : 'btn btn-light'">
		<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-list-ul" viewBox="0 0 16 16">
  <path fill-rule="evenodd" d="M5 11.5a.5.5 0 0 1 .5-.5h9a.5.5 0 0 1 0 1h-9a.5.5 0 0 1-.5-.5zm0-4a.5.5 0 0 1 .5-.5h9a.5.5 0 0 1 0 1h-9a.5.5 0 0 1-.5-.5zm0-4a.5.5 0 0 1 .5-.5h9a.5.5 0 0 1 0 1h-9a.5.5 0 0 1-.5-.5zm-3 1a1 1 0 1 0 0-2 1 1 0 0 0 0 2zm0 4a1 1 0 1 0 0-2 1 1 0 0 0 0 2zm0 4a1 1 0 1 0 0-2 1 1 0 0 0 0 2z"/>
</svg>
    </button>
    <button @click.prevent="editor.chain().focus().toggleOrderedList().run()" :class="editor.isActive('orderedList') ? 'btn btn-primary' : 'btn btn-light'">
		<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-list-ol" viewBox="0 0 16 16">
  <path fill-rule="evenodd" d="M5 11.5a.5.5 0 0 1 .5-.5h9a.5.5 0 0 1 0 1h-9a.5.5 0 0 1-.5-.5zm0-4a.5.5 0 0 1 .5-.5h9a.5.5 0 0 1 0 1h-9a.5.5 0 0 1-.5-.5zm0-4a.5.5 0 0 1 .5-.5h9a.5.5 0 0 1 0 1h-9a.5.5 0 0 1-.5-.5z"/>
  <path d="M1.713 11.865v-.474H2c.217 0 .363-.137.363-.317 0-.185-.158-.31-.361-.31-.223 0-.367.152-.373.31h-.59c.016-.467.373-.787.986-.787.588-.002.954.291.957.703a.595.595 0 0 1-.492.594v.033a.615.615 0 0 1 .569.631c.003.533-.502.8-1.051.8-.656 0-1-.37-1.008-.794h.582c.008.178.186.306.422.309.254 0 .424-.145.422-.35-.002-.195-.155-.348-.414-.348h-.3zm-.004-4.699h-.604v-.035c0-.408.295-.844.958-.844.583 0 .96.326.96.756 0 .389-.257.617-.476.848l-.537.572v.03h1.054V9H1.143v-.395l.957-.99c.138-.142.293-.304.293-.508 0-.18-.147-.32-.342-.32a.33.33 0 0 0-.342.338v.041zM2.564 5h-.635V2.924h-.031l-.598.42v-.567l.629-.443h.635V5z"/>
</svg>
    </button>
    <button @click.prevent="editor.chain().focus().setTextAlign('left').run()" :class="editor.isActive({textAlign: 'left'}) ? 'btn btn-primary' : 'btn btn-light'">
		<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-text-left" viewBox="0 0 16 16">
  <path fill-rule="evenodd" d="M2 12.5a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7a.5.5 0 0 1-.5-.5zm0-3a.5.5 0 0 1 .5-.5h11a.5.5 0 0 1 0 1h-11a.5.5 0 0 1-.5-.5zm0-3a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7a.5.5 0 0 1-.5-.5zm0-3a.5.5 0 0 1 .5-.5h11a.5.5 0 0 1 0 1h-11a.5.5 0 0 1-.5-.5z"/>
</svg>
    </button>
    <button @click.prevent="editor.chain().focus().setTextAlign('right').run()" :class="editor.isActive({textAlign: 'right'}) ? 'btn btn-primary' : 'btn btn-light'">
		<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-text-right" viewBox="0 0 16 16">
  <path fill-rule="evenodd" d="M6 12.5a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7a.5.5 0 0 1-.5-.5zm-4-3a.5.5 0 0 1 .5-.5h11a.5.5 0 0 1 0 1h-11a.5.5 0 0 1-.5-.5zm4-3a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7a.5.5 0 0 1-.5-.5zm-4-3a.5.5 0 0 1 .5-.5h11a.5.5 0 0 1 0 1h-11a.5.5 0 0 1-.5-.5z"/>
</svg> 
    </button>
    <button @click.prevent="editor.chain().focus().setTextAlign('center').run()" :class="editor.isActive({textAlign: 'center'}) ? 'btn btn-primary' : 'btn btn-light'">
		<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-text-center" viewBox="0 0 16 16">
  <path fill-rule="evenodd" d="M4 12.5a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7a.5.5 0 0 1-.5-.5zm-2-3a.5.5 0 0 1 .5-.5h11a.5.5 0 0 1 0 1h-11a.5.5 0 0 1-.5-.5zm2-3a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7a.5.5 0 0 1-.5-.5zm-2-3a.5.5 0 0 1 .5-.5h11a.5.5 0 0 1 0 1h-11a.5.5 0 0 1-.5-.5z"/>
</svg>
    </button>
    <button @click.prevent="editor.chain().focus().setTextAlign('justify').run()" :class="editor.isActive({textAlign: 'justify'}) ? 'btn btn-primary' : 'btn btn-light'">
		<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-justify" viewBox="0 0 16 16">
  <path fill-rule="evenodd" d="M2 12.5a.5.5 0 0 1 .5-.5h11a.5.5 0 0 1 0 1h-11a.5.5 0 0 1-.5-.5zm0-3a.5.5 0 0 1 .5-.5h11a.5.5 0 0 1 0 1h-11a.5.5 0 0 1-.5-.5zm0-3a.5.5 0 0 1 .5-.5h11a.5.5 0 0 1 0 1h-11a.5.5 0 0 1-.5-.5zm0-3a.5.5 0 0 1 .5-.5h11a.5.5 0 0 1 0 1h-11a.5.5 0 0 1-.5-.5z"/>
</svg> 
	</button>
	<button @click.prevent="addImage" class="btn btn-light">
		<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-card-image" viewBox="0 0 16 16">
  <path d="M6.002 5.5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0z"/>
  <path d="M1.5 2A1.5 1.5 0 0 0 0 3.5v9A1.5 1.5 0 0 0 1.5 14h13a1.5 1.5 0 0 0 1.5-1.5v-9A1.5 1.5 0 0 0 14.5 2h-13zm13 1a.5.5 0 0 1 .5.5v6l-3.775-1.947a.5.5 0 0 0-.577.093l-3.71 3.71-2.66-1.772a.5.5 0 0 0-.63.062L1.002 12v.54A.505.505 0 0 1 1 12.5v-9a.5.5 0 0 1 .5-.5h13z"/>
</svg>
	</button>
	</div>
	<editor-content :editor="editor" />
	
  </template>
  
  <script>
  import { Editor, EditorContent } from '@tiptap/vue-3'
  import StarterKit from '@tiptap/starter-kit'
  import TextAlign from '@tiptap/extension-text-align'
  import Image from '@tiptap/extension-image'
  export default {
	components: {
	  EditorContent,
	},
  
	data() {
	  return {
		editor: null,
	  }
	},
	props: {
		modelValue : {
			type : 	String,
			default : ""
		}		
	},
	methods: {
    addImage() {
      const url = window.prompt('URL')

      if (url) {
        this.editor.chain().focus().setImage({ src: url }).run()
      }
    },
  },

	watch : {
		modelValue(value){
			const isSame = this.editor.getHTML() === value;

			if (isSame) {
				return;
			}

			this.editor.commands.setContent(value, false);
		}
	},
	emits : ["update:modelValue"],
	mounted() {
	  this.editor = new Editor({
		content: this.modelValue,
		onUpdate: () => {
        // HTML
        this.$emit('update:modelValue', this.editor.getHTML())

        // JSON
        // this.$emit('update:modelValue', this.editor.getJSON())
      },
		extensions: [
		  StarterKit,
		  TextAlign.configure({
			types : ["heading", "paragraph"]
		  }),
		  Image
		],
	  })
	},
  
	beforeUnmount() {
	  this.editor.destroy()
	},
  }
  </script>
  <style>
	.ProseMirror{
		border: 1px solid #e2e2e2;
		border-radius: 50px;
		padding: 10px 20px;
		margin-top: 10px;
		min-height: 50vh;
	}
	.button{
		background: #e2e2e2;
		color: #000;
	}
	.is-active{
		background-color: #000;
		color: #fff;

	}
  </style>
		