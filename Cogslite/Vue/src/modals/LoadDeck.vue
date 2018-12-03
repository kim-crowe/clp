<template>
  <div
    class="rounded border border-black bg-white block"
    style="width: 450px; height: 250px; margin-top: 300px;"
  >
    <div class="px-2 py-2 bg-cogs-primary text-cogs-secondary block">Load a deck</div>
    <div class="p-6">
      <ul class="list-reset mx-auto border border-grey h-32 overflow-auto">
        <li
          @click="selectedDeck=deck"
          v-for="deck in data.decks"
          v-bind:key="deck.id"
          :class="['p-2', {'hover:bg-blue-lighter hover:text-white': selectedDeck !== deck}, {'bg-blue text-white': selectedDeck === deck}]"
        >{{deck.name}}</li>
      </ul>
      <button class="bg-green-dark px-4 py-2 rounded text-white" @click="$emit('accept', {selectedDeck})">Load</button>
      <button class="bg-red px-4 py-2 rounded text-white" @click="$emit('reject')">Cancel</button>
    </div>
  </div>
</template>

<script>
export default {
  props: ["data"],
  mounted: function() {},
  data: function() {
    return {
      selectedDeck: null
    }
  }
};
</script>

