var deckServiceFactory = {
    methods: {
        deckService: function ($http) {
            return {
                $http: $http,
                save: function (deck) {
                    return this.$http.post('/Deck?handler=Deck', deck);
                },
                deckList: function (gameId) {
                    return this.$http.get('/Deck?handler=List&gameId=' + gameId);
                }
            }
        }
    }
};