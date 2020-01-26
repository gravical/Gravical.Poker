import { strict as assert } from 'assert';
import { Tests } from './TestFixture.mjs';
import { Enums } from '../src/Enums.mjs';

// Face

Tests.add("Enums.Face should be exact numeric values for sorting and indexing", () => {
	assert.equal(Enums.Face.Two, 2);
	assert.equal(Enums.Face.Three, 3);
	assert.equal(Enums.Face.Four, 4);
	assert.equal(Enums.Face.Five, 5);
	assert.equal(Enums.Face.Six, 6);
	assert.equal(Enums.Face.Seven, 7);
	assert.equal(Enums.Face.Eight, 8);
	assert.equal(Enums.Face.Nine, 9);
	assert.equal(Enums.Face.Ten, 10);
	assert.equal(Enums.Face.Jack, 11);
	assert.equal(Enums.Face.Queen, 12);
	assert.equal(Enums.Face.King, 13);
	assert.equal(Enums.Face.Ace, 14);
});

Tests.add("Enums.Face.isValid() should return correct bool", () => {
	assert.ok(!Enums.Face.isValid(1));
	assert.ok(Enums.Face.isValid(Enums.Face.Two));
	assert.ok(Enums.Face.isValid(Enums.Face.Three));
	assert.ok(Enums.Face.isValid(Enums.Face.Four));
	assert.ok(Enums.Face.isValid(Enums.Face.Five));
	assert.ok(Enums.Face.isValid(Enums.Face.Six));
	assert.ok(Enums.Face.isValid(Enums.Face.Seven));
	assert.ok(Enums.Face.isValid(Enums.Face.Eight));
	assert.ok(Enums.Face.isValid(Enums.Face.Nine));
	assert.ok(Enums.Face.isValid(Enums.Face.Ten));
	assert.ok(Enums.Face.isValid(Enums.Face.Jack));
	assert.ok(Enums.Face.isValid(Enums.Face.Queen));
	assert.ok(Enums.Face.isValid(Enums.Face.King));
	assert.ok(Enums.Face.isValid(Enums.Face.Ace));
	assert.ok(!Enums.Face.isValid(15));
});

Tests.add("Enums.Face.toString() should return correct short string", () => {
	assert.equal(Enums.Face.toString(Enums.Face.Two), '2');
	assert.equal(Enums.Face.toString(Enums.Face.Three), '3');
	assert.equal(Enums.Face.toString(Enums.Face.Four), '4');
	assert.equal(Enums.Face.toString(Enums.Face.Five), '5');
	assert.equal(Enums.Face.toString(Enums.Face.Six), '6');
	assert.equal(Enums.Face.toString(Enums.Face.Seven), '7');
	assert.equal(Enums.Face.toString(Enums.Face.Eight), '8');
	assert.equal(Enums.Face.toString(Enums.Face.Nine), '9');
	assert.equal(Enums.Face.toString(Enums.Face.Ten), 'T');
	assert.equal(Enums.Face.toString(Enums.Face.Jack), 'J');
	assert.equal(Enums.Face.toString(Enums.Face.Queen), 'Q');
	assert.equal(Enums.Face.toString(Enums.Face.King), 'K');
	assert.equal(Enums.Face.toString(Enums.Face.Ace), 'A');
});

Tests.add("Enums.Face.toString('l') should return correct long string", () => {
	assert.equal(Enums.Face.toString(Enums.Face.Two, 'l'), 'Two');
	assert.equal(Enums.Face.toString(Enums.Face.Three, 'l'), 'Three');
	assert.equal(Enums.Face.toString(Enums.Face.Four, 'l'), 'Four');
	assert.equal(Enums.Face.toString(Enums.Face.Five, 'l'), 'Five');
	assert.equal(Enums.Face.toString(Enums.Face.Six, 'l'), 'Six');
	assert.equal(Enums.Face.toString(Enums.Face.Seven, 'l'), 'Seven');
	assert.equal(Enums.Face.toString(Enums.Face.Eight, 'l'), 'Eight');
	assert.equal(Enums.Face.toString(Enums.Face.Nine, 'l'), 'Nine');
	assert.equal(Enums.Face.toString(Enums.Face.Ten, 'l'), 'Ten');
	assert.equal(Enums.Face.toString(Enums.Face.Jack, 'l'), 'Jack');
	assert.equal(Enums.Face.toString(Enums.Face.Queen, 'l'), 'Queen');
	assert.equal(Enums.Face.toString(Enums.Face.King, 'l'), 'King');
	assert.equal(Enums.Face.toString(Enums.Face.Ace, 'l'), 'Ace');
});

