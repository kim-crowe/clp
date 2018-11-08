Vue.http.headers.common['XSRF-TOKEN'] = $("#af-token").val();

var cardsVue = new Vue({
    el: '#cardApp',
    mixins: [cardServiceFactory, deckServiceFactory, debouncer],
    data: {
        gameId: '',
        ownerId: '',
        isSignedIn: false,
        isGameOwner: false,
        numberOfPages: 1,
        search: { page: 1, itemsPerPage: 20, cardName: '', cardType: "", tags: [], cardIds: [] },
        cards: [],
        tags: [],
        decks: [],
        deckFilterActive: false,
        deck: null,
        dialog: null
    },
    created: function () {
        this.gameId = $('#game-id').val();
        this.ownerId = $('#owner-id').val();
        this.isSignedIn = $('#is-signed-in').val();
        this.isGameOwner = $('#is-game-owner').val();
        this.refreshCards();
        this.refreshDeckList();
    },
    computed: {
        hasDeck: function () {
            return this.deck != null;
        },
        cardsInDeck: function () {
            if (this.deck != null && this.deck.items && this.deck.items.length > 0)
                return this.deck.items.map(function (item) {
                    return item.amount;
                }).reduce(function (total, next) {
                    return total + next;
                });
            else
                return 0;
        }
    },
    methods: {
        onSearchChange: function (e) {
            this.search.cardName = e.target.value;
            this.refreshCards();
        },
        onCardTypeChanged: function (cardType) {
            this.search.cardType = cardType;
            this.refreshCards();
        },
        filterByDeck: function (opt) {
            if (opt && this.deck && this.deck.items && this.deck.items.length > 0)
                this.search.cardIds = this.deck.items.map(function (item) { return item.id });
            else
                this.search.cardIds = [];
            this.refreshCards();
            this.deckFilterActive = opt;
        },
        refreshCards: function () {
            this.cardService(this.$http).getCards(this.ownerId, this.gameId, this.search).then(data => {
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
            return card.imageUrl;
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
        showDetails: function (card) {
            this.dialog = {
                title: card.name,
                fields: [
                    { type: "image", value: "http://" + window.location.host + "/Image?imageId=" + card.id },
                    { name: "name", label: "Name", type: "text", value: card.name, readonly: !this.isGameOwner },
                    { name: "type", label: "Type", type: "text", value: card.type, readonly: !this.isGameOwner }
                ],
                cancelText: "Close",
                confirmText: "Save Changes",
                hideConfirmButton: !this.isGameOwner,
                onConfirm: function (model, vue) {
                    card.name = model.name;
                    card.type = model.type;
                    vue.cardService(vue.$http).updateCard({ GameId: vue.gameId, CardId: card.id, Name: card.name, Type: card.type });
                }
            }

            $('#modal1').modal();
        },
        showDeckUrls: function () {
            this.dialog = {
                title: "Deck urls",
                fields: [
                    { name: "backUrl", type: "copytext", value: "http://" + window.location.host + "/Image?imageId=" + this.gameId, label: "Card back" },
                    { name: "deckUrl", type: "copytext", value: "http://" + window.location.host + "/Deck?handler=Sheet&deckId=" + this.deck.id, label: "Cards" }
                ],
                hideConfirmButton: true,
                cancelText: "Close"
            }

            $('#modal1').modal();
        },
        newDeck: function () {

            this.dialog = {
                title: "Create a new deck",
                fields: [
                    { name: "deckName", type: "text", placeholder: "Deck name..." }
                ],
                confirmText: 'Save deck',
                onConfirm: function (model, parent) {
                    parent.deck = { id: '', gameId: parent.gameId, name: model.deckName, items: [], version: 0, hasChanges: true };
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
        saveDeck: function () {
            if (this.deck.hasChanges) {
                this.deckService(this.$http).save(this.deck).then(function (response) {
                    this.deck = response.body;
                })
            }
        },
        confirmDialog: function (model) {
            this.dialog.onConfirm(model, this);
            $('#modal1').modal('hide');
        },
        removeCard: function (card) {
            var deckEntry = this.getDeckEntry(card);
            deckEntry.amount--;

            if (deckEntry.amount < 1) {
                var index = this.deck.items.indexOf(deckEntry);
                this.deck.items.splice(index, 1);
            }

            this.deck.hasChanges = true;
        },
        addCard: function (card) {

            var deckEntry = this.getDeckEntry(card);

            if (deckEntry)
                deckEntry.amount++;
            else
                this.deck.items.push({ id: card.id, amount: 1 });

            this.deck.hasChanges = true;
        },
        cardCount: function (card) {

            if (!this.hasDeck)
                return 0;

            var deckEntry = this.getDeckEntry(card);

            if (deckEntry)
                return deckEntry.amount;
            else
                return 0;
        },
        getDeckEntry: function (card) {
            return this.deck.items.find(function (c) {
                return c.id == card.id;
            });
        }
    }
});