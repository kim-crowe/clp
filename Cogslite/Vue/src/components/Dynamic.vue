<template>
  <component :is="component" :data="modal.data" v-if="component"/>
</template>
<script>
export default {
  name: "dynamic",
  props: ["modal"],
  computed: {
    component() {
      if (!this.modal || !this.modal.component) {
        return null;
      }
      return () => import(`@/${this.modal.component}`);
    }
  }
};
</script>
