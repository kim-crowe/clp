export class Deck {
  constructor(id, name, gameId, items, version) {
    this.id = id;
    this.name = name;
    this.gameId = gameId;
    this.items = items;
    this.version = version;
    this.hasChanges = !id.length > 0;
  }

  removeCard(card) {
    var deckEntry = this.getDeckEntry(card);

    if(deckEntry) {
      deckEntry.amount--;

      if (deckEntry.amount < 1) {
        var index = this.items.indexOf(deckEntry);
        this.items.splice(index, 1);
      }

      this.hasChanges = true;
    }
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
