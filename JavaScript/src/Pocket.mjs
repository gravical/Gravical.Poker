import { Card } from './Card.mjs';
import { Deck } from './Deck.mjs';
import { TableFinal } from './TableFinal.mjs';

// Pocket holds two distinct poker cards for a player.
export class Pocket {

	constructor(init) {

		if (init && init instanceof Deck) {
			this.a = init.drawCard();
			this.b = init.drawCard();
		} else if (init && init instanceof Uint8Array) {
			if (init.length != 2) throw "Initializer must be two bytes";
			this.a = Card.parse(init[0]);
			this.b = Card.parse(init[1]);
		} else if (!init) {
			this.a = null;
			this.b = null;
		} else {
			throw "Initializer is of an unknown type";
		}
	}

	toString() {
		return `${this.a} ${this.b}`;
	}

	toArray() {
		return [ this.a, this.b ];
	}

	isTrumped(by) {
		if (!by) {
			throw "By is null";
		} else if (by instanceof Pocket) {
			return this.a.equals(by.a) || this.a.equals(by.b) || this.b.equals(by.a) || this.b.equals(by.b);
		} else if (by instanceof TableFinal) {
			return by.contains(this.a) || by.contains(this.b);
		} else {
			throw "Unknown by";
		}
	}

	toBinary() {
		return new Uint8Array([ this.a ? this.a.uint8() : 0, this.b ? this.b.uint8() : 0]);
	}
}
