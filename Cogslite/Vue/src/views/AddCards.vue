<template>
  <div class="container mx-auto my-6">
    <div class="w-full max-w-md mx-auto">
      <div class="font-bold text-2xl p-1">Add cards from a card sheet</div>
      <div class="p-1 pb-4 border-b">
        Choose a ready made card sheet from your local file system. Specify the number of cards per row and the total number of cards and cogs will take care of the rest.
      </div>
      <div class="flex">
        <div class="w-1/2">
          <label class="block my-2" for="name">Cards per row</label>
          <input v-model="name"
            class="bg-grey-lighter appearance-none
            border-2 border-grey-lighter rounded w-24
            py-2 px-4 text-grey-darker leading-tight
            focus:outline-none focus:bg-white focus:border-purple" id="name" type="number">
          <label class="block my-2" for="description (optional)">Total number of cards</label>
          <input v-model="description"
            class="bg-grey-lighter appearance-none
            border-2 border-grey-lighter rounded
            w-24 py-2 px-4 text-grey-darker leading-tight
            focus:outline-none focus:bg-white focus:border-purple" id="description" type="number">
        </div>
        <div class="w-1/2 my-4">
          <div class="rounded bg-grey-dark" style="width: 250px; height: 350px;">
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
