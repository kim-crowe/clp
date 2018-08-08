var cardServiceFactory = {
    methods: {
        cardService: function ($http) {
            return {
                $http: $http,
                getCards: function (ownerId, gameId, search) {
                    return this.$http.post('/Cards?handler=CardSearch&ownerId=' + ownerId + '&gameId=' + gameId, search);                    
                },
                updateCard: function (card) {
                    return this.$http.post('/Cards?handler=CardUpdate', card);
                }
            }
        }
    }
};