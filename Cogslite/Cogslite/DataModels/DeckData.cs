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
				GameId = ShortGuid.Parse(gameid),
				Name = name,
				Items = items.Select(i => new DeckItem
				{
					CardId = ShortGuid.Parse(i.id),
					Amount = i.amount
				}).ToArray()
			};
		}

		private Guid ParseOrCreateGuid(string id)
		{
			return String.IsNullOrEmpty(id) ? Guid.NewGuid() : ShortGuid.Parse(id);
		}

		public static DeckData FromDeck(Deck deck)
		{
			return new DeckData
			{
				id = deck.Id.ToShortGuid(),
				gameid = deck.GameId.ToShortGuid(),
				name = deck.Name,
				items = deck.Items.Select(i => new DeckDataItem { id = i.CardId.ToShortGuid(), amount = i.Amount }).ToArray(),
				hasChanges = false
			};
		}
	}	

	public class DeckDataItem
	{
		public string id { get; set; }
		public int amount { get; set; }
	}
}
