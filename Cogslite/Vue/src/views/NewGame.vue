<template>
  <div class="container mx-auto my-6">
    <div class="w-full max-w-md mx-auto" v-show="!success">
      <div class="font-bold text-2xl p-1">Create a new game</div>
      <div class="p-1 pb-4 border-b">You can create and manage cards for your game once it is created</div>
      <label class="block my-2" for="name">Name</label>
      <input v-model="name"
        class="bg-grey-lighter appearance-none
        border-2 border-grey-lighter rounded w-full
        py-2 px-4 text-grey-darker leading-tight
        focus:outline-none focus:bg-white focus:border-purple" id="name" type="text">
      <label class="block my-2" for="description (optional)">Description</label>
      <input v-model="description"
        class="bg-grey-lighter appearance-none
        border-2 border-grey-lighter rounded
        w-full py-2 px-4 text-grey-darker leading-tight
        focus:outline-none focus:bg-white focus:border-purple" id="description" type="text">
      <label class="block my-2" for="Image">Image</label>
      <div class="rounded bg-cogs-secondary my-2 flex items-center text-white" style="width: 90px; height: 130px;">
        <img v-bind:src="preview" v-show="showPreview"/>
      </div>
      <file-select @fileSelected="fileSelected" @previewReady="previewReady" text="Choose an image"/>
      <div class="border-t pt-4">
        <button class="bg-green-dark px-4 py-2 rounded text-white" type="button" @click="saveGame">Create</button>
        <button class="bg-red focus:outline-none text-white py-2 px-4 ml-2 rounded" type="button">Cancel</button>
      </div>
    </div>
    <div class="w-full max-w-md mx-auto" v-show="success">
      <div class="bg-green-lightest border-l-4 border-green text-green-dark p-4 my-2" role="alert">
        <p class="font-bold">Success</p>
        <p>Your game has been created, go forth and make awesome.</p>
      </div>
      <div class="my-2">
        <img v-bind:src="preview" class="rounded float-left" width="90" height="130" />
        <span class="text-3xl text-bold mx-3">{{name}}</span>
      </div>
    </div>
  </div>
</template>

<script>
import gameService from "../services/gamesService";
import FileSelect from "../components/FileSelect";

export default {
  components: { FileSelect },
  methods: {
    saveGame() {
      gameService
        .saveGame(this.name, this.description, this.file)
        .then(r => { this.success = true; this.gameId = r.data; });
    },
    fileSelected(file) {
      this.file = file;
    },
    previewReady(preview) {      
      this.preview = preview;
    }
  },
  computed: {
    showPreview: function() {
      return this.preview != null;
    }
  },
  data: function() {
    return {
      name: "",
      description: "",
      success: false,
      file: null,
      preview: null,
      gameId: '',
    };
  }
};
</script>
