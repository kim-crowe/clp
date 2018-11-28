export class Deck {
  constructor() {
    this.items = [];
    this.hasChanges = false;
  }

  removeCard(card) {
    var deckEntry = this.getDeckEntry(card);
    deckEntry.amount--;

    if (deckEntry.amount < 1) {
      var index = this.deck.items.indexOf(deckEntry);
      this.items.splice(index, 1);
    }

    this.hasChanges = true;
  }

  addCard(card) {

    var deckEntry = this.getDeckEntry(card);

    if (deckEntry)
      deckEntry.amount++;
    else
      this.items.push({ id: card.id, amount: 1 });

    this.hasChanges = true;
  }

  countOf(card) {

    var deckEntry = this.getDeckEntry(card);

    if (deckEntry)
      return deckEntry.amount;
    else
      return 0;
  }

  getDeckEntry(card) {
    return this.items.find(function (c) {
      return c.id == card.id;
    });
  }
};
