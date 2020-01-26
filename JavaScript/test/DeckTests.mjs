import { strict as assert } from 'assert';
import { Tests } from './TestFixture.mjs';
import { Enums } from '../src/Enums.mjs';
import { Random } from '../src/Random.mjs';
import { Deck } from '../src/Deck.mjs';

Tests.add('Deck.constructor() initializer invalid should throw', () => {
	assert.throws(() => new Deck(new Object()), /^Can't construct deck from an unknown type$/);
	assert.throws(() => new Deck('value'), /^Can't construct deck from an unknown type$/);
	assert.throws(() => new Deck(123), /^Can't construct deck from an unknown type$/);
	assert.throws(() => new Deck(new Uint32Array()), /^Can't construct deck from an unknown type$/);
});

function assertCardIfNot(next, drawn, actual, expected) {
	if (drawn.includes(expected))
		return next;
	assert.equal(expected, actual[next]);
	return next + 1;
}

function assertValidFullDeck(deck, drawn = []) {
	let actual = deck.toBinary();
	actual.sort();
	assert.equal(52 - drawn.length, actual.length);
	let next = 0;
	next = assertCardIfNot(next, drawn, actual, 0x21);
	next = assertCardIfNot(next, drawn, actual, 0x22);
	next = assertCardIfNot(next, drawn, actual, 0x23);
	next = assertCardIfNot(next, drawn, actual, 0x24);
	next = assertCardIfNot(next, drawn, actual, 0x31);
	next = assertCardIfNot(next, drawn, actual, 0x32);
	next = assertCardIfNot(next, drawn, actual, 0x33);
	next = assertCardIfNot(next, drawn, actual, 0x34);
	next = assertCardIfNot(next, drawn, actual, 0x41);
	next = assertCardIfNot(next, drawn, actual, 0x42);
	next = assertCardIfNot(next, drawn, actual, 0x43);
	next = assertCardIfNot(next, drawn, actual, 0x44);
	next = assertCardIfNot(next, drawn, actual, 0x51);
	next = assertCardIfNot(next, drawn, actual, 0x52);
	next = assertCardIfNot(next, drawn, actual, 0x53);
	next = assertCardIfNot(next, drawn, actual, 0x54);
	next = assertCardIfNot(next, drawn, actual, 0x61);
	next = assertCardIfNot(next, drawn, actual, 0x62);
	next = assertCardIfNot(next, drawn, actual, 0x63);
	next = assertCardIfNot(next, drawn, actual, 0x64);
	next = assertCardIfNot(next, drawn, actual, 0x71);
	next = assertCardIfNot(next, drawn, actual, 0x72);
	next = assertCardIfNot(next, drawn, actual, 0x73);
	next = assertCardIfNot(next, drawn, actual, 0x74);
	next = assertCardIfNot(next, drawn, actual, 0x81);
	next = assertCardIfNot(next, drawn, actual, 0x82);
	next = assertCardIfNot(next, drawn, actual, 0x83);
	next = assertCardIfNot(next, drawn, actual, 0x84);
	next = assertCardIfNot(next, drawn, actual, 0x91);
	next = assertCardIfNot(next, drawn, actual, 0x92);
	next = assertCardIfNot(next, drawn, actual, 0x93);
	next = assertCardIfNot(next, drawn, actual, 0x94);
	next = assertCardIfNot(next, drawn, actual, 0xA1);
	next = assertCardIfNot(next, drawn, actual, 0xA2);
	next = assertCardIfNot(next, drawn, actual, 0xA3);
	next = assertCardIfNot(next, drawn, actual, 0xA4);
	next = assertCardIfNot(next, drawn, actual, 0xB1);
	next = assertCardIfNot(next, drawn, actual, 0xB2);
	next = assertCardIfNot(next, drawn, actual, 0xB3);
	next = assertCardIfNot(next, drawn, actual, 0xB4);
	next = assertCardIfNot(next, drawn, actual, 0xC1);
	next = assertCardIfNot(next, drawn, actual, 0xC2);
	next = assertCardIfNot(next, drawn, actual, 0xC3);
	next = assertCardIfNot(next, drawn, actual, 0xC4);
	next = assertCardIfNot(next, drawn, actual, 0xD1);
	next = assertCardIfNot(next, drawn, actual, 0xD2);
	next = assertCardIfNot(next, drawn, actual, 0xD3);
	next = assertCardIfNot(next, drawn, actual, 0xD4);
	next = assertCardIfNot(next, drawn, actual, 0xE1);
	next = assertCardIfNot(next, drawn, actual, 0xE2);
	next = assertCardIfNot(next, drawn, actual, 0xE3);
	next = assertCardIfNot(next, drawn, actual, 0xE4);
};

Tests.add('Deck.constructor() null initializer should construct standard deck', () => {
	assertValidFullDeck(new Deck(null));
});

Tests.add('Deck.constructor() no initializer should construct standard deck', () => {
	assertValidFullDeck(new Deck());
});

Tests.add('Deck.constructor() no initializer should shuffle deck', () => {
	for (let i = 0; i < 5; i++) {
		let deck = new Deck();
		let actual = deck.toBinary();
		if (0x21 != actual[0] &&
			0x31 != actual[4] &&
			0x41 != actual[8] &&
			0x51 != actual[12] &&
			0x61 != actual[16] &&
			0x71 != actual[20] &&
			0x81 != actual[24] &&
			0x91 != actual[28] &&
			0xA1 != actual[32] &&
			0xB1 != actual[36] &&
			0xC1 != actual[40] &&
			0xD1 != actual[44] &&
			0xE1 != actual[48]) {
			return;
		}
	}
	assert.fail("Five times selected cards weren't shuffled");
});

Tests.add('Deck.constructor() binary initializer with invalid cards should throw', () => {
	assert.throws(() => new Deck(new Uint8Array([0x21, 0x22, 0x23, 0x24, 0x25])), /^Invalid binary card: 0x25$/);
	assert.throws(() => new Deck(new Uint8Array([0x11])), /^Invalid binary card: 0x11$/);
	assert.throws(() => new Deck(new Uint8Array([0xE1, 0xE2, 0xE3, 0xE4, 0xE5])), /^Invalid binary card: 0xE5$/);
	assert.throws(() => new Deck(new Uint8Array([0xF0])), /^Invalid binary card: 0xF0$/);
});

Tests.add('Deck.constructor() binary initializer with partial deck should construct', () => {
	let arranged = new Uint8Array([ 0x21, 0x31, 0x41, 0x51, 0x61, 0x71, 0x81, 0x91, 0xA1, 0xB1, 0xC1, 0xD1, 0xE1, ]);
	let deck = new Deck(arranged);
	let actual = deck.getCards();
	assert.equal(actual[0].toString(), '2h');
	assert.equal(actual[1].toString(), '3h');
	assert.equal(actual[2].toString(), '4h');
	assert.equal(actual[3].toString(), '5h');
	assert.equal(actual[4].toString(), '6h');
	assert.equal(actual[5].toString(), '7h');
	assert.equal(actual[6].toString(), '8h');
	assert.equal(actual[7].toString(), '9h');
	assert.equal(actual[8].toString(), 'Th');
	assert.equal(actual[9].toString(), 'Jh');
	assert.equal(actual[10].toString(), 'Qh');
	assert.equal(actual[11].toString(), 'Kh');
	assert.equal(actual[12].toString(), 'Ah');
});

Tests.add('Deck.constructor() binary initializer with standard deck should construct', () => {
	let arranged = new Uint8Array([
		0x21, 0x22, 0x23, 0x24, 0x31, 0x32, 0x33, 0x34, 0x41, 0x42, 0x43, 0x44,
		0x51, 0x52, 0x53, 0x54, 0x61, 0x62, 0x63, 0x64, 0x71, 0x72, 0x73, 0x74,
		0x81, 0x82, 0x83, 0x84, 0x91, 0x92, 0x93, 0x94, 0xA1, 0xA2, 0xA3, 0xA4,
		0xB1, 0xB2, 0xB3, 0xB4, 0xC1, 0xC2, 0xC3, 0xC4, 0xD1, 0xD2, 0xD3, 0xD4,
		0xE1, 0xE2, 0xE3, 0xE4, ]);
	assertValidFullDeck(new Deck(arranged));
});

Tests.add('Deck.size() should return correct size', () => {
	assert.equal(new Deck().size(), 52);
	assert.equal(new Deck(new Uint8Array([0x21, 0x22, 0x23, 0x24, ])).size(), 4);
	assert.equal(new Deck(new Uint8Array(0)).size(), 0);
});

Tests.add('Deck.getCards() should return copy of deck', () => {
	let deck = new Deck();
	let cards = deck.getCards();
	cards.splice(1, 1);
	assert.equal(new Deck().getCards().length, 52);
	assert.equal(cards.length, 51);
});

Tests.add('Deck.getCards() should return deck', () => {
	let deck = new Deck(new Uint8Array([0x21, 0x22, 0x23, 0x24 ]));
	let cards = deck.getCards();
	assert.equal(cards[0].uint8(), 0x21);
	assert.equal(cards[1].uint8(), 0x22);
	assert.equal(cards[2].uint8(), 0x23);
	assert.equal(cards[3].uint8(), 0x24);
});

Tests.add('Deck.drawCard() should pull next card', () => {
	let deck = new Deck();

	// First draw
	let cards1 = deck.getCards();
	let popped1 = deck.drawCard();
	assert.equal(deck.getCards().length, 51);
	assert.equal(cards1[51].uint8(), popped1.uint8());
	assertValidFullDeck(deck, [popped1.uint8()]);

	// Second draw
	let cards2 = deck.getCards();
	let popped2 = deck.drawCard();
	assert.equal(deck.getCards().length, 50);
	assert.equal(cards2[50].uint8(), popped2.uint8());
	assertValidFullDeck(deck, [popped1.uint8(), popped2.uint8()]);
});

Tests.add('Deck.toString() should return string with size', () => {
	assert.equal(new Deck().toString(), '52 cards');
	assert.equal(new Deck(new Uint8Array([0x21, 0x22, 0x23, 0x24, ])).toString(), '4 cards');
	assert.equal(new Deck(new Uint8Array(0)).toString(), '0 cards');
});

Tests.add('Deck.toBinary() full standard deck should return exact array, shuffled', () => {
	assertValidFullDeck(new Deck(new Deck().toBinary()));
});

Tests.add('Deck.toBinary() given deck should return exact array', () => {
	let  deck = new Deck(new Uint8Array([0x21, 0x31, 0x41, 0x51, 0x61, 0x71, 0x81, 0x91, 0xA1, 0xB1, 0xC1, 0xD1, 0xE1 ]));
	let actual = deck.toBinary();
	assert.equal(0x21, actual[0]);
	assert.equal(0x31, actual[1]);
	assert.equal(0x41, actual[2]);
	assert.equal(0x51, actual[3]);
	assert.equal(0x61, actual[4]);
	assert.equal(0x71, actual[5]);
	assert.equal(0x81, actual[6]);
	assert.equal(0x91, actual[7]);
	assert.equal(0xA1, actual[8]);
	assert.equal(0xB1, actual[9]);
	assert.equal(0xC1, actual[10]);
	assert.equal(0xD1, actual[11]);
	assert.equal(0xE1, actual[12]);
});
