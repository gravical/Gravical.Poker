import { Enums } from "./Enums.mjs";
import { HandGrouping } from "./HandGrouping.mjs";
import { Card } from "./Card.mjs";

// A MadeHand represents a completed 5-card poker hand which can be scored (for sorting winners relative to losers).
export class MadeHand {

	static makeFromParts(type, played, alternates = null) {

		if (!Enums.HandTypes.isValid(type)) throw `Type is not valid: ${type}`;
		if (!played) throw "Played is null";
		if (played.length != 5) throw "Played length must be 5";
		if (alternates && !(alternates instanceof Map)) throw "Alternates has an invalid type";

		let hand = new MadeHand();
		hand.type = type;
		hand.played = played;
		hand.alternates = alternates;
		hand.scoreHand();
		return hand;
	}

	static makeFromBinary(binary) {

		if (!binary || !(binary instanceof Uint8Array)) throw "Binary must be a Uint8Array";
		if (binary.length != 21) throw "Binary must be exactly 21 bytes";

		let hand = new MadeHand();
		hand.type = binary[0];
		hand.played = [];
		hand.alternates = null;

		// For each card played
		let alternates = new Map();
		for (let iPlayed = 0; iPlayed < 5; iPlayed++) {
			let iBinaryAtCard = 1 + iPlayed * 4;

			// Add this card
			let card = Card.parse(binary[iBinaryAtCard]);
			hand.played.push(card);

			// Add all three alternates
			let alt = [];
			for (let iAlt = 0; iAlt < 3; iAlt++) {
				let altCard = binary[iBinaryAtCard + 1 + iAlt];
				if (altCard != 0) {
					alt.push(Card.parse(altCard));
				}
			}
			if (alt.length > 0) {
				alternates.set(card.uint8(), alt);
			}
		}

		// If we have any alternates, set the dictionary
		if (alternates.size > 0) hand.alternates = alternates;

		hand.scoreHand();
		return hand;
	}

	static makeHand(cards) {

		if (!cards) throw "Cards is null";
		if (!(cards instanceof Array)) throw "Cards is an invalid type";
		if (cards.length < 5 || cards.length > 7) throw `Can't make a hand with ${cards.length} cards`;

		// Ensure they are sorted
		cards = Card.sort(cards, "desc");

		// Group them all by face
		let groups = new HandGrouping(cards);

		let straightFlush = this.tryMakeStraightFlush(cards);
		if (straightFlush != null) return straightFlush;

		let fourOfAKind = this.tryMakeFourOfAKind(cards, groups);
		if (fourOfAKind != null) return fourOfAKind;

		let fullHouse = this.tryMakeFullHouse(cards, groups);
		if (fullHouse != null) return fullHouse;

		let flush = this.tryMakeFlush(cards);
		if (flush != null) return flush;

		let straight = this.tryMakeStraight(cards);
		if (straight != null) return straight;

		let threeOfAKind = this.tryMakeThreeOfAKind(groups);
		if (threeOfAKind != null) return threeOfAKind;

		let twoPair = this.tryMakeTwoPair(cards, groups);
		if (twoPair != null) return twoPair;

		let pair = this.tryMakePair(groups);
		if (pair != null) return pair;

		let highCard = this.tryMakeHighCard(cards);
		if (highCard != null) return highCard;

		throw "Impossible poker hand";
	}

	scoreHand() {
		this.score =
			(this.type << 40) |
			(this.played[0].face << 32) |
			(this.played[1].face << 24) |
			(this.played[2].face << 16) |
			(this.played[3].face << 8) |
			(this.played[4].face << 0);
	}

	static tryMakeHighCard(cards) {
		let played = Card.sort(cards, 'desc').splice(0, 5);
		return MadeHand.makeFromParts(Enums.HandTypes.HighCard, played);
	}

	static tryMakePair(groups) {
		if (groups.pair.length != 1) return null;
		if (groups.single.length < 3) return null;

		let played = groups.pair[0].concat(MadeHand.flattenArrayOfArrayOfCards(groups.single).slice(0, 3));
		return MadeHand.makeFromParts(Enums.HandTypes.Pair, played);
	}

	static tryMakeTwoPair(cards, groups) {
		if (groups.pair.length < 2) return null;

		let high = (groups.pair.length > 2 ? groups.pair[2] : [])
			.concat(MadeHand.flattenArrayOfArrayOfCards(groups.single));
		high = Card.sort(high, 'desc').slice(0, 1);

		let played = groups.pair[0]
			.concat(groups.pair[1])
			.concat(high);
		return MadeHand.makeFromParts(Enums.HandTypes.TwoPair, played, MadeHand.getFaceAlternates(played, cards, 1));
	}

	static tryMakeThreeOfAKind(groups) {
		if (groups.threeOfAKind.length < 1) return null;

		let high = (groups.threeOfAKind.length > 1 ? groups.threeOfAKind[1] : [])
			.concat(MadeHand.flattenArrayOfArrayOfCards(groups.pair))
			.concat(MadeHand.flattenArrayOfArrayOfCards(groups.single));
		high = Card.sort(high, 'desc').slice(0, 2);

		let played = groups.threeOfAKind[0].concat(high);
		return MadeHand.makeFromParts(Enums.HandTypes.ThreeOfAKind, played);
	}

