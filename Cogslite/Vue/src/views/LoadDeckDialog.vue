<template>
  <div
    class="rounded border border-black bg-white block"
    style="width: 450px; height: 250px; margin-top: 300px;"
  >
    <div class="px-2 py-2 bg-cogs-primary text-cogs-secondary block">Load a deck</div>
    <div class="p-6">
      <ul class="list-reset mx-auto border border-grey h-32 overflow-auto">
        <li
          v-for="opt in data.options"
          v-bind:key="opt"
          class="p-2 hover:bg-blue hover:text-white"
        >{{opt}}</li>
      </ul>
    </div>
  </div>
</template>

<script>
export default {
  props: ["data"],
  mounted: function() {}
};
</script>

