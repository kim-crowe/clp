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
		public DeckDataItem[] items { get; set; }
		public bool hasChanges { get; set; }

		public Deck ToDeck()
		{
			return new Deck
			{
				Id = ParseOrCreateGuid(id),
				GameId = ParseOrCreateGuid(gameid),
				Name = name,
				Items = items.Select(i => new DeckItem
				{
					CardId = i.id,
					Amount = i.amount
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
				items = deck.Items.Select(i => new DeckDataItem { id = i.CardId, amount = i.Amount }).ToArray(),
				hasChanges = false
			};
		}
	}	

	public class DeckDataItem
	{
		public Guid id { get; set; }
		public int amount { get; set; }
	}
}
