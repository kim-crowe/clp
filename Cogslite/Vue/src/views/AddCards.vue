<template>
  <div class="mx-auto my-6">
    <div class="w-full max-w-md mx-auto">
      <div class="font-bold text-2xl p-1">Add cards from a card sheet</div>
      <div class="p-1 pb-4 border-b">
        Choose a ready made card sheet from your local file system. Specify the number of cards per row and the total number of cards and cogs will take care of the rest.
      </div>
      <div class="flex">
        <div class="w-1/2">
          <label class="block my-2" for="name">Cards per row</label>
          <input v-model="cardsPerRow"
            class="bg-grey-lighter appearance-none
            border-2 border-grey-lighter rounded w-24
            py-2 px-4 text-grey-darker leading-tight
            focus:outline-none focus:bg-white focus:border-purple" id="name" type="number">
          <label class="block my-2" for="description (optional)">Total number of cards</label>
          <input v-model="cardCount"
            class="bg-grey-lighter appearance-none
            border-2 border-grey-lighter rounded
            w-24 py-2 px-4 text-grey-darker leading-tight
            focus:outline-none focus:bg-white focus:border-purple" id="description" type="number">
            <file-select @fileSelected="fileSelected" @previewReady="previewLoaded" text="Choose a card sheet"/>
        </div>
        <div class="w-1/2 my-4">
          <div class="rounded bg-grey-dark relative overflow-hidden" style="width: 250px; height: 350px;">
            <canvas ref="canvas" width="250" height="350" v-show="showPreview"></canvas>
          </div>
          <div class="flex items-center justify-between my-2" style="width: 250px;">
            <button class="rounded px-4 py-2 bg-indigo text-white" @click="previousCard">&lt;</button>
            <button class="rounded px-4 py-2 bg-indigo text-white" @click="nextCard">&gt;</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import FileSelect from "../components/FileSelect";

export default {
  components: { FileSelect },
  methods: {
    fileSelected(file) {
      this.file = file;
    },
    previewLoaded(preview) {
      this.showPreview = true;
      this.preview = new Image();
      this.preview.src = preview;
      this.updatePreview();
    },
    updatePreview() {

      if(this.cardsPerRow < 1 | this.cardCount < 1)
        return;

      var canvas = this.$refs.canvas.getContext("2d");
      var imageWidth = Math.floor(this.preview.width / this.cardsPerRow);
      var rows = Math.floor(this.cardCount / this.cardsPerRow);
      if( (this.cardCount % this.cardsPerRow) > 0 )
      {
        rows++;
      }

      var imageHeight = Math.floor(this.preview.height / rows);
      var rowIndex = Math.floor(this.cardIndex / this.cardsPerRow);
      var columnIndex = this.cardIndex % this.cardsPerRow;

      //canvas.drawImage(this.preview, 0, 0, imageWidth, imageHeight, 0, 0, 250, 350);
      canvas.drawImage(this.preview, columnIndex * imageWidth, rowIndex * imageHeight, imageWidth, imageHeight, 0, 0, 250, 350);
    },
    nextCard() {
      if (this.cardIndex < this.cardCount)
        this.cardIndex++;
      else
        this.cardIndex = 0;
      this.updatePreview();
    },
    previousCard() {
      if (this.cardIndex > 0)
        this.cardIndex--;
      else
        this.cardIndex = this.cardCount -1;
      this.updatePreview();
    }
  },
  data: function() {
    return {
      cardsPerRow: 0,
      cardCount: 0, 
      preview: null,
      showPreview: false,
      file: null,
      cardIndex: 0
    }
  }
}
</script>
