using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CogsLite.Core;

namespace Cogslite
{
    public static class CardList
    {
		public static IEnumerable<Card> FromString(string infoSheet)
		{
			var lines = infoSheet.Split(Environment.NewLine);
			foreach (var line in lines)
			{
				var card = new Card();
				var parts = new List<string>(line.Split('|'));

				card.Name = parts[0];
				card.Type = parts.Count > 1 ? parts[1] : String.Empty;
				card.Tags = parts.Skip(2).ToArray();
				yield return card;
			}
		}
    }
}
