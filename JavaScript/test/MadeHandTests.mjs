import { strict as assert } from 'assert';
import { Tests } from './TestFixture.mjs';
import { Enums } from '../src/Enums.mjs';
import { Card } from '../src/Card.mjs';
import { MadeHand } from '../src/MadeHand.mjs';

function highHand() {
	return Card.parseShortArray("2h 3d 4c 5s 6h");
}

function createAlternates(main, parts) {
	let alternates = new Map();
	let mainCards = Card.parseShortArray(main);
	for (let i = 0; i < mainCards.length; i++) {
		alternates.set(mainCards[i].uint8(), Card.parseShortArray(parts[i]));
	}
	return alternates;
}

function createBasicAlternates() {
	return createAlternates("2h 3d 4c 5s 6h", ["2d 2c 2s", "3h 3c 3s", "4h 4d 4s", "5h 5d 5c", "6d 6c 6s"]);
}

Tests.add('MadeHand.makeFromParts() type invalid should throw', () => {
	assert.throws(() => MadeHand.makeFromParts(-1, highHand()), /^Type is not valid: -1$/);
});

Tests.add('MadeHand.makeFromParts() played null should throw', () => {
	assert.throws(() => MadeHand.makeFromParts(Enums.HandTypes.HighCard, null), /^Played is null$/);
});

Tests.add('MadeHand.makeFromParts() played too few should throw', () => {
	assert.throws(() => MadeHand.makeFromParts(Enums.HandTypes.HighCard, Card.parseShortArray("2h 3d 4c 5s")), /^Played length must be 5$/);
});

Tests.add('MadeHand.makeFromParts() played too many should throw', () => {
	assert.throws(() => MadeHand.makeFromParts(Enums.HandTypes.HighCard, Card.parseShortArray("2h 3d 4c 5s 6h 7d")), /^Played length must be 5$/);
});

Tests.add('MadeHand.makeFromParts() alternates null should succeed', () => {
	MadeHand.makeFromParts(Enums.HandTypes.HighCard, highHand(), null);
});

Tests.add('MadeHand.makeFromParts() type should set', () => {
	let actual = MadeHand.makeFromParts(Enums.HandTypes.ThreeOfAKind, highHand());
	assert.equal(Enums.HandTypes.ThreeOfAKind, actual.type);
});

Tests.add('MadeHand.makeFromParts() played should set', () => {
	let actual = MadeHand.makeFromParts(Enums.HandTypes.HighCard, highHand());
	assert.equal(Card.toCardsString(highHand()), Card.toCardsString(actual.played));
});

Tests.add('MadeHand.makeFromParts() score should be correct', () => {
	let actual = MadeHand.makeFromParts(Enums.HandTypes.HighCard, highHand());
	assert.equal(50595078, actual.score);
});

Tests.add('MadeHand.makeFromBinary() binary not Uint8Array should throw', () => {
	assert.throws(() => MadeHand.makeFromBinary(), /^Binary must be a Uint8Array$/);
	assert.throws(() => MadeHand.makeFromBinary(null), /^Binary must be a Uint8Array$/);
	assert.throws(() => MadeHand.makeFromBinary(new Object()), /^Binary must be a Uint8Array$/);
	assert.throws(() => MadeHand.makeFromBinary('example'), /^Binary must be a Uint8Array$/);
	assert.throws(() => MadeHand.makeFromBinary(123), /^Binary must be a Uint8Array$/);
});

Tests.add('MadeHand.makeFromBinary() binary too short should throw', () => {
	assert.throws(() => MadeHand.makeFromBinary(new Uint8Array(20)), /^Binary must be exactly 21 bytes$/);
});

Tests.add('MadeHand.makeFromBinary() binary too long should throw', () => {
	assert.throws(() => MadeHand.makeFromBinary(new Uint8Array(22)), /^Binary must be exactly 21 bytes$/);
});

function assertBasicBinary(actual) {
	assert.equal(actual[0], Enums.HandTypes.HighCard);
	assert.equal(actual[1], 0x21);
	assert.equal(actual[2], 0x22);
	assert.equal(actual[3], 0x23);
	assert.equal(actual[4], 0x24);
	assert.equal(actual[5], 0x32);
	assert.equal(actual[6], 0x31);
	assert.equal(actual[7], 0x33);
	assert.equal(actual[8], 0x34);
	assert.equal(actual[9], 0x43);
	assert.equal(actual[10], 0x41);
	assert.equal(actual[11], 0x42);
	assert.equal(actual[12], 0x44);
	assert.equal(actual[13], 0x54);
	assert.equal(actual[14], 0x51);
	assert.equal(actual[15], 0x52);
	assert.equal(actual[16], 0x53);
	assert.equal(actual[17], 0x61);
	assert.equal(actual[18], 0x62);
	assert.equal(actual[19], 0x63);
	assert.equal(actual[20], 0x64);
}

Tests.add('MadeHand.makeFromBinary() binary valid should construct', () => {
	let binary = new Uint8Array([
		0x01, 0x21, 0x22, 0x23, 0x24, 0x32, 0x31, 0x33, 0x34,
		0x43, 0x41, 0x42, 0x44, 0x54, 0x51, 0x52, 0x53, 0x61,
		0x62, 0x63, 0x64,]);
	let actual = MadeHand.makeFromBinary(binary);
	assertBasicBinary(actual.toBinary());
});

Tests.add('MadeHand.toString() should return known value', () => {
	let arranged = MadeHand.makeFromParts(Enums.HandTypes.HighCard, highHand(), createBasicAlternates());
	assert.equal("HighCard: 2h 3d 4c 5s 6h", arranged.toString());
});

Tests.add('MadeHand.toBinary() should return known value', () => {
	let arranged = MadeHand.makeFromParts(Enums.HandTypes.HighCard, highHand(), createBasicAlternates());
	assertBasicBinary(arranged.toBinary());
});