Tests.add("Enums.Face.parse() from binary should return correct value from string", () => {
	assert.equal(Enums.Face.parse(Enums.Face.Two), Enums.Face.Two);
	assert.equal(Enums.Face.parse(Enums.Face.Three), Enums.Face.Three);
	assert.equal(Enums.Face.parse(Enums.Face.Four), Enums.Face.Four);
	assert.equal(Enums.Face.parse(Enums.Face.Five), Enums.Face.Five);
	assert.equal(Enums.Face.parse(Enums.Face.Six), Enums.Face.Six);
	assert.equal(Enums.Face.parse(Enums.Face.Seven), Enums.Face.Seven);
	assert.equal(Enums.Face.parse(Enums.Face.Eight), Enums.Face.Eight);
	assert.equal(Enums.Face.parse(Enums.Face.Nine), Enums.Face.Nine);
	assert.equal(Enums.Face.parse(Enums.Face.Ten), Enums.Face.Ten);
	assert.equal(Enums.Face.parse(Enums.Face.Jack), Enums.Face.Jack);
	assert.equal(Enums.Face.parse(Enums.Face.Queen), Enums.Face.Queen);
	assert.equal(Enums.Face.parse(Enums.Face.King), Enums.Face.King);
	assert.equal(Enums.Face.parse(Enums.Face.Ace), Enums.Face.Ace);
});

Tests.add("Enums.Face.parse() from lower case should return correct value from string", () => {
	assert.equal(Enums.Face.parse('2'), Enums.Face.Two);
	assert.equal(Enums.Face.parse('two'), Enums.Face.Two);
	assert.equal(Enums.Face.parse('twos'), Enums.Face.Two);
	assert.equal(Enums.Face.parse('deuce'), Enums.Face.Two);
	assert.equal(Enums.Face.parse('deuces'), Enums.Face.Two);
	assert.equal(Enums.Face.parse('3'), Enums.Face.Three);
	assert.equal(Enums.Face.parse('three'), Enums.Face.Three);
	assert.equal(Enums.Face.parse('threes'), Enums.Face.Three);
	assert.equal(Enums.Face.parse('4'), Enums.Face.Four);
	assert.equal(Enums.Face.parse('four'), Enums.Face.Four);
	assert.equal(Enums.Face.parse('fours'), Enums.Face.Four);
	assert.equal(Enums.Face.parse('5'), Enums.Face.Five);
	assert.equal(Enums.Face.parse('five'), Enums.Face.Five);
	assert.equal(Enums.Face.parse('fives'), Enums.Face.Five);
	assert.equal(Enums.Face.parse('6'), Enums.Face.Six);
	assert.equal(Enums.Face.parse('six'), Enums.Face.Six);
	assert.equal(Enums.Face.parse('sixes'), Enums.Face.Six);
	assert.equal(Enums.Face.parse('7'), Enums.Face.Seven);
	assert.equal(Enums.Face.parse('seven'), Enums.Face.Seven);
	assert.equal(Enums.Face.parse('sevens'), Enums.Face.Seven);
	assert.equal(Enums.Face.parse('8'), Enums.Face.Eight);
	assert.equal(Enums.Face.parse('eight'), Enums.Face.Eight);
	assert.equal(Enums.Face.parse('eights'), Enums.Face.Eight);
	assert.equal(Enums.Face.parse('9'), Enums.Face.Nine);
	assert.equal(Enums.Face.parse('nine'), Enums.Face.Nine);
	assert.equal(Enums.Face.parse('nines'), Enums.Face.Nine);
	assert.equal(Enums.Face.parse('t'), Enums.Face.Ten);
	assert.equal(Enums.Face.parse('ten'), Enums.Face.Ten);
	assert.equal(Enums.Face.parse('tens'), Enums.Face.Ten);
	assert.equal(Enums.Face.parse('j'), Enums.Face.Jack);
	assert.equal(Enums.Face.parse('jack'), Enums.Face.Jack);
	assert.equal(Enums.Face.parse('jacks'), Enums.Face.Jack);
	assert.equal(Enums.Face.parse('q'), Enums.Face.Queen);
	assert.equal(Enums.Face.parse('queen'), Enums.Face.Queen);
	assert.equal(Enums.Face.parse('queens'), Enums.Face.Queen);
	assert.equal(Enums.Face.parse('k'), Enums.Face.King);
	assert.equal(Enums.Face.parse('king'), Enums.Face.King);
	assert.equal(Enums.Face.parse('kings'), Enums.Face.King);
	assert.equal(Enums.Face.parse('a'), Enums.Face.Ace);
	assert.equal(Enums.Face.parse('ace'), Enums.Face.Ace);
	assert.equal(Enums.Face.parse('aces'), Enums.Face.Ace);
});

