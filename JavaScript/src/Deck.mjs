import { Card } from './Card.mjs';
import { Random } from './Random.mjs';

// A deck creates a shuffled stack of 52 standard cards matching a normal deck.
export class Deck {

	constructor(binary = null) {

		this.cards = [];

		if (binary && !(binary instanceof Uint8Array)) {
			throw "Can't construct deck from an unknown type";
		}

		// Construct from existing binary array
		if (binary) {
			for (let i = 0; i < binary.length; i++) {
				let card = Card.parse(binary[i]);
				if (!card) throw 'Invalid binary card: 0x' + ('00' + binary[i].toString(0x10)).substr(-2).toUpperCase();
				this.cards.push(card);
			}
			return;
		}

		// Build up a standard deck
		for (let iSuit = 1; iSuit <= 4; iSuit++) {
			for (let iFace = 2; iFace <= 14; iFace++) {
				this.cards.push(Card.parse((iFace << 4) | iSuit));
			}
		}

		// Shuffle the deck
		let shuffleFrom = Random.getUint32Array(52);
		let shuffleTo = Random.getUint32Array(52);
		for (let i = 0; i < shuffleFrom.length; i++) {
			let from = shuffleFrom[i] % 52;
			let to = shuffleTo[i] % 52;
			let swap = this.cards[to];
			this.cards[to] = this.cards[from];
			this.cards[from] = swap;
		}
	}

	size() {
		return this.cards.length;
	}

	getCards() {
		return this.cards.slice();
	}

	drawCard() {
		if (this.size() <= 0)
			throw "Cannot draw a card because the deck is empty";
		else
			return this.cards.pop();
	}

	toString() {
		return `${this.size()} cards`;
	}

	toBinary() {
		let binary = new Uint8Array(this.cards.length);
		for (let i = 0; i < this.cards.length; i++) {
			binary[i] = this.cards[i].uint8();
		}
		return binary;
	}
}
