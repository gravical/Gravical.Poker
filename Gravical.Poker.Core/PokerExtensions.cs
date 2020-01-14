using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gravical.Poker.Core
{
    public static class PokerExtensions
    {
        public static bool IsValid(this Suit suit)
        {
            return (int)suit >= 1 && (int)suit <= 4;
        }

        public static bool IsValid(this Face face)
        {
            return (int)face >= 2 && (int)face <= 14;
        }

        public static bool IsValid(this TableStatus status)
        {
            return Enum.IsDefined(typeof(TableStatus), status);
        }

        public static bool IsValid(this BettingRoundStatus status)
        {
            return Enum.IsDefined(typeof(BettingRoundStatus), status);
        }

        public static bool IsValid(this HandTypes type)
        {
            return Enum.IsDefined(typeof(HandTypes), type);
        }

        public static bool AnyIntersection(this IEnumerable<Card> cards, params Card[] within)
        {
            if (cards == null) return false;
            if (within == null) return false;
            return cards.Any(card => card.AnyIntersection(within));
        }

        public static bool AnyIntersection(this Card card, params Card[] within)
        {
            if (within == null) return false;
            foreach (var candidate in within)
            {
                if (card.Equals(candidate)) return true;
            }
            return false;
        }

        public static Card[] SortByFace(this IEnumerable<Card> cards)
        {
            return cards?.OrderByDescending(_ => _.Face).ThenByDescending(_ => _.Suit).ToArray();
        }

        public static Suit? ParseCardSuit(this string from)
        {
            switch (from?.ToLower())
            {
                case "h":
                case "heart":
                case "hearts":
                    return Suit.Heart;
                case "d":
                case "diamond":
                case "diamonds":
                    return Suit.Diamond;
                case "c":
                case "club":
                case "clubs":
                    return Suit.Club;
                case "s":
                case "spade":
                case "spades":
                    return Suit.Spade;
                default:
                    return null;
            }
        }

        public static Face? ParseCardFace(this string from)
        {
            switch (from?.ToLower())
            {
                case "2":
                case "two":
                case "twos":
                case "deuce":
                case "deuces":
                    return Face.Two;
                case "3":
                case "three":
                case "threes":
                    return Face.Three;
                case "4":
                case "four":
                case "fours":
                    return Face.Four;
                case "5":
                case "five":
                case "fives":
                    return Face.Five;
                case "6":
                case "six":
                case "sixes":
                    return Face.Six;
                case "7":
                case "seven":
                case "sevens":
                    return Face.Seven;
                case "8":
                case "eight":
                case "eights":
                    return Face.Eight;
                case "9":
                case "nine":
                case "nines":
                    return Face.Nine;
                case "10":
                case "t":
                case "ten":
                case "tens":
                    return Face.Ten;
                case "j":
                case "jack":
                case "jacks":
                    return Face.Jack;
                case "q":
                case "queen":
                case "queens":
                    return Face.Queen;
                case "k":
                case "king":
                case "kings":
                    return Face.King;
                case "a":
                case "ace":
                case "aces":
                    return Face.Ace;
                default:
                    return null;
            }
        }

        private static readonly char[] Spaces = new[] { ' ' };

        public static Card[] ParseCards(this string from)
        {
            if (from == null) return null;

            var split = from?.Split(Spaces, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length == 0) return new Card[0];

            var cards = split.Select(_ => _.ParseCard()).ToArray();
            if (cards.Any(_ => _ == null)) throw new ArgumentException("One or more cards could not be parsed", nameof(from));

            return cards.Cast<Card>().ToArray();
        }

        public static readonly Regex CardShortPattern = new Regex("^([TJQKA0-9]0?)([hcds])$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static readonly Regex CardLongPattern = new Regex("^(Two|Three|Four|Five|Six|Seven|Eight|Nine|Ten|Jack|Queen|King|Ace) of (Heart|Diamond|Club|Spade)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static Card? ParseCard(this string from)
        {
            if (from == null) return null;

            var match = CardShortPattern.Match(from);
            if (!match.Success) match = CardLongPattern.Match(from);
            if (!match.Success) return null;

            var face = match.Groups[1].Value.ParseCardFace();
            var suit = match.Groups[2].Value.ParseCardSuit();

            if (face.HasValue && suit.HasValue)
                return new Card(face.Value, suit.Value);
            else
                return null;
        }

        public static Card? ParseCard(this byte from)
        {
            var face = (Face)((from >> 4) & 0xF);
            var suit = (Suit)(from & 0xF);
            if (!face.IsValid() || !suit.IsValid()) return null;

            return new Card(face, suit);
        }

        public static string ToCardString(this IEnumerable<Card> cards)
        {
            return cards == null ? null : string.Join(" ", cards);
        }
    }
}