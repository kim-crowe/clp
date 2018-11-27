<template>
  <div class="rounded overflow-hidden mx-3 flex flex-col">
    <img
      class="rounded shadow-md cursor-pointer"
      @click="$emit('select', game)"
      :src="getImageUrl(game)"
      width="80"
      height="130"
    >
    <a
      href="#"
      @click="$emit('select', game)"
      :to="{name: 'cards', params: {gameId: game.id}}"
      class="font-bold text-sm no-underline text-black py-2"
    >{{game.name}}</a>
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
</style>
