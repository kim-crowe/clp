var cogsModule = angular.module("cogs", []);

cogsModule.factory('cardsService', function ($http) {
    var cardsService = {
        antiForgeryToken: $('#af-token').val(),
        getCards: function (search) {
            var gameId = $('#gameId').val();
            var promise = $http.post('/Cards?handler=CardSearch&gameId=' + gameId, search, { headers: { 'XSRF-TOKEN': this.antiForgeryToken } })
                .then(function (response) {
                    return response.data;
                });
            return promise;
        },
        getTags: function () {
            var gameId = $('#gameId').val();
            var promise = $http.get('/Cards?handler=Tags&gameId=' + gameId)
                .then(function (response) {
                    return response.data;
                });
            return promise;
        },
        updateCard: function (cardUpdate) {
            var gameId = $('#gameId').val();
            var promise = $http.post('/Cards?handler=CardUpdate&gameId=' + gameId, cardUpdate, { headers: { 'XSRF-TOKEN': this.antiForgeryToken } })
                .then(function (response) {
                    return response.data;
                });
            return promise;
        }
    };

    return cardsService;
});

cogsModule.controller("cardsController", function ($scope, cardsService) {
    $scope.numberOfPages = 1;
    $scope.search = { page: 1, itemsPerPage: 20, cardName: '', cardType: "-1", tags: [] };
    $scope.isSignedIn = false;
    $scope.isGameOwner = false;
    $scope.cards = [];
    $scope.tags = [];
    $scope.cardEdit = {};
    $scope.selectedCard = null;
    $scope.isInitialised = false;

    $scope.$watch('search.cardName', function () {
        if ($scope.isInitialised)
            loadCards();
    });

    $scope.$watch('search.tags', function () {
        if ($scope.isInitialised)
            loadCards();
    });

    loadCards = function () {
        cardsService.getCards($scope.search).then(function (data) {
            $scope.cards = data.cards;
            $scope.numberOfPages = data.numberOfPages;
        });
    }    
    
    loadCards();
    $scope.isInitialised = true;

    $scope.removeCard = function (cardId) {

        var existingItem = $scope.deck.items.filter(function (item) {
            return item.card.id == cardId;
        });

        if (existingItem.length > 0) {
            existingItem[0].count--;
            if (existingItem[0].count < 1) {
                var index = $scope.deck.items.indexOf(existingItem[0]);
                $scope.deck.items.splice(index, 1);
            }
        }
    };

    $scope.imageUrl = function (card) {
        return "/Image?imageId=" + card.id;
    }

    $scope.firstPage = function () {
        $scope.search.page = 1;
        loadCards();
    }

    $scope.previousPage = function () {
        if ($scope.search.page > 1) {
            $scope.search.page--;
            loadCards();
        }
    }

    $scope.nextPage = function () {
        if ($scope.search.page < $scope.numberOfPages) {
            $scope.search.page++;
            loadCards();
        }
    }

    $scope.lastPage = function () {
        $scope.search.page = $scope.numberOfPages;
        loadCards();
    }

    $scope.applyFilter = function () {
        $scope.search.cardName = $scope.searchText;
        $scope.search.page = 1;
        loadCards();
    }

    $scope.showDetails = function (card) {
        $scope.selectedCard = card;
        $scope.cardEdit = { cardId: $scope.selectedCard.id, name: $scope.selectedCard.name, type: $scope.selectedCard.type, tags: $scope.tags.join() };

        $('#cardDetails').modal({})
    }

    $scope.updateCard = function () {
        cardsService.updateCard($scope.cardEdit).then(function (data) {
            $scope.selectedCard.name = $scope.cardEdit.name;
            $scope.selectedCard.type = $scope.cardEdit.type;
            $scope.selectedCard.tags = $scope.cardEdit.tags.split(',');
            $('#cardDetails').model('hide');
        });

    }
});    