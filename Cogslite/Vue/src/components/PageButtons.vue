<template>
  <ul class="flex list-reset border border-grey-light rounded w-auto font-sans">
    <li>
      <a
        class="block hover:text-white hover:bg-blue text-blue border-r border-grey-light px-3 py-2"
        href="#"
      >Previous</a>
    </li>
    <li>
      <a
        class="block hover:text-white hover:bg-blue text-blue border-r border-grey-light px-3 py-2"
        href="#"
      >1</a>
    </li>
    <li>
      <a
        class="block hover:text-white hover:bg-blue text-blue border-r border-grey-light px-3 py-2"
        href="#"
      >2</a>
    </li>
    <li>
      <a class="block text-white bg-blue border-r border-blue px-3 py-2" href="#">3</a>
    </li>
    <li>
      <a class="block hover:text-white hover:bg-blue text-blue px-3 py-2" href="#">Next</a>
    </li>
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
