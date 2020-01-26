import { Card } from './Card.mjs';
import { Pocket } from './Pocket.mjs';

// Holds all community cards from a Texas Hold-em table.
export class TableFinal {

	constructor() {
		this.first = null;
		this.second = null;
		this.third = null;
		this.turn = null;
		this.river = null;
	}

	static make(first, second, third, turn, river) {
		let table = new TableFinal();
		table.first = first;
		table.second = second;
		table.third = third;
		table.turn = turn;
		table.river = river;
		return table;
	}

	toArray() {
		return [ this.first, this.second, this.third, this.turn, this.river ];
	}

	getAllCards(pocket) {
		if (!pocket || !(pocket instanceof Pocket)) throw "Pocket isn't valid";
		return [
			this.first,
			this.second,
			this.third,
			this.turn,
			this.river,
			pocket.a,
			pocket.b,
		];
	}

	contains(card) {
		if (!card || !(card instanceof Card)) return false;
		return card.equals(this.first)
				|| card.equals(this.second)
				|| card.equals(this.third)
				|| card.equals(this.turn)
				|| card.equals(this.river);
	}

	toString() {
		return `${this.first} ${this.second} ${this.third} ${this.turn} ${this.river}`;
	}
}
