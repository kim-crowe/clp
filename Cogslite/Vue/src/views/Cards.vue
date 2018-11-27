<template>
  <div class="container mx-auto mt-2">
    <div class="float-left mr-2">
      <img class="rounded block" :src="gameImageUrl(game)" width="90" height="180">
      <a
        class="block rounded-full bg-cogs-secondary text-cogs-secondary px-2 py-1 my-2 text-center text-xs no-underline"
        href="#"
      >Add cards</a>
      <a
        class="block rounded-full bg-cogs-secondary text-cogs-secondary px-2 py-1 my-2 text-center text-xs no-underline"
        href="#"
      >Edit details</a>
    </div>
    <div class="p-2 mx-3 text-3xl font-semibold">{{game.name}}</div>
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
  components: { CardItem },
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
      });
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
      cards: []
    };
  }
};
</script>
