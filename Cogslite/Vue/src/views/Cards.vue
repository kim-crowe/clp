<template>
  <div class="about">
    {{games}}
  </div>
</template>

<script>
import GameTile from "../components/GameTile";
import PageButtons from "../components/PageButtons";
import gamesService from "../services/gamesService";

export default {
  name: "cards",
  created: function() {
    this.loadData();
  },
  methods: {
    loadData: function() {
      gamesService.getGamesReal().then(d => (this.games = d));
    }
  },
  data: function() {
    return {
      games: {}
    };
  }
};
</script>
