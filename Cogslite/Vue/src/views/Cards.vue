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
      <a
        class="block rounded-full bg-cogs-secondary text-cogs-secondary px-2 py-1 my-2 text-center text-xs no-underline"
        href="#"
        @click="createDeck"
      >Create deck</a>
      <a
        class="block rounded-full bg-cogs-secondary text-cogs-secondary px-2 py-1 my-2 text-center text-xs no-underline"
        href="#"
        @click="loadDeck"
      >Load deck</a>
    </div>
    <div class="p-2 mx-3 text-3xl font-semibold">{{game.name}}</div>
    <div>
      <page-buttons
        :currentPage="search.page"
        :totalPages="numberOfPages"
        @first="gotoFirst"
        @last="gotoLast"
        @previous="gotoPrevious"
        @next="gotoNext"
      />
      <span v-if="deckIsLoaded">
        <ul class="inline-flex list-reset border border-grey rounded w-auto ml-2 text-grey-dark">
          <li
            :class="[ {'opt-active': filterByDeck}, 'py-2', 'w-16', 'block', 'text-center', 'no-underline', 'cursor-pointer' ]"
            @click="toggleDeckFilter(true)"
          >Deck</li>
          <li
            :class="[ {'opt-active': !filterByDeck}, 'py-2', 'w-16', 'block', 'text-center', 'no-underline', 'cursor-pointer' ]"
            @click="toggleDeckFilter(false)"
            href="#"
          >All</li>
        </ul>
        <span>{{this.deck.name}}</span>
        <button v-show="this.deck.hasChanges" class="px-4 py-2 rounded bg-cogs-secondary text-cogs-secondary" @click="saveDeck">Save changes</button>
        <a :href="`http://localhost:5000/api/deck/${this.deck.id}/sheet`">deck sheet</a>
      </span>
    </div>
    <ul class="list-reset flex flex-wrap px-2 py-2">
      <li v-for="card in cards" v-bind:key="card.id">
        <card-item
          :card="card"
          :cardCount="cardCount(card)"
          @add="addCard(card)"
          @remove="removeCard(card)"
        />
      </li>
    </ul>
  </div>
</template>

<script>
import CardItem from "../components/CardItem";
import PageButtons from "../components/PageButtons";
import gamesService from "../services/gamesService";
import cardsService from "../services/cardsService";
import deckService from "../services/deckService";
import * as utils from "../utils/deck";

export default {
  name: "cards",
  components: { CardItem, PageButtons },
  computed: {
    deckIsLoaded: function() {
      return this.deck !== null;
    }    
  },
  mounted: function() {
    var gameId = this.$route.params.gameId;
    gamesService.getGame(gameId).then(game => {
      this.game = game;
      this.loadCards();
      this.loadDecks();
    });
  },
  methods: {
    gameImageUrl: function(game) {
      return game.imageUrl;
    },
    loadCards: function() {
      cardsService.search(this.game.id, this.search).then(data => {
        this.cards = data.cards;
        this.numberOfPages = data.numberOfPages;
      });
    },
    loadDecks: function() {
      deckService
        .loadDecks(this.game.id)
        .then(data => 
          this.decks = data.map(d => new utils.Deck(d.id, d.name, d.gameId, d.items, d.version)));
    },
    toggleDeckFilter: function(opt) {
      if (opt && this.deck && this.deck.items && this.deck.items.length > 0) {
        this.search.page = 1;
        this.search.cardIds = this.deck.items.map(function(item) {
          return item.id;
        });
      } else this.search.cardIds = [];
      this.loadCards();
      this.filterByDeck = opt;
    },
    gotoPage: function(page) {
      this.search.page = page;
      this.loadCards();
    },
    gotoFirst: function() {
      if (this.search.page > 1) {
        this.gotoPage(1);
      }
    },
    gotoPrevious: function() {
      if (this.search.page > 1) {
        this.gotoPage(this.search.page - 1);
      }
    },
    gotoLast: function() {
      if (this.search.page < this.numberOfPages) {
        this.gotoPage(this.numberOfPages);
      }
    },
    gotoNext: function() {
      if (this.search.page < this.numberOfPages) {
        this.gotoPage(this.search.page + 1);
      }
    },
    addCard: function(card) {
      if(this.deck)
        this.deck.addCard(card);
    },
    removeCard: function(card) {
      if(this.deck)
        this.deck.removeCard(card);
    },
    cardCount: function(card) {
      if(this.deck)
        return this.deck.countOf(card);
      else
        return 0;
    },
    createDeck: function() {
      this.$showModal({
        component: "modals/UserPrompt",
        data: {
          prompt: 'Enter a name for your deck'
        }
      }).then(r => this.deck = new utils.Deck('', r.value, this.game.id, [], 0));
    },
    saveDeck: function() {
      deckService.saveDeck(this.deck).then(r => {
        this.deck.version = r.version;
        this.deck.hasChanges = false;
        if(r.version === 1) {
          this.decks.push(this.deck);
        }
      })
    },
    loadDeck: function() {
      this.$showModal({
        component: "modals/LoadDeck",
        data: {
          decks: this.decks
        }
      }).then(r => this.deck = r.selectedDeck);
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
      numberOfPages: 1,
      deck: null,
      decks: [],
      filterByDeck: false
    };
  }
};
</script>

<style>
.opt-active {
  background-color: #535353;
  color: #dcae1d;
}
.opt {
  padding-top: 1rem;
  padding-bottom: 1rem;
  width: 50px;
  text-decoration: none;
}
.bg-dialog {
  background-color: rgb(0, 0, 0, 0, 0.25);
}
</style>

