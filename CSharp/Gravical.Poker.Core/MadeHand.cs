using System;
using System.Collections.Generic;
using System.Linq;

namespace Gravical.Poker.Core
{
	/// <summary>
	/// A MadeHand represents a completed 5-card poker hand which can be scored (for sorting winners relative to losers).
	/// </summary>
	public class MadeHand
	{
		public HandTypes Type { get; }
		public Card[] Played { get; }
		public Dictionary<Card, List<Card>> Alternates { get; }
		public long Score { get; private set; }

		public MadeHand(HandTypes type, Card[] played, Dictionary<Card, List<Card>> alternates = null)
		{
			Guards.ArgumentSuccess(type.IsValid(), nameof(type));
			Guards.ArgumentSuccess(played != null && played.Length == 5, nameof(played));

			Type = type;
			Played = played;
			Alternates = alternates;

			ScoreHand();
		}

		public MadeHand(byte[] binary)
		{
			Guards.ArgumentHasExactSize(binary, 21, nameof(binary));

			Type = (HandTypes)binary[0];
			Played = new Card[5];
			Alternates = null;

			// For each card played
			var alternates = new Dictionary<Card, List<Card>>();
			for (int iPlayed = 0; iPlayed < 5; iPlayed++)
			{
				var iBinaryAtCard = 1 + iPlayed * 4;

				// Add this card
				Played[iPlayed] = new Card(binary[iBinaryAtCard]);

				// Add all three alternates
				var alt = new List<Card>();
				for (int iAlt = 0; iAlt < 3; iAlt++)
				{
					var altCard = binary[iBinaryAtCard + 1 + iAlt];
					if (altCard != 0)
					{
						alt.Add(new Card(altCard));
					}
				}
				if (alt.Count > 0)
				{
					alternates.Add(Played[iPlayed], alt);
				}
			}

			// If we have any alternates, set the dictionary
			if (alternates.Count > 0) Alternates = alternates;

			ScoreHand();
		}

		private void ScoreHand()
		{
			Score =
				((long)Type << 40) |
				((long)Played[0].Face << 32) |
				((long)Played[1].Face << 24) |
				((long)Played[2].Face << 16) |
				((long)Played[3].Face << 8) |
				((long)Played[4].Face << 0);
		}

		public static MadeHand MakeHand(Card[] cards)
		{
			Guards.Assert(cards.Length >= 5 && cards.Length <= 7, $"Can't make a hand with {cards.Length} cards");

			// Ensure they are sorted
			cards = cards.SortByFace();

			// Group them all by face
			var groups = new HandGrouping(cards);

			var straightFlush = TryMakeStraightFlush(cards);
			if (straightFlush != null) return straightFlush;

			var fourOfAKind = TryMakeFourOfAKind(cards, groups);
			if (fourOfAKind != null) return fourOfAKind;

			var fullHouse = TryMakeFullHouse(cards, groups);
			if (fullHouse != null) return fullHouse;

			var flush = TryMakeFlush(cards);
			if (flush != null) return flush;

			var straight = TryMakeStraight(cards);
			if (straight != null) return straight;

			var threeOfAKind = TryMakeThreeOfAKind(groups);
			if (threeOfAKind != null) return threeOfAKind;

			var twoPair = TryMakeTwoPair(cards, groups);
			if (twoPair != null) return twoPair;

			var pair = TryMakePair(groups);
			if (pair != null) return pair;

			var highCard = TryMakeHighCard(cards);
			if (highCard != null) return highCard;

			throw new InvalidOperationException("Impossible poker hand");
		}

		private static MadeHand TryMakeHighCard(Card[] cards)
		{
			var played = cards.SortByFace().Take(5).ToArray();
			return new MadeHand(HandTypes.HighCard, played);
		}

		private static MadeHand TryMakePair(HandGrouping groups)
		{
			if (groups.Pair.Count != 1) return null;
			if (groups.Single.Count < 3) return null;

			var played = groups.Pair[0]
				.Concat(groups.Single.SelectMany(_ => _).Take(3))
				.ToArray();
			return new MadeHand(HandTypes.Pair, played);
		}

		private static MadeHand TryMakeTwoPair(Card[] cards, HandGrouping groups)
		{
			if (groups.Pair.Count < 2) return null;

			var high = (groups.Pair.Count > 2 ? groups.Pair[2] : new List<Card>())
				.Concat(groups.Single.SelectMany(_ => _))
				.SortByFace()
				.Take(1);

			var played = groups.Pair[0]
				.Concat(groups.Pair[1])
				.Concat(high)
				.ToArray();
			return new MadeHand(HandTypes.TwoPair, played, GetFaceAlternates(played, cards, 1));
		}

		private static MadeHand TryMakeThreeOfAKind(HandGrouping groups)
		{
			if (groups.ThreeOfAKind.Count < 1) return null;

			var high = (groups.ThreeOfAKind.Count > 1 ? groups.ThreeOfAKind[1] : new List<Card>())
				.Concat(groups.Pair.SelectMany(_ => _))
				.Concat(groups.Single.SelectMany(_ => _))
				.SortByFace()
				.Take(2);

			var played = groups.ThreeOfAKind[0]
				.Concat(high)
				.ToArray();
			return new MadeHand(HandTypes.ThreeOfAKind, played);
		}

