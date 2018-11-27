<template>
  <ul class="inline-flex list-reset border border-grey rounded w-auto">
    <li>
      <a
        class="block hover:text-white hover:bg-blue text-grey border-r border-grey px-3 py-2"
        @click="$emit('first')"
        href="#"
      ><i class="fa fa-angle-double-left"></i></a>
    </li>
    <li>
      <a
        class="block hover:text-white hover:bg-blue text-grey border-r border-grey px-3 py-2"
        @click="$emit('previous')"
        href="#"
      ><i class="fa fa-angle-left"></i></a>
    </li>
    <li class="block text-grey-darker border-r border-grey px-3 py-2">
      {{currentPage}} of {{totalPages}}
    </li>
    <li>
      <a class="block hover:text-white hover:bg-blue text-grey border-r border-grey px-3 py-2" 
        @click="$emit('next')"
        href="#">
        <i class="fa fa-angle-right"></i>
      </a>
    </li>
    <li>
      <a class="block hover:text-white hover:bg-blue text-grey px-3 py-2" 
      @click="$emit('last')"
      href="#">
        <i class="fa fa-angle-double-right"></i>
      </a>
    </li>
  </ul>
</template>

<script>
export default {
  props: [    
    "currentPage",
    "totalPages"
  ]
};
</script>
