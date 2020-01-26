import { strict as assert } from 'assert';
import { Tests } from './TestFixture.mjs';
import { Enums } from '../src/Enums.mjs';
import { Card } from '../src/Card.mjs';
import { HandGrouping } from '../src/HandGrouping.mjs';

let simpleCards = [
	Card.parse('Ah'),
	Card.parse('Kd'),
	Card.parse('Qc'),
	Card.parse('Js'),
	Card.parse('Th'),
	Card.parse('9d'),
	Card.parse('8c'),
	Card.parse('7s'),
];

Tests.add('HandGrouping.constructor() null should throw', () => {
	assert.throws(() => new HandGrouping(null), /^Cards is null$/);
});

Tests.add('HandGrouping.constructor() empty should throw', () => {
	assert.throws(() => new HandGrouping([]), /^Only 5-7 cards can be grouped$/);
});

Tests.add('HandGrouping.constructor() too few cards should throw', () => {
	assert.throws(() => new HandGrouping(simpleCards.slice(0, 4)), /^Only 5-7 cards can be grouped$/);
});

Tests.add('HandGrouping.constructor() too many cards should throw', () => {
	assert.throws(() => new HandGrouping(simpleCards), /^Only 5-7 cards can be grouped$/);
});

Tests.add('HandGrouping.constructor() duplicate card should throw', () => {
	let arrange = simpleCards.slice(0, 5);
	arrange[1] = arrange[0];
	assert.throws(() => new HandGrouping(arrange), /^Duplicate cards cannot be grouped$/);
});

Tests.add('HandGrouping.constructor() invalid card should throw', () => {
	let arrange = simpleCards.slice(0, 5);
	arrange[1] = new Card();
	assert.throws(() => new HandGrouping(arrange), /^Invalid cards cannot be grouped$/);
});

Tests.add('HandGrouping.constructor() four of a kind should return it', () => {
	let actual = new HandGrouping([
		Card.parse('Ah'),
		Card.parse('Ad'),
		Card.parse('Ac'),
		Card.parse('As'),
		Card.parse('Kh'),
		Card.parse('Qh'),
		Card.parse('Jh'),
	]);
	assert.equal(3, actual.single.length);
	assert.equal(0, actual.pair.length);
	assert.equal(0, actual.threeOfAKind.length);
	assert.equal(1, actual.fourOfAKind.length);
	assert.equal(4, actual.fourOfAKind[0].length);
	assert.equal("Ah", actual.fourOfAKind[0][0].toString());
	assert.equal("Ad", actual.fourOfAKind[0][1].toString());
	assert.equal("Ac", actual.fourOfAKind[0][2].toString());
	assert.equal("As", actual.fourOfAKind[0][3].toString());
});

Tests.add('HandGrouping.constructor() three of a kind should return it', () => {
	let actual = new HandGrouping([
		Card.parse('Ah'),
		Card.parse('Ad'),
		Card.parse('Ac'),
		Card.parse('Kh'),
		Card.parse('Qh'),
		Card.parse('Jh'),
		Card.parse('Th'),
	]);
	assert.equal(4, actual.single.length);
	assert.equal(0, actual.pair.length);
	assert.equal(1, actual.threeOfAKind.length);
	assert.equal("Ah", actual.threeOfAKind[0][0].toString());
	assert.equal("Ad", actual.threeOfAKind[0][1].toString());
	assert.equal("Ac", actual.threeOfAKind[0][2].toString());
	assert.equal(0, actual.fourOfAKind.length);
});

Tests.add('HandGrouping.constructor() multiple three of a kind should return in correct order', () => {
	let actual = new HandGrouping([
		Card.parse('Ah'),
		Card.parse('Ad'),
		Card.parse('Ac'),
		Card.parse('Kh'),
		Card.parse('Kd'),
		Card.parse('Kc'),
		Card.parse('Th'),
	]);
	assert.equal(1, actual.single.length);
	assert.equal(0, actual.pair.length);
	assert.equal(2, actual.threeOfAKind.length);
	assert.equal(3, actual.threeOfAKind[0].length);
	assert.equal("Ah", actual.threeOfAKind[0][0].toString());
	assert.equal("Ad", actual.threeOfAKind[0][1].toString());
	assert.equal("Ac", actual.threeOfAKind[0][2].toString());
	assert.equal(3, actual.threeOfAKind[1].length);
	assert.equal("Kh", actual.threeOfAKind[1][0].toString());
	assert.equal("Kd", actual.threeOfAKind[1][1].toString());
	assert.equal("Kc", actual.threeOfAKind[1][2].toString());
	assert.equal(0, actual.fourOfAKind.length);
});

