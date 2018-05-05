Vue.http.headers.common['XSRF-TOKEN'] = $("#af-token").val();

var cardsVue = new Vue({
    el: '#cardApp',
    mixins: [cardServiceFactory, deckServiceFactory, debouncer],
    data: {
        gameId: '',
        numberOfPages: 1,
        search: { page: 1, itemsPerPage: 20, cardName: '', cardType: "-1", tags: [] },
        cards: [],
        tags: [],
        decks: [],
        deck: null,
        dialog: null
    },
    created: function () {
        this.gameId = $('#game-id').val();
        this.refreshCards();
        this.refreshDeckList();
    },
    computed: {
        hasDeck: function () {
            return this.deck != null;
        }
    },
    methods: {
        onSearchChange: function (e) {
            this.search.cardName = e.target.value;
            this.debounce(this.refreshCards, 300);
        },
        refreshCards: function () {
            this.cardService(this.$http).getCards(this.gameId, this.search).then(data => {                
                this.cards = data.body.cards;
                this.numberOfPages = data.body.numberOfPages;
            });
        },
        refreshDeckList: function () {
            this.deckService(this.$http).deckList(this.gameId).then(function (response) {
                this.decks = response.body;
            });
        },
        imageUrl: function (card) {
            return "/Image?imageId=" + card.id;
        },
        firstPage: function () {
            this.search.page = 1;
            this.refreshCards();
        },
        previousPage: function () {
            if (this.search.page > 1) {
                this.search.page--;
                this.refreshCards();
            }
        },
        nextPage: function () {
            if (this.search.page < this.numberOfPages) {
                this.search.page++;
                this.refreshCards();
            }
        },
        lastPage: function () {
            this.search.page = this.numberOfPages;
            this.refreshCards();
        },
        newDeck: function () {            

            this.dialog = {                
                title: "Create a new deck",
                fields: [
                    { name: "deckName", type: "text", placeholder: "Deck name..." }                    
                ],
                confirmText: 'Save deck',
                onConfirm: function (model, parent) {
                    var deck = { id: '', gameId: parent.gameId, name: model.deckName, items: [] };
                    parent.saveDeck(deck)
                }
            }
            
            $('#modal1').modal();
        },
        loadDeck: function () {

           var decksAsOptions = this.decks.map(function (deck) {
                return {
                    value: deck.id,
                    text: deck.name
                };
            });

            this.dialog = {
                title: "Load a deck",
                fields: [
                    { name: "deck", type: "options", options: decksAsOptions }
                ],
                confirmText: 'Load deck',
                onConfirm: function (model, parent) {
                    parent.deck = parent.decks.find(function (d) {
                        return d.id == model.deck;
                    });
                }
            }

            $('#modal1').modal();
        },
        saveDeck: function (deck) {
            this.deckService(this.$http).save(deck).then(function (response) {
                this.deck = response.body;
            })
        },
        confirmDialog: function (model) {
            this.dialog.onConfirm(model, this);
            $('#modal1').modal('hide');
        }        
    }
});