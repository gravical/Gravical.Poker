import { strict as assert } from 'assert';
import { Tests } from './TestFixture.mjs';
import { Enums } from '../src/Enums.mjs';
import { Card } from '../src/Card.mjs';
import { Pocket } from '../src/Pocket.mjs';
import { Deck } from '../src/Deck.mjs';
import { TableFinal } from '../src/TableFinal.mjs';

function simple() {
	return TableFinal.make(
		Card.parse('2h'),
		Card.parse('3c'),
		Card.parse('4d'),
		Card.parse('5s'),
		Card.parse('6h'));
}

Tests.add('TableFinal.constructor() should set all cards null', () => {
	let actual = new TableFinal();
	assert.ok(!actual.first);
	assert.ok(!actual.second);
	assert.ok(!actual.third);
	assert.ok(!actual.turn);
	assert.ok(!actual.river);
});

Tests.add('TableFinal.make() should set correct cards', () => {
	assert.equal(Card.parse('2h').uint8(), simple().first.uint8());
	assert.equal(Card.parse('3c').uint8(), simple().second.uint8());
	assert.equal(Card.parse('4d').uint8(), simple().third.uint8());
	assert.equal(Card.parse('5s').uint8(), simple().turn.uint8());
	assert.equal(Card.parse('6h').uint8(), simple().river.uint8());
});

Tests.add('TableFinal.toArray() null should return all null', () => {
	let actual = new TableFinal().toArray();
	assert.equal(5, actual.length);
	assert.ok(!actual[0]);
	assert.ok(!actual[1]);
	assert.ok(!actual[2]);
	assert.ok(!actual[3]);
	assert.ok(!actual[4]);
});

Tests.add('TableFinal.toArray() should return in order', () => {
	let actual = simple().toArray();
	assert.equal(5, actual.length);
	assert.equal(Card.parse('2h').uint8(), actual[0].uint8());
	assert.equal(Card.parse('3c').uint8(), actual[1].uint8());
	assert.equal(Card.parse('4d').uint8(), actual[2].uint8());
	assert.equal(Card.parse('5s').uint8(), actual[3].uint8());
	assert.equal(Card.parse('6h').uint8(), actual[4].uint8());
});

Tests.add('TableFinal.getAllCards() invalid pocket should throw', () => {
	assert.throws(() => simple().getAllCards(null), /^Pocket isn't valid$/);
	assert.throws(() => simple().getAllCards(new Object()), /^Pocket isn't valid$/);
	assert.throws(() => simple().getAllCards('example'), /^Pocket isn't valid$/);
	assert.throws(() => simple().getAllCards(123), /^Pocket isn't valid$/);
});

Tests.add('TableFinal.getAllCards() should return seven cards', () => {
	let arranged = new Pocket(new Deck());
	let actual = simple().getAllCards(arranged);
	assert.equal(7, actual.length);
	assert.equal(Card.parse('2h').uint8(), actual[0].uint8());
	assert.equal(Card.parse('2h').uint8(), actual[0].uint8());
	assert.equal(Card.parse('3c').uint8(), actual[1].uint8());
	assert.equal(Card.parse('4d').uint8(), actual[2].uint8());
	assert.equal(Card.parse('5s').uint8(), actual[3].uint8());
	assert.equal(Card.parse('6h').uint8(), actual[4].uint8());
	assert.equal(arranged.a.uint8(), actual[5].uint8());
	assert.equal(arranged.b.uint8(), actual[6].uint8());
});

Tests.add('TableFinal.contains() not a card should return false', () => {
	assert.ok(!simple().contains(null));
	assert.ok(!simple().contains(new Object()));
	assert.ok(!simple().contains('example'));
	assert.ok(!simple().contains(123));
});

Tests.add('TableFinal.contains() does not should return false', () => {
	assert.ok(!simple().contains(Card.parse('Ah')));
});

Tests.add('TableFinal.contains() match first should return true', () => {
	assert.ok(simple().contains(Card.parse('2h')));
});

Tests.add('TableFinal.contains() match second should return true', () => {
	assert.ok(simple().contains(Card.parse('3c')));
});

Tests.add('TableFinal.contains() match third should return true', () => {
	assert.ok(simple().contains(Card.parse('4d')));
});

Tests.add('TableFinal.contains() match turn should return true', () => {
	assert.ok(simple().contains(Card.parse('5s')));
});

Tests.add('TableFinal.contains() match river should return true', () => {
	assert.ok(simple().contains(Card.parse('6h')));
});

Tests.add('TableFinal.toString() null should return nulls', () => {
	assert.equal("null null null null null", new TableFinal().toString());
});

Tests.add('TableFinal.toString() should return correct string', () => {
	assert.equal("2h 3c 4d 5s 6h", simple().toString());
});