		private static MadeHand TryMakeStraight(Card[] cards)
		{
			var played = GetStraight(cards, false);
			return played == null ? null : new MadeHand(HandTypes.Straight, played, GetFaceAlternates(played, cards, 5));
		}

		private static MadeHand TryMakeFlush(Card[] cards)
		{
			foreach (var suit in new[] { Suit.Heart, Suit.Diamond, Suit.Club, Suit.Spade })
			{
				var flush = cards.Where(_ => _.Suit == suit).SortByFace().Take(5).ToArray();
				if (flush.Length == 5)
				{
					return new MadeHand(HandTypes.Flush, flush);
				}
			}

			return null;
		}

		private static MadeHand TryMakeFullHouse(Card[] cards, HandGrouping groups)
		{
			List<Card> high = null;
			if (groups.ThreeOfAKind.Count > 1)
			{
				high = groups.ThreeOfAKind[1].Take(2).ToList();
			}
			else if (groups.ThreeOfAKind.Count == 1)
			{
				if (groups.Pair.Count <= 0) return null;
				high = groups.Pair[0];
			}
			else
			{
				return null;
			}

			var played = groups.ThreeOfAKind[0]
				.Concat(high)
				.ToArray();
			return new MadeHand(HandTypes.FullHouse, played, GetFaceAlternates(played, cards, 2));
		}

		private static MadeHand TryMakeFourOfAKind(Card[] cards, HandGrouping groups)
		{
			if (groups.FourOfAKind.Count <= 0) return null;

			var high = groups.ThreeOfAKind.SelectMany(_ => _)
				.Concat(groups.Pair.SelectMany(_ => _))
				.Concat(groups.Single.SelectMany(_ => _))
				.SortByFace();

			var played = groups.FourOfAKind[0]
				.Concat(high)
				.Take(5)
				.ToArray();
			return new MadeHand(HandTypes.FourOfAKind, played, GetFaceAlternates(played, cards, 1));
		}

		private static MadeHand TryMakeStraightFlush(Card[] cards)
		{
			var played = GetStraight(cards, true);
			if (played == null) return null;

			return new MadeHand(played[0].Face == Face.Ace ? HandTypes.RoyalFlush : HandTypes.StraightFlush, played);
		}

		private static Card[] GetStraight(Card[] cards, bool flush)
		{
			var straight = new List<Card>(5);

			// Accomodate the wheel with minus
			var minus = cards[0].Face == Face.Ace ? 3 : 4;
			for (int iTop = 0; iTop < cards.Length - minus; iTop++)
			{
				var top = cards[iTop];
				straight.Clear();
				straight.Add(top);

				// Can we make a straight from here
				var last = top;
				Card next;
				for (int iNext = iTop + 1; iNext < cards.Length; iNext++)
				{
					next = cards[iNext];

					// If this card is a larger face, error
					if (next.Face > last.Face) throw new InvalidOperationException("Internal error");

					// If this card is the same face, try the next one
					if (next.Face == last.Face) continue;

					// If this card skips a face, not a straight
					if (next.Face < last.Face - 1) break;

					// If we need a straight flush, match the suit
					if (flush && next.Suit != top.Suit) continue;

					// This is straightening
					last = next;
					straight.Add(next);
					if (straight.Count == 5)
					{
						return straight.ToArray();
					}
				}

				// Check for the wheel
				if (straight[0].Face == Face.Five && straight.Count == 4)
				{
					// Loop all aces looking for a match (try them all for royal flush case)
					for (int iAce = 0; iAce < cards.Length - 4; iAce++)
					{
						if (cards[iAce].Face != Face.Ace) break;

						if (!flush || cards[iAce].Suit == top.Suit)
						{
							straight.Add(cards[iAce]);
							return straight.ToArray();
						}
					}
				}
			}
			return null;
		}

		private static Dictionary<Card, List<Card>> GetFaceAlternates(Card[] played, Card[] cards, int last)
		{
			var results = new Dictionary<Card, List<Card>>();

			for (int iPlayed = played.Length - last; iPlayed < played.Length; iPlayed++)
			{
				var play = played[iPlayed];
				foreach (var card in cards)
				{
					if (card.Face == play.Face && card.Suit != play.Suit)
					{
						if (!results.ContainsKey(play)) results.Add(play, new List<Card>());
						results[play].Add(card);
					}
				}
			}
			return results.Count == 0 ? null : results;
		}

		public override string ToString() => $"{Type}: {Played.ToCardString()}";

		public byte[] Serialize()
		{
			var alternates = Alternates ?? new Dictionary<Card, List<Card>>();

			// Start with the made hand type
			var binary = new byte[21];
			binary[0] = (byte)Type;

			// For every played card
			for (int iPlayed = 0; iPlayed < 5; iPlayed++)
			{
				var iBinaryAtCard = 1 + iPlayed * 4;

				// Add this card
				binary[iBinaryAtCard] = Played[iPlayed].Key;

				// Add all three alternates
				var alt = alternates.ContainsKey(Played[iPlayed]) ? alternates[Played[iPlayed]] : new List<Card>();
				for (int iAlt = 0; iAlt < 3; iAlt++)
				{
					binary[iBinaryAtCard + 1 + iAlt] = iAlt >= alt.Count ? (byte)0 : alt[iAlt].Key;
				}
			}

			return binary;
		}
	}
}