import { strict as assert } from 'assert';
import { Tests } from './TestFixture.mjs';
import { Enums } from '../src/Enums.mjs';
import { Card } from '../src/Card.mjs';
import { Deck } from '../src/Deck.mjs';
import { TableFinal } from '../src/TableFinal.mjs';
import { Pocket } from '../src/Pocket.mjs';

Tests.add('Pocket.constructor() init is Deck and not enough cards should throw', () => {
	let deck = new Deck(new Uint8Array([0x21]));
	assert.throws(() => new Pocket(deck), /^Cannot draw a card because the deck is empty$/);
});

Tests.add('Pocket.constructor() init is Deck should construct', () => {
	let actual = new Pocket(new Deck());
	assert.ok(actual.a.isValid());
	assert.ok(actual.b.isValid());
	assert.notEqual(actual.a, actual.b);
});

Tests.add('Pocket.constructor() init is binary and length is wrong should throw', () => {
	assert.throws(() => new Pocket(new Uint8Array([0x21])), /^Initializer must be two bytes$/);
	assert.throws(() => new Pocket(new Uint8Array([0x21, 0x22, 0x23])), /^Initializer must be two bytes$/);
});

Tests.add('Pocket.constructor() init is binary should construct', () => {
	let actual = new Pocket(new Uint8Array([0x21, 0x22]));
	assert.equal(actual.a.toString(), '2h');
	assert.equal(actual.b.toString(), '2d');
});

function assertPocketNullCards(actual) {
	assert.ok(!!actual);
	assert.ok(!actual.a);
	assert.ok(!actual.b);
}

Tests.add('Pocket.constructor() init null should construct', () => {
	assertPocketNullCards(new Pocket(null));
	assertPocketNullCards(new Pocket());
});

Tests.add('Pocket.constructor() init is unsupported type should throw', () => {
	assert.throws(() => new Pocket(new Object()), /^Initializer is of an unknown type$/);
	assert.throws(() => new Pocket('example'), /^Initializer is of an unknown type$/);
	assert.throws(() => new Pocket(123), /^Initializer is of an unknown type$/);
});

Tests.add('Pocket.toString() null should return correct string', () => {
	assert.equal(new Pocket().toString(), 'null null');
});

Tests.add('Pocket.toString() valid should return correct string', () => {
	assert.equal(new Pocket(new Uint8Array([0x21, 0x22])).toString(), '2h 2d');
});

Tests.add('Pocket.toArray() null should return correct values', () => {
	let actual = new Pocket().toArray();
	assert.equal(2, actual.length);
	assert.ok(!actual[0]);
	assert.ok(!actual[1]);
});

Tests.add('Pocket.toArray() valid should return correct values', () => {
	let actual = new Pocket(new Uint8Array([0x21, 0x22])).toArray();
	assert.equal(actual[0].uint8(), 0x21);
	assert.equal(actual[1].uint8(), 0x22);
});

function simple() {
	return new Pocket(new Uint8Array([0x21, 0x84]));
}

Tests.add('Pocket.isTrumpedByPocket_Null_ShouldThrowArgumentNullException', () => {
	assert.throws(() => simple().isTrumped(null), /^By is null$/);
});

Tests.add('Pocket.isTrumpedByPocket_CardAEqualsCardA_ShouldReturnTrue', () => {
	assert.ok(simple().isTrumped(new Pocket(new Uint8Array([ 0x21, 0x31 ]))));
});

Tests.add('Pocket.isTrumpedByPocket_CardAEqualsCardB_ShouldReturnTrue', () => {
	assert.ok(simple().isTrumped(new Pocket(new Uint8Array([ 0x84, 0x31 ]))));
});

Tests.add('Pocket.isTrumpedByPocket_CardBEqualsCardA_ShouldReturnTrue', () => {
	assert.ok(simple().isTrumped(new Pocket(new Uint8Array([ 0x31, 0x21 ]))));
});

Tests.add('Pocket.isTrumpedByPocket_CardBEqualsCardB_ShouldReturnTrue', () => {
	assert.ok(simple().isTrumped(new Pocket(new Uint8Array([ 0x31, 0x84 ]))));
});

Tests.add('Pocket.isTrumpedByPocket_No_ShouldReturnFalse', () => {
	assert.ok(!simple().isTrumped(new Pocket(new Uint8Array([ 0x31, 0x41 ]))));
});

function makeTable(value) {
	let cards = value.split(' ').map(_ => Card.parse(_));
	return TableFinal.make(cards[0], cards[1], cards[2], cards[3], cards[4]);
}

Tests.add('Pocket.isTrumped() by table _Null_ShouldThrowArgumentNullException', () => {
	assert.throws(() => simple().isTrumped(null), /^By is null$/);
});

Tests.add('Pocket.isTrumped() by table _CardAEqualsFirst_ShouldReturnTrue', () => {
	assert.ok(simple().isTrumped(makeTable("2h 3c 4s 5h 6d")));
});

Tests.add('Pocket.isTrumped() by table _CardAEqualsSecond_ShouldReturnTrue', () => {
	assert.ok(simple().isTrumped(makeTable("2d 2h 4s 5h 6d")));
});

Tests.add('Pocket.isTrumped() by table _CardAEqualsThird_ShouldReturnTrue', () => {
	assert.ok(simple().isTrumped(makeTable("2d 3c 2h 5h 6d")));
});

Tests.add('Pocket.isTrumped() by table _CardAEqualsTurn_ShouldReturnTrue', () => {
	assert.ok(simple().isTrumped(makeTable("2d 3c 4s 2h 6d")));
});

Tests.add('Pocket.isTrumped() by table _CardAEqualsRiver_ShouldReturnTrue', () => {
	assert.ok(simple().isTrumped(makeTable("2d 3c 4s 5h 2h")));
});

Tests.add('Pocket.isTrumped() by table _CardBEqualsFirst_ShouldReturnTrue', () => {
	assert.ok(simple().isTrumped(makeTable("8s 3c 4s 5h 6d")));
});

Tests.add('Pocket.isTrumped() by table _CardBEqualsSecond_ShouldReturnTrue', () => {
	assert.ok(simple().isTrumped(makeTable("2d 8s 4s 5h 6d")));
});

Tests.add('Pocket.isTrumped() by table _CardBEqualsThird_ShouldReturnTrue', () => {
	assert.ok(simple().isTrumped(makeTable("2d 3c 8s 5h 6d")));
});

Tests.add('Pocket.isTrumped() by table _CardBEqualsTurn_ShouldReturnTrue', () => {
	assert.ok(simple().isTrumped(makeTable("2d 3c 4s 8s 6d")));
});

Tests.add('Pocket.isTrumped() by table _CardBEqualsRiver_ShouldReturnTrue', () => {
	assert.ok(simple().isTrumped(makeTable("2d 3c 4s 5h 8s")));
});

Tests.add('Pocket.isTrumped() by table _No_ShouldReturnFalse', () => {
	assert.ok(!simple().isTrumped(makeTable("2d 3c 4s 5h 6d")));
});

Tests.add('Pocket.toBinary() null should return correct values', () => {
	let actual = new Pocket().toBinary();
	assert.equal(actual.length, 2);
	assert.ok(!actual[0]);
	assert.ok(!actual[1]);
});

Tests.add('Pocket.toBinary() valid should return correct values', () => {
	let actual = new Pocket(new Uint8Array([0x21, 0x22])).toBinary();
	assert.equal(actual.length, 2);
	assert.equal(actual[0], 0x21);
	assert.equal(actual[1], 0x22);
});
