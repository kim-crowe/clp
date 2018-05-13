var gameServiceFactory = {
    methods: {
        gameService: function ($http) {
            return {
                $http: $http,
                getGames: function (search) {
                    return this.$http.post('/Home?handler=Search', search);
                }
            }
        }
    }
};