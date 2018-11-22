<template>
  <a :href="link" class="inline-block rounded py-2 px-4 bg-cogs-secondary text-cogs-secondary hover:text-red-lightest no-underline">
    <slot></slot>
  </a>
</template>

<script>
export default {
  props: ["link"]
};
</script>

