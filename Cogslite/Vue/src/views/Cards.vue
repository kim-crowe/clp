<template>
  <div class="container mx-auto mt-2">
    <div class="float-left mr-2">
      <img class="rounded block" :src="gameImageUrl(game)" width="90" height="180">
      <router-link
        class="block rounded-full bg-cogs-secondary text-cogs-secondary px-2 py-1 my-2 text-center text-xs no-underline"
        :to="{name: 'add-cards', params: {gameId: game.id}}"
      >Add cards</router-link>
      <a
        class="block rounded-full bg-cogs-secondary text-cogs-secondary px-2 py-1 my-2 text-center text-xs no-underline"
        href="#"
      >Edit details</a>
    </div>
    <div class="p-2 mx-3 text-3xl font-semibold">{{game.name}}</div>
    <div>
      <page-buttons :currentPage="search.page" :totalPages="numberOfPages" @first="gotoFirst" @last="gotoLast" @previous="gotoPrevious" @next="gotoNext"/>
      <input type="text">
    </div>
    <ul class="list-reset flex flex-wrap px-2 py-2">
      <li v-for="card in cards" v-bind:key="card.id">
        <card-item :card="card"/>
      </li>
    </ul>
  </div>
</template>

<script>
import CardItem from "../components/CardItem";
import PageButtons from "../components/PageButtons";
import gamesService from "../services/gamesService";
import cardsService from "../services/cardsService";

export default {
  name: "cards",
  components: { CardItem, PageButtons },
  mounted: function() {
    var gameId = this.$route.params.gameId;
    gamesService.getGame(gameId).then(game => {
      this.game = game;
      this.loadCards();
    });
  },
  methods: {
    gameImageUrl: function(game) {
      return "http://localhost:5000/" + game.imageUrl;
    },
    loadCards: function() {
      cardsService.search(this.game.id, this.search).then(data => {
        this.cards = data.cards;
        this.numberOfPages = data.numberOfPages;
      });
    },
    gotoPage: function(page) {
      this.search.page = page;
      this.loadCards();
    },
    gotoFirst: function() {
      if(this.search.page > 1) {
        this.gotoPage(1);        
      }      
    },
    gotoPrevious: function() {
      if(this.search.page > 1) {
        this.gotoPage(this.search.page - 1);
      }
    },
    gotoLast: function() {
      if(this.search.page < this.numberOfPages) {
        this.gotoPage(this.numberOfPages);
      }
    },
    gotoNext : function() {
      if(this.search.page < this.numberOfPages) {
        this.gotoPage(this.search.page + 1);
      }
    }
  },
  data: function() {
    return {
      game: {},
      search: {
        page: 1,
        itemsPerPage: 20,
        cardName: "",
        cardType: "",
        tags: [],
        cardIds: []
      },
      cards: [],
      numberOfPages: 1
    };
  }
};
</script>
