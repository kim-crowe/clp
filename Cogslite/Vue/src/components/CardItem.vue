<template>
  <div class="card-item rounded overflow-hidden mx-3">
    <img class="rounded shadow-md" :src="getImageUrl(card)" width="250" height="350">
    <div class="px-4 py-2 text-center" v-if="card.name">
      <div class="font-bold text-sm mb-2 mx-auto">{{card.name}}</div>
    </div>
  </div>
</template>

<script>
export default {
  props: ["card"],
  methods: {
    getImageUrl: function(card) {
      return "http://localhost:5000" + card.imageUrl;
    }
  }
};
</script>
