Vue.http.headers.common['XSRF-TOKEN'] = $("#af-token").val();

var gamesVue = new Vue({
    el: '#games',
    mixins: [gameServiceFactory],
    data: {        
        isSignedIn: false,
        numberOfPages: 1,
        search: { page: 1, itemsPerPage: 6, searchText: '' },
        games: []        
    },
    created: function () {
        this.isSignedIn = $('#is-signed-in').val();
        this.refreshGames();
    },
    computed: {
        hasPages: function () {
            return this.numberOfPages > 1;
        }
    },
    methods: {
        onSearchChange: function (e) {
            this.search.searchText = e.target.value;
            this.refreshGames();
        },        
        refreshGames: function () {
            this.gameService(this.$http).getGames(this.search).then(data => {
                this.games = data.body.games;
                this.numberOfPages = data.body.numberOfPages;
            });
        },
        gameUrl: function (game) {
            return "/Cards?gameId=" + game.id;
        },
        imageUrl: function (game) {
            return "/Image?imageId=" + game.id;
        },
        firstPage: function () {
            this.search.page = 1;
            this.refreshGames();
        },
        previousPage: function () {
            if (this.search.page > 1) {
                this.search.page--;
                this.refreshGames();
            }
        },
        nextPage: function () {
            if (this.search.page < this.numberOfPages) {
                this.search.page++;
                this.refreshGames();
            }
        },
        lastPage: function () {
            this.search.page = this.numberOfPages;
            this.refreshGames();
        }       
    }
});