Tests.add("Enums.Face.parse() from upper case should return correct value from string", () => {
	assert.equal(Enums.Face.parse('TWO'), Enums.Face.Two);
	assert.equal(Enums.Face.parse('TWOS'), Enums.Face.Two);
	assert.equal(Enums.Face.parse('DEUCE'), Enums.Face.Two);
	assert.equal(Enums.Face.parse('DEUCES'), Enums.Face.Two);
	assert.equal(Enums.Face.parse('THREE'), Enums.Face.Three);
	assert.equal(Enums.Face.parse('THREES'), Enums.Face.Three);
	assert.equal(Enums.Face.parse('FOUR'), Enums.Face.Four);
	assert.equal(Enums.Face.parse('FOURS'), Enums.Face.Four);
	assert.equal(Enums.Face.parse('FIVE'), Enums.Face.Five);
	assert.equal(Enums.Face.parse('FIVES'), Enums.Face.Five);
	assert.equal(Enums.Face.parse('SIX'), Enums.Face.Six);
	assert.equal(Enums.Face.parse('SIXES'), Enums.Face.Six);
	assert.equal(Enums.Face.parse('SEVEN'), Enums.Face.Seven);
	assert.equal(Enums.Face.parse('SEVENS'), Enums.Face.Seven);
	assert.equal(Enums.Face.parse('EIGHT'), Enums.Face.Eight);
	assert.equal(Enums.Face.parse('EIGHTS'), Enums.Face.Eight);
	assert.equal(Enums.Face.parse('NINE'), Enums.Face.Nine);
	assert.equal(Enums.Face.parse('NINES'), Enums.Face.Nine);
	assert.equal(Enums.Face.parse('T'), Enums.Face.Ten);
	assert.equal(Enums.Face.parse('TEN'), Enums.Face.Ten);
	assert.equal(Enums.Face.parse('TENS'), Enums.Face.Ten);
	assert.equal(Enums.Face.parse('J'), Enums.Face.Jack);
	assert.equal(Enums.Face.parse('JACK'), Enums.Face.Jack);
	assert.equal(Enums.Face.parse('JACKS'), Enums.Face.Jack);
	assert.equal(Enums.Face.parse('Q'), Enums.Face.Queen);
	assert.equal(Enums.Face.parse('QUEEN'), Enums.Face.Queen);
	assert.equal(Enums.Face.parse('QUEENS'), Enums.Face.Queen);
	assert.equal(Enums.Face.parse('K'), Enums.Face.King);
	assert.equal(Enums.Face.parse('KING'), Enums.Face.King);
	assert.equal(Enums.Face.parse('KINGS'), Enums.Face.King);
	assert.equal(Enums.Face.parse('A'), Enums.Face.Ace);
	assert.equal(Enums.Face.parse('ACE'), Enums.Face.Ace);
	assert.equal(Enums.Face.parse('ACES'), Enums.Face.Ace);
});

Tests.add("Enums.Face.parse() from invalid should return null", () => {
	assert.equal(Enums.Face.parse(), null);
	assert.equal(Enums.Face.parse(null), null);
	assert.equal(Enums.Face.parse(0), null);
	assert.equal(Enums.Face.parse('0'), null);
	assert.equal(Enums.Face.parse('x'), null);
	assert.equal(Enums.Face.parse('X'), null);
	assert.equal(Enums.Face.parse('Example'), null);
});

// Suit

Tests.add("Enums.Suit should be exact numeric values for indexing", () => {
	assert.equal(Enums.Suit.Heart, 1);
	assert.equal(Enums.Suit.Diamond, 2);
	assert.equal(Enums.Suit.Club, 3);
	assert.equal(Enums.Suit.Spade, 4);
});