Tests.add('HandGrouping.constructor() pair should return it', () => {
	let actual = new HandGrouping([
		Card.parse('Ah'),
		Card.parse('Ad'),
		Card.parse('Qc'),
		Card.parse('Js'),
		Card.parse('Th'),
		Card.parse('9d'),
		Card.parse('8c'),
	]);
	assert.equal(5, actual.single.length);
	assert.equal(1, actual.single[0].length);
	assert.equal(1, actual.single[1].length);
	assert.equal(1, actual.single[2].length);
	assert.equal(1, actual.single[3].length);
	assert.equal(1, actual.single[4].length);
	assert.equal("Qc", actual.single[0][0].toString());
	assert.equal("Js", actual.single[1][0].toString());
	assert.equal(1, actual.pair.length);
	assert.equal(2, actual.pair[0].length);
	assert.equal("Ah", actual.pair[0][0].toString());
	assert.equal("Ad", actual.pair[0][1].toString());
	assert.equal(0, actual.threeOfAKind.length);
	assert.equal(0, actual.fourOfAKind.length);
});

Tests.add('HandGrouping.constructor() multiple pair should return in correct order', () => {
	let actual = new HandGrouping([
		Card.parse('Ah'),
		Card.parse('Ad'),
		Card.parse('Kh'),
		Card.parse('Kd'),
		Card.parse('Qh'),
		Card.parse('Qd'),
		Card.parse('Jh'),
	]);
	assert.equal(1, actual.single.length);
	assert.equal(1, actual.single[0].length);
	assert.equal("Jh", actual.single[0][0].toString());
	assert.equal(3, actual.pair.length);
	assert.equal(2, actual.pair[0].length);
	assert.equal("Ah", actual.pair[0][0].toString());
	assert.equal("Ad", actual.pair[0][1].toString());
	assert.equal(2, actual.pair[1].length);
	assert.equal("Kh", actual.pair[1][0].toString());
	assert.equal("Kd", actual.pair[1][1].toString());
	assert.equal(2, actual.pair[2].length);
	assert.equal("Qh", actual.pair[2][0].toString());
	assert.equal("Qd", actual.pair[2][1].toString());
	assert.equal(0, actual.threeOfAKind.length);
	assert.equal(0, actual.fourOfAKind.length);
});

Tests.add('HandGrouping.constructor() singles should return in correct order', () => {
	let actual = new HandGrouping(simpleCards.slice(0, 7));
	assert.equal(7, actual.single.length);
	assert.equal(1, actual.single[0].length);
	assert.equal(1, actual.single[1].length);
	assert.equal(1, actual.single[2].length);
	assert.equal(1, actual.single[3].length);
	assert.equal(1, actual.single[4].length);
	assert.equal(1, actual.single[5].length);
	assert.equal(1, actual.single[6].length);
	assert.equal("Ah", actual.single[0][0].toString());
	assert.equal("Kd", actual.single[1][0].toString());
	assert.equal("Qc", actual.single[2][0].toString());
	assert.equal("Js", actual.single[3][0].toString());
	assert.equal("Th", actual.single[4][0].toString());
	assert.equal("9d", actual.single[5][0].toString());
	assert.equal("8c", actual.single[6][0].toString());
	assert.equal(0, actual.pair.length);
	assert.equal(0, actual.threeOfAKind.length);
	assert.equal(0, actual.fourOfAKind.length);
});

Tests.add('HandGrouping.constructor() four of a kind with high cards should return correctly', () => {
	let actual = new HandGrouping([
		Card.parse('Ah'),
		Card.parse('Ad'),
		Card.parse('Ac'),
		Card.parse('As'),
		Card.parse('Kh'),
		Card.parse('Qh'),
		Card.parse('Jh'),
	]);
	assert.equal(3, actual.single.length);
	assert.equal(1, actual.single[0].length);
	assert.equal(1, actual.single[1].length);
	assert.equal(1, actual.single[2].length);
	assert.equal("Kh", actual.single[0][0].toString());
	assert.equal("Qh", actual.single[1][0].toString());
	assert.equal("Jh", actual.single[2][0].toString());
	assert.equal(0, actual.pair.length);
	assert.equal(0, actual.threeOfAKind.length);
	assert.equal(1, actual.fourOfAKind.length);
	assert.equal(4, actual.fourOfAKind[0].length);
	assert.equal("Ah", actual.fourOfAKind[0][0].toString());
	assert.equal("Ad", actual.fourOfAKind[0][1].toString());
	assert.equal("Ac", actual.fourOfAKind[0][2].toString());
	assert.equal("As", actual.fourOfAKind[0][3].toString());
});

