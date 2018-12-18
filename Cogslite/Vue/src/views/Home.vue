<template>
  <div>
    <div class="text-left text-3xl p-6 text-cogs-alt w-screen bg-grey-light">
      <div class="container mx-auto">
        <div class="mx-6 px-6">
          <div>Welcome to CCG Works.</div>
          <div>An awesome tool for collectible card game players.</div>
          <div>Build decks for games you love and import them in to Tabletop Simulator.</div>
        </div>
      </div>
    </div>
    <div class="container mx-auto">
      <div class="my-6">
        <div class="w-3/4 mx-auto">
          <SearchBox placeholder="Search for a game..." :onSearch="onSearch"/>
        </div>
      </div>
      <ul class="flex flex-wrap w-3/4 mx-auto list-reset">
        <li v-for="game in filteredGames" v-bind:key="game.id">
          <GameTile :game="game" @select="selectGame"/>
        </li>
      </ul>
    </div>
  </div>
</template>

<script>
import GameTile from "../components/GameTile";
import PageButtons from "../components/PageButtons";
import gamesService from "../services/gamesService";
import SearchBox from "../components/SearchBox";

export default {
  name: "home",
  components: {
    GameTile,
    PageButtons,
    SearchBox
  },
  created: function() {
    this.refreshGames();
  },
  methods: {
    onSearch: function(e) {
      this.filter = e.target.value;
    },
    selectGame: function(game) {
      this.$router.push({ name: "cards", params: { gameId: game.id } });
    },
    refreshGames: function() {
      gamesService.getGames().then(d => {
        this.games = d.games;
      });
    }
  },
  computed: {
    filteredGames: function() {
      if (this.games) {
        return this.games.filter(g => g.name.startsWith(this.filter));
      }
      return [];
    }
  },
  data: function() {
    return {
      filter: "",
      games: []
    };
  }
};
</script>
