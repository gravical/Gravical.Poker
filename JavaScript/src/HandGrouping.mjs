import { Card } from './Card.mjs';
import { Random } from './Random.mjs';

// HandGrouping builds list of multiple cards, pair, three of a kind, and four of a kind.
export class HandGrouping {

	constructor(cards)
	{
		if (!cards) throw "Cards is null";
		if (!(cards instanceof Array)) throw "Cards must be an array of cards";
		if (cards.length < 5 || cards.length > 7) throw "Only 5-7 cards can be grouped";

		this.fourOfAKind = [];
		this.threeOfAKind = [];
		this.pair = [];
		this.single = [];

		let groups = new Map();
		let unique = new Map();
		for (const card of cards) {
			if (!(card instanceof Card))
				throw "One or more initializers isn't a card"
			if (!card.isValid())
				throw "Invalid cards cannot be grouped";
			if (unique.has(card.toString()))
				throw "Duplicate cards cannot be grouped";

			unique.set(card.toString(), card);

			if (!groups.has(card.face))
				groups.set(card.face, []);

			groups.get(card.face).push(card);
		}

		for (const [face, list] of groups.entries()) {
			if (list.length == 4)
				this.fourOfAKind.push(list);
			else if (list.length == 3)
				this.threeOfAKind.push(list);
			else if (list.length == 2)
				this.pair.push(list);
			else if (list.length == 1)
				this.single.push(list);
			else
				throw "Impossible grouping";
		}
	}
}