Tests.add("Enums.Suit.isValid() should return correct bool", () => {
	assert.ok(!Enums.Suit.isValid(0));
	assert.ok(Enums.Suit.isValid(Enums.Suit.Heart));
	assert.ok(Enums.Suit.isValid(Enums.Suit.Diamond));
	assert.ok(Enums.Suit.isValid(Enums.Suit.Club));
	assert.ok(Enums.Suit.isValid(Enums.Suit.Spade));
	assert.ok(!Enums.Suit.isValid(5));
});

Tests.add("Enums.Suit.toString() should return correct short string", () => {
	assert.equal(Enums.Suit.toString(Enums.Suit.Heart), 'h');
	assert.equal(Enums.Suit.toString(Enums.Suit.Diamond), 'd');
	assert.equal(Enums.Suit.toString(Enums.Suit.Club), 'c');
	assert.equal(Enums.Suit.toString(Enums.Suit.Spade), 's');
});

Tests.add("Enums.Suit.toString('l') should return correct long string", () => {
	assert.equal(Enums.Suit.toString(Enums.Suit.Heart, 'l'), 'Heart');
	assert.equal(Enums.Suit.toString(Enums.Suit.Diamond, 'l'), 'Diamond');
	assert.equal(Enums.Suit.toString(Enums.Suit.Club, 'l'), 'Club');
	assert.equal(Enums.Suit.toString(Enums.Suit.Spade, 'l'), 'Spade');
});

Tests.add("Enums.Suit.parse() from binary should return correct value from string", () => {
	assert.equal(Enums.Suit.parse(Enums.Suit.Heart), Enums.Suit.Heart);
	assert.equal(Enums.Suit.parse(Enums.Suit.Diamond), Enums.Suit.Diamond);
	assert.equal(Enums.Suit.parse(Enums.Suit.Club), Enums.Suit.Club);
	assert.equal(Enums.Suit.parse(Enums.Suit.Spade), Enums.Suit.Spade);
});

Tests.add("Enums.Suit.parse() from lower case should return correct value from string", () => {
	assert.equal(Enums.Suit.parse('h'), Enums.Suit.Heart);
	assert.equal(Enums.Suit.parse('heart'), Enums.Suit.Heart);
	assert.equal(Enums.Suit.parse('hearts'), Enums.Suit.Heart);
	assert.equal(Enums.Suit.parse('d'), Enums.Suit.Diamond);
	assert.equal(Enums.Suit.parse('diamond'), Enums.Suit.Diamond);
	assert.equal(Enums.Suit.parse('diamonds'), Enums.Suit.Diamond);
	assert.equal(Enums.Suit.parse('c'), Enums.Suit.Club);
	assert.equal(Enums.Suit.parse('club'), Enums.Suit.Club);
	assert.equal(Enums.Suit.parse('clubs'), Enums.Suit.Club);
	assert.equal(Enums.Suit.parse('s'), Enums.Suit.Spade);
	assert.equal(Enums.Suit.parse('spade'), Enums.Suit.Spade);
	assert.equal(Enums.Suit.parse('spades'), Enums.Suit.Spade);
});

Tests.add("Enums.Suit.parse() from upper case should return correct value from string", () => {
	assert.equal(Enums.Suit.parse('H'), Enums.Suit.Heart);
	assert.equal(Enums.Suit.parse('HEART'), Enums.Suit.Heart);
	assert.equal(Enums.Suit.parse('HEARTS'), Enums.Suit.Heart);
	assert.equal(Enums.Suit.parse('D'), Enums.Suit.Diamond);
	assert.equal(Enums.Suit.parse('DIAMOND'), Enums.Suit.Diamond);
	assert.equal(Enums.Suit.parse('DIAMONDS'), Enums.Suit.Diamond);
	assert.equal(Enums.Suit.parse('C'), Enums.Suit.Club);
	assert.equal(Enums.Suit.parse('CLUB'), Enums.Suit.Club);
	assert.equal(Enums.Suit.parse('CLUBS'), Enums.Suit.Club);
	assert.equal(Enums.Suit.parse('S'), Enums.Suit.Spade);
	assert.equal(Enums.Suit.parse('SPADE'), Enums.Suit.Spade);
	assert.equal(Enums.Suit.parse('SPADES'), Enums.Suit.Spade);
});

