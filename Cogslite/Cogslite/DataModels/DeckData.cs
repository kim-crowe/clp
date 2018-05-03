using System;
using System.Linq;
using CogsLite.Core;

namespace Cogslite.DataModels
{
	public class DeckData
	{
		public string id { get; set; }
		public string name { get; set; }
		public string gameid { get; set; }
		public DeckItemData[] items { get; set; }

		public Deck ToDeck()
		{
			return new Deck
			{
				Id = ParseOrCreateGuid(id),
				GameId = ParseOrCreateGuid(gameid),
				Name = name,
				Items = items.Select(i => new DeckItem
				{
					Card = new Card
					{
						Name = i.card.name,
						Id = ParseOrCreateGuid(i.card.id)
					},
					Count = i.count
				}).ToArray()
			};
		}

		private Guid ParseOrCreateGuid(string id)
		{
			return String.IsNullOrEmpty(id) ? Guid.NewGuid() : Guid.Parse(id);
		}

		public static DeckData FromDeck(Deck deck)
		{
			return new DeckData
			{
				id = deck.Id.ToString(),
				gameid = deck.GameId.ToString(),
				name = deck.Name,
				items = new DeckItemData[0]
			};
		}
	}

	public class DeckItemData
	{
		public CardData card { get; set; }
		public int count { get; set; }
	}

	public class CardData
	{
		public string id { get; set; }
		public string name { get; set; }
	}
}
