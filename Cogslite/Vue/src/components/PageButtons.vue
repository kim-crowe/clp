<template>
  <ul class="pagination">
      <li class="page-item"><a class="page-link" href="#" v-on:click="gotoFirst"><i class="fa fa-angle-double-left"></i></a></li>
      <li class="page-item"><a class="page-link" href="#" v-on:click="gotoPrevious"><i class="fa fa-angle-left"></i></a></li>
      <li class="page-item active"><a class="page-link" href="#">Page {{currentPage}} of {{totalPages}}</a></li>
      <li class="page-item"><a class="page-link" href="#" v-on:click="gotoNext"><i class="fa fa-angle-right"></i></a></li>
      <li class="page-item"><a class="page-link" href="#" v-on:click="gotoLast"><i class="fa fa-angle-double-right"></i></a></li>
  </ul>
</template>

<script>
export default {
  props: [
    "gotoFirst",
    "gotoPrevious",
    "gotoNext",
    "gotoLast",
    "currentPage",
    "totalPages"
  ]
};
</script>
