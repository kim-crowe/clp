<template>
  <div>
      <div class="text-left text-3xl p-6 text-cogs-alt w-screen bg-grey-light">
        <div class="container mx-auto">
          <div class="mx-6 px-6">
            <div>Welcome to Cogs.</div>
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
        <div class="flex flex-wrap w-3/4 mx-auto">
          <GameTile v-for="game in filteredGames" v-bind:key="game.id" :game=game />
        </div>
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
    refreshGames: function() {
      this.games = gamesService.getGames();
    }
  },
  computed: {
    filteredGames: function() {
      return this.games.filter(g => g.name.startsWith(this.filter));
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
