export class Enums {}

Enums.Face = {
	Two: 2,
	Three: 3,
	Four: 4,
	Five: 5,
	Six: 6,
	Seven: 7,
	Eight: 8,
	Nine: 9,
	Ten: 10,
	Jack: 11,
	Queen: 12,
	King: 13,
	Ace: 14,
};

Enums.Face.isValid = (face) => (face >= 2 && face <= 14);

Enums.Face.toString = (face, pattern) => {
	if(pattern === 'l') {
		switch (face) {
			case Enums.Face.Two: return "Two";
			case Enums.Face.Three: return "Three";
			case Enums.Face.Four: return "Four";
			case Enums.Face.Five: return "Five";
			case Enums.Face.Six: return "Six";
			case Enums.Face.Seven: return "Seven";
			case Enums.Face.Eight: return "Eight";
			case Enums.Face.Nine: return "Nine";
			case Enums.Face.Ten: return "Ten";
			case Enums.Face.Jack: return "Jack";
			case Enums.Face.Queen: return "Queen";
			case Enums.Face.King: return "King";
			case Enums.Face.Ace: return "Ace";
		}
	} else {
		switch (face) {
			case Enums.Face.Two:
			case Enums.Face.Three:
			case Enums.Face.Four:
			case Enums.Face.Five:
			case Enums.Face.Six:
			case Enums.Face.Seven:
			case Enums.Face.Eight:
			case Enums.Face.Nine:
				return face.toString();
			case Enums.Face.Ten: return "T";
			case Enums.Face.Jack: return "J";
			case Enums.Face.Queen: return "Q";
			case Enums.Face.King: return "K";
			case Enums.Face.Ace: return "A";
		}
	}
	return "";
};

Enums.Face.parse = (from) => {
	if (from && typeof from === 'string') from = from.toLowerCase();
	switch (from)
	{
		case Enums.Face.Two:
		case "2":
		case "two":
		case "twos":
		case "deuce":
		case "deuces":
			return Enums.Face.Two;
		case Enums.Face.Three:
		case "3":
		case "three":
		case "threes":
			return Enums.Face.Three;
		case Enums.Face.Four:
		case "4":
		case "four":
		case "fours":
			return Enums.Face.Four;
		case Enums.Face.Five:
		case "5":
		case "five":
		case "fives":
			return Enums.Face.Five;
		case Enums.Face.Six:
		case "6":
		case "six":
		case "sixes":
			return Enums.Face.Six;
		case Enums.Face.Seven:
		case "7":
		case "seven":
		case "sevens":
			return Enums.Face.Seven;
		case Enums.Face.Eight:
		case "8":
		case "eight":
		case "eights":
			return Enums.Face.Eight;
		case Enums.Face.Nine:
		case "9":
		case "nine":
		case "nines":
			return Enums.Face.Nine;
		case Enums.Face.Ten:
		case "10":
		case "t":
		case "ten":
		case "tens":
			return Enums.Face.Ten;
		case Enums.Face.Jack:
		case "j":
		case "jack":
		case "jacks":
			return Enums.Face.Jack;
		case Enums.Face.Queen:
		case "q":
		case "queen":
		case "queens":
			return Enums.Face.Queen;
		case Enums.Face.King:
		case "k":
		case "king":
		case "kings":
			return Enums.Face.King;
		case Enums.Face.Ace:
		case "a":
		case "ace":
		case "aces":
			return Enums.Face.Ace;
		default:
			return null;
	}
};

Enums.Suit = {
	Heart: 1,
	Diamond: 2,
	Club: 3,
	Spade: 4,
};

Enums.Suit.isValid = (suit) => (suit >= 1 && suit <= 4);

Enums.Suit.toString = (suit, pattern) => {
	if(pattern === 'l') {
		switch (suit) {
			case Enums.Suit.Heart: return "Heart";
			case Enums.Suit.Diamond: return "Diamond";
			case Enums.Suit.Club: return "Club";
			case Enums.Suit.Spade: return "Spade";
		}
	} else {
		switch (suit) {
			case Enums.Suit.Heart: return "h";
			case Enums.Suit.Diamond: return "d";
			case Enums.Suit.Club: return "c";
			case Enums.Suit.Spade: return "s";
		}
	}
	return "";
};

Enums.Suit.parse = (from) => {
	if (from && typeof from === 'string') from = from.toLowerCase();
	switch (from)
	{
		case Enums.Suit.Heart:
		case "h":
		case "heart":
		case "hearts":
			return Enums.Suit.Heart;
		case Enums.Suit.Diamond:
		case "d":
		case "diamond":
		case "diamonds":
			return Enums.Suit.Diamond;
		case Enums.Suit.Club:
		case "c":
		case "club":
		case "clubs":
			return Enums.Suit.Club;
		case Enums.Suit.Spade:
		case "s":
		case "spade":
		case "spades":
			return Enums.Suit.Spade;
		default:
			return null;
	}
};

Enums.HandTypes = {
	HighCard: 1,
	Pair: 2,
	TwoPair: 3,
	ThreeOfAKind: 4,
	Straight: 5,
	Flush: 6,
	FullHouse: 7,
	FourOfAKind: 8,
	StraightFlush: 9,
	RoyalFlush: 10,
};

Enums.HandTypes.isValid = (type) => (type >= 1 && type <= 10);

Enums.HandTypes.toString = (type) => {
	switch (type) {
		case Enums.HandTypes.HighCard: return "HighCard";
		case Enums.HandTypes.Pair: return "Pair";
		case Enums.HandTypes.TwoPair: return "TwoPair";
		case Enums.HandTypes.ThreeOfAKind: return "ThreeOfAKind";
		case Enums.HandTypes.Straight: return "Straight";
		case Enums.HandTypes.Flush: return "Flush";
		case Enums.HandTypes.FullHouse: return "FullHouse";
		case Enums.HandTypes.FourOfAKind: return "FourOfAKind";
		case Enums.HandTypes.StraightFlush: return "StraightFlush";
		case Enums.HandTypes.RoyalFlush: return "RoyalFlush";
		default: return "";
	}
};
Enums.BettingRoundStatus = {
	Unopened: 1,
	Folded: 2,
	Checked: 3,
	Called: 4,
	Raised: 5,
};

Enums.TableStatus = {
	BeforeFlop: 1,
	BeforeTurn: 2,
	BeforeRiver: 3,
	Complete: 4,
};
