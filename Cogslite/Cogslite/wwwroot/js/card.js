Vue.http.headers.common['XSRF-TOKEN'] = $("#af-token").val();

var cardsVue = new Vue({
    el: '#cardApp',
    mixins: [cardServiceFactory],
    data: {
        gameId: '',
        numberOfPages: 1,
        search: { page: 1, itemsPerPage: 20, cardName: '', cardType: "-1", tags: [] },
        cards: [],
        tags: [],
        deck: {name: 'Test Deck', items: []}
    },
    created: function () {
        this.gameId = $('#game-id').val();
        this.refreshCards();
    },
    methods: {
        refreshCards: function () {
            this.cardService(this.$http).getCards(this.gameId, this.search).then(data => {                
                this.cards = data.body.cards;
                this.numberOfPages = data.body.numberOfPages;
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
        }
    }
});