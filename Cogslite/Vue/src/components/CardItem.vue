<template>
  <div class="card-item rounded mx-3 my-3 relative">
    <div
      class="flex items-end justify-between absolute pin-t pin-l w-full p-2 show-on-hover"
      style="height: 175px; background-color:rgba(0, 0, 0, 0.25);"
    >
      <div
        @click="$emit('remove')"
        class="rounded-full h-8 w-8 flex items-center justify-center bg-red text-white"
      >
        <i class="fa fa-minus"></i>
      </div>
      <div
        @click="$emit('add')"
        class="rounded-full h-8 w-8 flex items-center justify-center bg-green text-white"
      >
        <i class="fa fa-plus"></i>
      </div>
    </div>
    <div
      style="top: -.625rem; right: -.625rem;"
      class="rounded-full h-6 w-6 flex items-center justify-center bg-yellow-dark text-orange-dark absolute border border-orange"
    >{{cardCount}}</div>
    <img
      class="rounded"
      :src="getImageUrl(card)"
      style="width: 125px; height: 175px; max-width: 125px;"
    >
    <div class="px-4 py-2 text-center" v-if="card.name">
      <div class="font-bold text-sm mb-2 mx-auto">{{card.name}}</div>
    </div>
  </div>
</template>

<script>
export default {
  props: ["card", "cardCount"],
  methods: {
    getImageUrl: function(card) {
      return "http://localhost:5000" + card.imageUrl;
    }
  }
};
</script>

<style>
.show-on-hover {
  transition: 0.5s ease;
  opacity: 0;
}

.show-on-hover:hover {
  opacity: 1;
}
</style>

