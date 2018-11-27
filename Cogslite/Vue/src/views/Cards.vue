<template>
  <div>
    <div class="p-2 mx-6">Cards for {{game.name}}</div>
    <ul class="list-reset">
      <li v-for="card in cards" v-bind:key="card.id">
        <div>{{card.id}}</div>
      </li>
    </ul>
  </div>
</template>

<script>
import GameTile from "../components/GameTile";
import PageButtons from "../components/PageButtons";
import gamesService from "../services/gamesService";
import cardsService from "../services/cardsService";

export default {
  name: "cards",
  mounted: function() {
    var gameId = this.$route.params.gameId;
    gamesService.getGame(gameId).then(game => {
      this.game = game;
      this.loadCards();
    });
  },
  methods: {
    loadCards: function() {
      cardsService
        .search(this.game.id, this.search)
        .then(data => (this.cards = data.cards));
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
