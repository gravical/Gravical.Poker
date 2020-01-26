import { Enums } from "./Enums.mjs";

const CardShortPattern = /^([TJQKA0-9]0?)([hcds])$/i;
const CardLongPattern = /^(Two|Three|Four|Five|Six|Seven|Eight|Nine|Ten|Jack|Queen|King|Ace)\s+of\s+(Hearts*|Diamonds*|Clubs*|Spades*)$/i;

export class Card {

	constructor() {
		this.face = 0;
		this.suit = 0;
	}

	isValid() {
		return Enums.Face.isValid(this.face) && Enums.Suit.isValid(this.suit);
	}

	uint8() {
		if (!this.isValid()) {
			return 0;
		} else {
			return this.suit | (this.face << 4);
		}
	}

	equals(other) {
		if (!other || !(other instanceof Card)) {
			return false;
		} else {
			return this.face === other.face && this.suit === other.suit;
		}
	}

	toString(format = 's') {
		if (!this.isValid()) {
			return '';
		}

		switch (format.toLowerCase()) {
			case "l":
				return `${Enums.Face.toString(this.face, 'l')} of ${Enums.Suit.toString(this.suit, 'l')}s`;
			default:
				return `${Enums.Face.toString(this.face)}${Enums.Suit.toString(this.suit)}`;
		}
	}

	static toCardsString(cards) {
		if (!cards || !(cards instanceof Array)) return '';
		return cards.map(_ => _.toString()).join(' ');
	}

	static parseShortArray(from) {
		let split = from.split(/\s+/);
		let cards = [];
		for (const text of split) {
			if (text.length == 0) continue;
			let card = Card.parse(text);
			if (card == null) throw `Invalid card string: "${text}"`;
			cards.push(card);
		}
		return cards;
	}

	static parse(from) {

		if (!isNaN(from)) {

			let card = new Card();
			card.face = ((from >> 4) & 0xF);
			card.suit = (from & 0xF);

			if (Enums.Face.isValid(card.face) && Enums.Suit.isValid(card.suit)) {
				return card;
			}

		} else if (typeof from === 'string') {
			let match = CardShortPattern.exec(from);
			if (!match) match = CardLongPattern.exec(from);
			if (!match) return null;

			let card = new Card();
			card.face = Enums.Face.parse(match[1]);
			card.suit = Enums.Suit.parse(match[2]);

			if (Enums.Face.isValid(card.face) && Enums.Suit.isValid(card.suit)) {
				return card;
			}
		}

		return null;
	}

	static sort(cards, direction) {
		if (!cards) throw "Cards is null";
		if (!(cards instanceof Array)) throw "Cards is not an Array";
		cards = cards.slice(); // clone
		if (direction === "asc") {
			cards.sort((a, b) => {
				if(a.face > b.face) return 1;
				if(a.face < b.face) return -1;
				if(a.suit > b.suit) return 1;
				if(a.suit < b.suit) return -1;
				return 0;
			});
		} else if (direction === "desc") {
			cards.sort((a, b) => {
				if(a.face > b.face) return -1;
				if(a.face < b.face) return 1;
				if(a.suit > b.suit) return -1;
				if(a.suit < b.suit) return 1;
				return 0;
			});
		} else {
			throw "Must specify a direction when sorting cards: asc or desc";
		}
		return cards;
	}
}
