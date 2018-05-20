var cardServiceFactory = {
    methods: {
        cardService: function ($http) {
            return {
                $http: $http,
                getCards: function (gameId, search) {
                    return this.$http.post('/Cards?handler=CardSearch&gameId=' + gameId, search);                    
                },
                updateCard: function (card) {
                    return this.$http.post('/Cards?handler=CardUpdate', card);
                }
            }
        }
    }
};