Tests.add('HandGrouping.constructor() full house with high cards should return correctly', () => {
	let actual = new HandGrouping([
		Card.parse('Ah'),
		Card.parse('Ad'),
		Card.parse('Ac'),
		Card.parse('Kh'),
		Card.parse('Kd'),
		Card.parse('Qh'),
		Card.parse('Jh'),
	]);
	assert.equal(2, actual.single.length);
	assert.equal(1, actual.single[0].length);
	assert.equal(1, actual.single[1].length);
	assert.equal("Qh", actual.single[0][0].toString());
	assert.equal("Jh", actual.single[1][0].toString());
	assert.equal(1, actual.pair.length);
	assert.equal(2, actual.pair[0].length);
	assert.equal("Kh", actual.pair[0][0].toString());
	assert.equal("Kd", actual.pair[0][1].toString());
	assert.equal(1, actual.threeOfAKind.length);
	assert.equal(3, actual.threeOfAKind[0].length);
	assert.equal("Ah", actual.threeOfAKind[0][0].toString());
	assert.equal("Ad", actual.threeOfAKind[0][1].toString());
	assert.equal("Ac", actual.threeOfAKind[0][2].toString());
	assert.equal(0, actual.fourOfAKind.length);
});

Tests.add('HandGrouping.constructor() three of a kind with high cards should return correctly', () => {
	let actual = new HandGrouping([
		Card.parse('Ah'),
		Card.parse('Ad'),
		Card.parse('Ac'),
		Card.parse('Kh'),
		Card.parse('Qh'),
		Card.parse('Jh'),
		Card.parse('Th'),
	]);
	assert.equal(4, actual.single.length);
	assert.equal(1, actual.single[0].length);
	assert.equal(1, actual.single[1].length);
	assert.equal(1, actual.single[2].length);
	assert.equal(1, actual.single[3].length);
	assert.equal("Kh", actual.single[0][0].toString());
	assert.equal("Qh", actual.single[1][0].toString());
	assert.equal("Jh", actual.single[2][0].toString());
	assert.equal("Th", actual.single[3][0].toString());
	assert.equal(0, actual.pair.length);
	assert.equal(1, actual.threeOfAKind.length);
	assert.equal(3, actual.threeOfAKind[0].length);
	assert.equal("Ah", actual.threeOfAKind[0][0].toString());
	assert.equal("Ad", actual.threeOfAKind[0][1].toString());
	assert.equal("Ac", actual.threeOfAKind[0][2].toString());
	assert.equal(0, actual.fourOfAKind.length);
});

Tests.add('HandGrouping.constructor() two pair with high cards should return correctly', () => {
	let actual = new HandGrouping([
		Card.parse('Ah'),
		Card.parse('Ad'),
		Card.parse('Kh'),
		Card.parse('Kd'),
		Card.parse('Qh'),
		Card.parse('Jh'),
		Card.parse('Th'),
	]);
	assert.equal(3, actual.single.length);
	assert.equal(1, actual.single[0].length);
	assert.equal(1, actual.single[1].length);
	assert.equal(1, actual.single[2].length);
	assert.equal("Qh", actual.single[0][0].toString());
	assert.equal("Jh", actual.single[1][0].toString());
	assert.equal("Th", actual.single[2][0].toString());
	assert.equal(2, actual.pair.length);
	assert.equal(2, actual.pair[0].length);
	assert.equal("Ah", actual.pair[0][0].toString());
	assert.equal("Ad", actual.pair[0][1].toString());
	assert.equal(2, actual.pair[1].length);
	assert.equal("Kh", actual.pair[1][0].toString());
	assert.equal("Kd", actual.pair[1][1].toString());
	assert.equal(0, actual.threeOfAKind.length);
	assert.equal(0, actual.fourOfAKind.length);
});