	static tryMakeStraight(cards) {
		let played = MadeHand.getStraight(cards, false);
		return played == null ? null : MadeHand.makeFromParts(Enums.HandTypes.Straight, played, MadeHand.getFaceAlternates(played, cards, 5));
	}

	static tryMakeFlush(cards) {
		for (const suit of [ Enums.Suit.Heart, Enums.Suit.Diamond, Enums.Suit.Club, Enums.Suit.Spade ]) {
			let flush = Card.sort(cards.filter(_ => _.suit == suit), 'desc').slice(0, 5);
			if (flush.length == 5) {
				return MadeHand.makeFromParts(Enums.HandTypes.Flush, flush);
			}
		}
		return null;
	}

	static tryMakeFullHouse(cards, groups) {
		let high = null;
		if (groups.threeOfAKind.length > 1) {
			high = groups.threeOfAKind[1].slice(0, 2);
		} else if (groups.threeOfAKind.length == 1) {
			if (groups.pair.length <= 0) return null;
			high = groups.pair[0];
		} else {
			return null;
		}

		let played = groups.threeOfAKind[0].concat(high);
		return MadeHand.makeFromParts(Enums.HandTypes.FullHouse, played, MadeHand.getFaceAlternates(played, cards, 2));
	}

	static tryMakeFourOfAKind(cards, groups) {
		if (groups.fourOfAKind.length <= 0) return null;

		let high = MadeHand.flattenArrayOfArrayOfCards(groups.threeOfAKind)
			.concat(MadeHand.flattenArrayOfArrayOfCards(groups.pair))
			.concat(MadeHand.flattenArrayOfArrayOfCards(groups.single));
		high = Card.sort(high, 'desc');

		let played = groups.fourOfAKind[0]
			.concat(high)
			.slice(0, 5);
		return MadeHand.makeFromParts(Enums.HandTypes.FourOfAKind, played, MadeHand.getFaceAlternates(played, cards, 1));
	}

	static tryMakeStraightFlush(cards) {
		let played = MadeHand.getStraight(cards, true);
		if (played == null) return null;

		let type = played[0].face == Enums.Face.Ace
			? Enums.HandTypes.RoyalFlush
			: Enums.HandTypes.StraightFlush;
		return MadeHand.makeFromParts(type, played);
	}

	static getStraight(cards, flush) {
		let straight = [];

		// Accomodate the wheel with minus
		let minus = cards[0].face == Enums.Face.Ace ? 3 : 4;
		for (let iTop = 0; iTop < cards.length - minus; iTop++) {
			let top = cards[iTop];
			straight = [];
			straight.push(top);

			// Can we make a straight from here
			let last = top;
			let next = null;
			for (let iNext = iTop + 1; iNext < cards.length; iNext++) {
				next = cards[iNext];

				// If this card is a larger face, error
				if (next.face > last.face) throw "Internal error";

				// If this card is the same face, try the next one
				if (next.face == last.face) continue;

				// If this card skips a face, not a straight
				if (next.face < last.face - 1) break;

				// If we need a straight flush, match the suit
				if (flush && next.suit != top.suit) continue;

				// This is straightening
				last = next;
				straight.push(next);
				if (straight.length == 5) {
					return straight;
				}
			}

			// Check for the wheel
			if (straight[0].face == Enums.Face.Five && straight.length == 4) {
				// Loop all aces looking for a match (try them all for royal flush case)
				for (let iAce = 0; iAce < cards.length - 4; iAce++) {
					if (cards[iAce].face != Enums.Face.Ace) break;

					if (!flush || cards[iAce].suit == top.suit) {
						straight.push(cards[iAce]);
						return straight;
					}
				}
			}
		}
		return null;
	}

	static getFaceAlternates(played, cards, last) {
		let results = new Map();
		for (let iPlayed = played.length - last; iPlayed < played.length; iPlayed++) {
			let play = played[iPlayed];
			for (const card of cards) {
				if (card.face == play.face && card.suit != play.suit) {
					if (!results.has(play)) results.set(play, []);
					results.set(play.uint8(), card);
				}
			}
		}
		return results.length == 0 ? null : results;
	}

	toString() {
		if (!this.played) return '';
		return `${Enums.HandTypes.toString(this.type)}: ${this.played.map(_ => _.toString()).join(' ')}`;
	}

	toBinary() {
		let alternates = this.alternates ? this.alternates : new Map();

		// Start with the made hand type
		let binary = new Uint8Array(21);
		binary[0] = this.type;

		// For every played card
		for (let iPlayed = 0; iPlayed < 5; iPlayed++) {
			let iBinaryAtCard = 1 + (iPlayed * 4);

			// Add this card
			binary[iBinaryAtCard] = this.played[iPlayed].uint8();

			// Add all three alternates
			let alt = alternates.has(this.played[iPlayed].uint8()) ? alternates.get(this.played[iPlayed].uint8()) : [];
			for (let iAlt = 0; iAlt < 3; iAlt++) {
				binary[iBinaryAtCard + 1 + iAlt] = iAlt >= alt.length ? 0 : alt[iAlt].uint8();
			}
		}

		return binary;
	}

	static flattenArrayOfArrayOfCards(cards) {
		return cards.reduce((a,b) => a.concat(b), []);
	}
}
