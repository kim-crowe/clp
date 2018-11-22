<template>
  <on-click-outside :do="handleClickOutside">
    <div class="relative inline-block">
      <div role="button" class="inline-block select-none" @click="open = !open">
        <span class="appearance-none flex items-center inline-block text-cogs-secondary font-semibold bg-cogs-secondary px-4 py-2 rounded">
        <span class="mr-1">{{text}}</span>
        <svg class="h-4 w-4 fill-current" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20">
          <path d="M9.293 12.95l.707.707L15.657 8l-1.414-1.414L10 10.828 5.757 6.586 4.343 8z" />
        </svg>
      </span>
      </div>
      <div class="absolute pin-r mt-px z-50" v-show="open">
        <slot></slot>
      </div>
    </div>
  </on-click-outside>
</template>

<script>
import OnClickOutside from "./OnClickOutside";

export default {
  props: ["text"],
  components: { OnClickOutside },
  methods: {
    handleClickOutside() {
      if (this.open) {
        this.open = false;
      }
    }
  },
  data: function() {
    return {
      open: false
    };
  }
};
</script>