Tests.add("Enums.Suit.parse() from invalid should return null", () => {
	assert.equal(Enums.Suit.parse(), null);
	assert.equal(Enums.Suit.parse(null), null);
	assert.equal(Enums.Suit.parse(0), null);
	assert.equal(Enums.Suit.parse('0'), null);
	assert.equal(Enums.Suit.parse('x'), null);
	assert.equal(Enums.Suit.parse('X'), null);
	assert.equal(Enums.Suit.parse('Example'), null);
});

// HandTypes

Tests.add("Enums.HandTypes should be ordinal relative to scoring", () => {
	assert.ok(Enums.HandTypes.HighCard < Enums.HandTypes.Pair);
	assert.ok(Enums.HandTypes.Pair < Enums.HandTypes.TwoPair);
	assert.ok(Enums.HandTypes.TwoPair < Enums.HandTypes.ThreeOfAKind);
	assert.ok(Enums.HandTypes.ThreeOfAKind < Enums.HandTypes.Straight);
	assert.ok(Enums.HandTypes.Straight < Enums.HandTypes.Flush);
	assert.ok(Enums.HandTypes.Flush < Enums.HandTypes.FullHouse);
	assert.ok(Enums.HandTypes.FullHouse < Enums.HandTypes.FourOfAKind);
	assert.ok(Enums.HandTypes.FourOfAKind < Enums.HandTypes.StraightFlush);
	assert.ok(Enums.HandTypes.StraightFlush < Enums.HandTypes.RoyalFlush);
});

Tests.add("Enums.HandTypes.isValid() should return correct bool", () => {
	assert.ok(!Enums.HandTypes.isValid(0));
	assert.ok(Enums.HandTypes.isValid(Enums.HandTypes.HighCard));
	assert.ok(Enums.HandTypes.isValid(Enums.HandTypes.Pair));
	assert.ok(Enums.HandTypes.isValid(Enums.HandTypes.TwoPair));
	assert.ok(Enums.HandTypes.isValid(Enums.HandTypes.ThreeOfAKind));
	assert.ok(Enums.HandTypes.isValid(Enums.HandTypes.Straight));
	assert.ok(Enums.HandTypes.isValid(Enums.HandTypes.Flush));
	assert.ok(Enums.HandTypes.isValid(Enums.HandTypes.FullHouse));
	assert.ok(Enums.HandTypes.isValid(Enums.HandTypes.FourOfAKind));
	assert.ok(Enums.HandTypes.isValid(Enums.HandTypes.StraightFlush));
	assert.ok(!Enums.HandTypes.isValid(11));
});

Tests.add("Enums.HandTypes.toString() should return correct string", () => {
	assert.equal(Enums.HandTypes.toString(Enums.HandTypes.HighCard), 'HighCard');
	assert.equal(Enums.HandTypes.toString(Enums.HandTypes.Pair), 'Pair');
	assert.equal(Enums.HandTypes.toString(Enums.HandTypes.TwoPair), 'TwoPair');
	assert.equal(Enums.HandTypes.toString(Enums.HandTypes.ThreeOfAKind), 'ThreeOfAKind');
	assert.equal(Enums.HandTypes.toString(Enums.HandTypes.Straight), 'Straight');
	assert.equal(Enums.HandTypes.toString(Enums.HandTypes.Flush), 'Flush');
	assert.equal(Enums.HandTypes.toString(Enums.HandTypes.FullHouse), 'FullHouse');
	assert.equal(Enums.HandTypes.toString(Enums.HandTypes.FourOfAKind), 'FourOfAKind');
	assert.equal(Enums.HandTypes.toString(Enums.HandTypes.StraightFlush), 'StraightFlush');
});

// BettingRoundStatus

Tests.add("BettingRoundStatus should be ordinal", () => {
	assert.ok(Enums.BettingRoundStatus.Unopened < Enums.BettingRoundStatus.Folded);
	assert.ok(Enums.BettingRoundStatus.Folded < Enums.BettingRoundStatus.Checked);
	assert.ok(Enums.BettingRoundStatus.Checked < Enums.BettingRoundStatus.Called);
	assert.ok(Enums.BettingRoundStatus.Called < Enums.BettingRoundStatus.Raised);
});

// TableStatus

Tests.add("TableStatus should be exact numeric values", () => {
	assert.ok(Enums.TableStatus.BeforeFlop, 1);
	assert.ok(Enums.TableStatus.BeforeTurn, 2);
	assert.ok(Enums.TableStatus.BeforeRiver, 3);
	assert.ok(Enums.TableStatus.Complete, 4);
});
