<template>
  <div class="tile rounded overflow-hidden mx-3">
    <img class="rounded shadow-md" :src="getImageUrl(game)" width="80" height="130" />
    <div class="px-4 py-2">
      <div class="font-bold text-sm mb-2 mx-auto">{{game.name}}</div>
    </div>
  </div>
</template>

<script>
export default {
  props: ["game"],
  methods: {
    getImageUrl: function(game) {
      return "http://localhost:5000" + game.imageUrl;
    }
  }
};
</script>

<style scoped>
.tile {
  width: 200px;
}
</style>
