import { strict as assert } from 'assert';
import { Tests } from './TestFixture.mjs';
import { Enums } from '../src/Enums.mjs';
import { Deck } from '../src/Deck.mjs';
import { Card } from '../src/Card.mjs';

Tests.add('Card.constructor() should return zero and invalid', () => {
	assert.ok(!(new Card).isValid());
	assert.equal(0, (new Card).face);
	assert.equal(0, (new Card).suit);
});

Tests.add('Card.isValid() valid should return true', () => {
	assert.ok(Card.parse('As').isValid());
	assert.ok(Card.parse('Kd').isValid());
	assert.ok(Card.parse('Tc').isValid());
	assert.ok(Card.parse('8h').isValid());
	assert.ok(Card.parse('3c').isValid());
});

Tests.add('Card.isValid() invalid should return false', () => {
	assert.ok(!(new Card).isValid());
	let card = new Card();
	card.face = Enums.Face.Ace;
	card.suit = 0;
	assert.ok(!card.isValid());
	card.face = 0;
	card.suit = Enums.Suit.Heart;
	assert.ok(!card.isValid());
});

Tests.add('Card.uint8() valid should return correct encoding', () => {
	assert.equal(Card.parse('2h').uint8(), 0x21);
	assert.equal(Card.parse('2d').uint8(), 0x22);
	assert.equal(Card.parse('2c').uint8(), 0x23);
	assert.equal(Card.parse('2s').uint8(), 0x24);
	assert.equal(Card.parse('3h').uint8(), 0x31);
	assert.equal(Card.parse('3d').uint8(), 0x32);
	assert.equal(Card.parse('3c').uint8(), 0x33);
	assert.equal(Card.parse('3s').uint8(), 0x34);
	assert.equal(Card.parse('4h').uint8(), 0x41);
	assert.equal(Card.parse('4d').uint8(), 0x42);
	assert.equal(Card.parse('4c').uint8(), 0x43);
	assert.equal(Card.parse('4s').uint8(), 0x44);
	assert.equal(Card.parse('5h').uint8(), 0x51);
	assert.equal(Card.parse('5d').uint8(), 0x52);
	assert.equal(Card.parse('5c').uint8(), 0x53);
	assert.equal(Card.parse('5s').uint8(), 0x54);
	assert.equal(Card.parse('6h').uint8(), 0x61);
	assert.equal(Card.parse('6d').uint8(), 0x62);
	assert.equal(Card.parse('6c').uint8(), 0x63);
	assert.equal(Card.parse('6s').uint8(), 0x64);
	assert.equal(Card.parse('7h').uint8(), 0x71);
	assert.equal(Card.parse('7d').uint8(), 0x72);
	assert.equal(Card.parse('7c').uint8(), 0x73);
	assert.equal(Card.parse('7s').uint8(), 0x74);
	assert.equal(Card.parse('8h').uint8(), 0x81);
	assert.equal(Card.parse('8d').uint8(), 0x82);
	assert.equal(Card.parse('8c').uint8(), 0x83);
	assert.equal(Card.parse('8s').uint8(), 0x84);
	assert.equal(Card.parse('9h').uint8(), 0x91);
	assert.equal(Card.parse('9d').uint8(), 0x92);
	assert.equal(Card.parse('9c').uint8(), 0x93);
	assert.equal(Card.parse('9s').uint8(), 0x94);
	assert.equal(Card.parse('Th').uint8(), 0xA1);
	assert.equal(Card.parse('Td').uint8(), 0xA2);
	assert.equal(Card.parse('Tc').uint8(), 0xA3);
	assert.equal(Card.parse('Ts').uint8(), 0xA4);
	assert.equal(Card.parse('Jh').uint8(), 0xB1);
	assert.equal(Card.parse('Jd').uint8(), 0xB2);
	assert.equal(Card.parse('Jc').uint8(), 0xB3);
	assert.equal(Card.parse('Js').uint8(), 0xB4);
	assert.equal(Card.parse('Qh').uint8(), 0xC1);
	assert.equal(Card.parse('Qd').uint8(), 0xC2);
	assert.equal(Card.parse('Qc').uint8(), 0xC3);
	assert.equal(Card.parse('Qs').uint8(), 0xC4);
	assert.equal(Card.parse('Kh').uint8(), 0xD1);
	assert.equal(Card.parse('Kd').uint8(), 0xD2);
	assert.equal(Card.parse('Kc').uint8(), 0xD3);
	assert.equal(Card.parse('Ks').uint8(), 0xD4);
	assert.equal(Card.parse('Ah').uint8(), 0xE1);
	assert.equal(Card.parse('Ad').uint8(), 0xE2);
	assert.equal(Card.parse('Ac').uint8(), 0xE3);
	assert.equal(Card.parse('As').uint8(), 0xE4);
});

Tests.add('Card.uint8() invalid should return zero', () => {
	assert.equal((new Card).uint8(), 0);
	let card = new Card();
	card.face = Enums.Face.Ace;
	card.suit = 0;
	assert.equal(card.uint8(), 0);
	card.face = 0;
	card.suit = Enums.Suit.Heart;
	assert.equal(card.uint8(), 0);
});

Tests.add('Card.equals() not another card should return false', () => {
	assert.ok(!Card.parse('2h').equals());
	assert.ok(!Card.parse('2h').equals(null));
	assert.ok(!Card.parse('2h').equals('2h'));
	assert.ok(!Card.parse('2h').equals(new Object()));
	assert.ok(!Card.parse('2h').equals(0x21));
});

Tests.add('Card.equals() another card but different face should return false', () => {
	assert.ok(!Card.parse('2h').equals(Card.parse(0x31)));
	assert.ok(!Card.parse('3d').equals(Card.parse(0x42)));
	assert.ok(!Card.parse('4c').equals(Card.parse(0x53)));
	assert.ok(!Card.parse('5s').equals(Card.parse(0x64)));
});

Tests.add('Card.equals() another card but different suit should return false', () => {
	assert.ok(!Card.parse('2h').equals(Card.parse(0x22)));
	assert.ok(!Card.parse('3d').equals(Card.parse(0x31)));
	assert.ok(!Card.parse('4c').equals(Card.parse(0x44)));
	assert.ok(!Card.parse('5s').equals(Card.parse(0x53)));
});

Tests.add('Card.equals() another card equal should return true', () => {
	assert.ok(Card.parse('2h').equals(Card.parse(0x21)));
	assert.ok(Card.parse('2d').equals(Card.parse(0x22)));
	assert.ok(Card.parse('2c').equals(Card.parse(0x23)));
	assert.ok(Card.parse('2s').equals(Card.parse(0x24)));
	assert.ok(Card.parse('3h').equals(Card.parse(0x31)));
	assert.ok(Card.parse('3d').equals(Card.parse(0x32)));
	assert.ok(Card.parse('3c').equals(Card.parse(0x33)));
	assert.ok(Card.parse('3s').equals(Card.parse(0x34)));
	assert.ok(Card.parse('4h').equals(Card.parse(0x41)));
	assert.ok(Card.parse('4d').equals(Card.parse(0x42)));
	assert.ok(Card.parse('4c').equals(Card.parse(0x43)));
	assert.ok(Card.parse('4s').equals(Card.parse(0x44)));
	assert.ok(Card.parse('5h').equals(Card.parse(0x51)));
	assert.ok(Card.parse('5d').equals(Card.parse(0x52)));
	assert.ok(Card.parse('5c').equals(Card.parse(0x53)));
	assert.ok(Card.parse('5s').equals(Card.parse(0x54)));
	assert.ok(Card.parse('6h').equals(Card.parse(0x61)));
	assert.ok(Card.parse('6d').equals(Card.parse(0x62)));
	assert.ok(Card.parse('6c').equals(Card.parse(0x63)));
	assert.ok(Card.parse('6s').equals(Card.parse(0x64)));
	assert.ok(Card.parse('7h').equals(Card.parse(0x71)));
	assert.ok(Card.parse('7d').equals(Card.parse(0x72)));
	assert.ok(Card.parse('7c').equals(Card.parse(0x73)));
	assert.ok(Card.parse('7s').equals(Card.parse(0x74)));
	assert.ok(Card.parse('8h').equals(Card.parse(0x81)));
	assert.ok(Card.parse('8d').equals(Card.parse(0x82)));
	assert.ok(Card.parse('8c').equals(Card.parse(0x83)));
	assert.ok(Card.parse('8s').equals(Card.parse(0x84)));
	assert.ok(Card.parse('9h').equals(Card.parse(0x91)));
	assert.ok(Card.parse('9d').equals(Card.parse(0x92)));
	assert.ok(Card.parse('9c').equals(Card.parse(0x93)));
	assert.ok(Card.parse('9s').equals(Card.parse(0x94)));
	assert.ok(Card.parse('Th').equals(Card.parse(0xA1)));
	assert.ok(Card.parse('Td').equals(Card.parse(0xA2)));
	assert.ok(Card.parse('Tc').equals(Card.parse(0xA3)));
	assert.ok(Card.parse('Ts').equals(Card.parse(0xA4)));
	assert.ok(Card.parse('Jh').equals(Card.parse(0xB1)));
	assert.ok(Card.parse('Jd').equals(Card.parse(0xB2)));
	assert.ok(Card.parse('Jc').equals(Card.parse(0xB3)));
	assert.ok(Card.parse('Js').equals(Card.parse(0xB4)));
	assert.ok(Card.parse('Qh').equals(Card.parse(0xC1)));
	assert.ok(Card.parse('Qd').equals(Card.parse(0xC2)));
	assert.ok(Card.parse('Qc').equals(Card.parse(0xC3)));
	assert.ok(Card.parse('Qs').equals(Card.parse(0xC4)));
	assert.ok(Card.parse('Kh').equals(Card.parse(0xD1)));
	assert.ok(Card.parse('Kd').equals(Card.parse(0xD2)));
	assert.ok(Card.parse('Kc').equals(Card.parse(0xD3)));
	assert.ok(Card.parse('Ks').equals(Card.parse(0xD4)));
	assert.ok(Card.parse('Ah').equals(Card.parse(0xE1)));
	assert.ok(Card.parse('Ad').equals(Card.parse(0xE2)));
	assert.ok(Card.parse('Ac').equals(Card.parse(0xE3)));
	assert.ok(Card.parse('As').equals(Card.parse(0xE4)));
});

Tests.add('Card.toString() invalid should return empty', () => {
	assert.equal((new Card).toString(), '');
	let card = new Card();
	card.face = Enums.Face.Ace;
	card.suit = 0;
	assert.equal(card.toString(), '');
	card.face = 0;
	card.suit = Enums.Suit.Heart;
	assert.equal(card.toString(), '');
});

Tests.add('Card.toString() short format should return correct format', () => {
	assert.equal(Card.parse('2h').toString(), '2h');
	assert.equal(Card.parse('2d').toString(), '2d');
	assert.equal(Card.parse('2c').toString(), '2c');
	assert.equal(Card.parse('2s').toString(), '2s');
	assert.equal(Card.parse('3h').toString(), '3h');
	assert.equal(Card.parse('3d').toString(), '3d');
	assert.equal(Card.parse('3c').toString(), '3c');
	assert.equal(Card.parse('3s').toString(), '3s');
	assert.equal(Card.parse('4h').toString(), '4h');
	assert.equal(Card.parse('4d').toString(), '4d');
	assert.equal(Card.parse('4c').toString(), '4c');
	assert.equal(Card.parse('4s').toString(), '4s');
	assert.equal(Card.parse('5h').toString(), '5h');
	assert.equal(Card.parse('5d').toString(), '5d');
	assert.equal(Card.parse('5c').toString(), '5c');
	assert.equal(Card.parse('5s').toString(), '5s');
	assert.equal(Card.parse('6h').toString(), '6h');
	assert.equal(Card.parse('6d').toString(), '6d');
	assert.equal(Card.parse('6c').toString(), '6c');
	assert.equal(Card.parse('6s').toString(), '6s');
	assert.equal(Card.parse('7h').toString(), '7h');
	assert.equal(Card.parse('7d').toString(), '7d');
	assert.equal(Card.parse('7c').toString(), '7c');
	assert.equal(Card.parse('7s').toString(), '7s');
	assert.equal(Card.parse('8h').toString(), '8h');
	assert.equal(Card.parse('8d').toString(), '8d');
	assert.equal(Card.parse('8c').toString(), '8c');
	assert.equal(Card.parse('8s').toString(), '8s');
	assert.equal(Card.parse('9h').toString(), '9h');
	assert.equal(Card.parse('9d').toString(), '9d');
	assert.equal(Card.parse('9c').toString(), '9c');
	assert.equal(Card.parse('9s').toString(), '9s');
	assert.equal(Card.parse('Th').toString(), 'Th');
	assert.equal(Card.parse('Td').toString(), 'Td');
	assert.equal(Card.parse('Tc').toString(), 'Tc');
	assert.equal(Card.parse('Ts').toString(), 'Ts');
	assert.equal(Card.parse('Jh').toString(), 'Jh');
	assert.equal(Card.parse('Jd').toString(), 'Jd');
	assert.equal(Card.parse('Jc').toString(), 'Jc');
	assert.equal(Card.parse('Js').toString(), 'Js');
	assert.equal(Card.parse('Qh').toString(), 'Qh');
	assert.equal(Card.parse('Qd').toString(), 'Qd');
	assert.equal(Card.parse('Qc').toString(), 'Qc');
	assert.equal(Card.parse('Qs').toString(), 'Qs');
	assert.equal(Card.parse('Kh').toString(), 'Kh');
	assert.equal(Card.parse('Kd').toString(), 'Kd');
	assert.equal(Card.parse('Kc').toString(), 'Kc');
	assert.equal(Card.parse('Ks').toString(), 'Ks');
	assert.equal(Card.parse('Ah').toString(), 'Ah');
	assert.equal(Card.parse('Ad').toString(), 'Ad');
	assert.equal(Card.parse('Ac').toString(), 'Ac');
	assert.equal(Card.parse('As').toString(), 'As');
});

Tests.add('Card.toString() long format should return correct format', () => {
	assert.equal(Card.parse('2h').toString('l'), 'Two of Hearts');
	assert.equal(Card.parse('2d').toString('l'), 'Two of Diamonds');
	assert.equal(Card.parse('2c').toString('l'), 'Two of Clubs');
	assert.equal(Card.parse('2s').toString('l'), 'Two of Spades');
	assert.equal(Card.parse('3h').toString('l'), 'Three of Hearts');
	assert.equal(Card.parse('3d').toString('l'), 'Three of Diamonds');
	assert.equal(Card.parse('3c').toString('l'), 'Three of Clubs');
	assert.equal(Card.parse('3s').toString('l'), 'Three of Spades');
	assert.equal(Card.parse('4h').toString('l'), 'Four of Hearts');
	assert.equal(Card.parse('4d').toString('l'), 'Four of Diamonds');
	assert.equal(Card.parse('4c').toString('l'), 'Four of Clubs');
	assert.equal(Card.parse('4s').toString('l'), 'Four of Spades');
	assert.equal(Card.parse('5h').toString('l'), 'Five of Hearts');
	assert.equal(Card.parse('5d').toString('l'), 'Five of Diamonds');
	assert.equal(Card.parse('5c').toString('l'), 'Five of Clubs');
	assert.equal(Card.parse('5s').toString('l'), 'Five of Spades');
	assert.equal(Card.parse('6h').toString('l'), 'Six of Hearts');
	assert.equal(Card.parse('6d').toString('l'), 'Six of Diamonds');
	assert.equal(Card.parse('6c').toString('l'), 'Six of Clubs');
	assert.equal(Card.parse('6s').toString('l'), 'Six of Spades');
	assert.equal(Card.parse('7h').toString('l'), 'Seven of Hearts');
	assert.equal(Card.parse('7d').toString('l'), 'Seven of Diamonds');
	assert.equal(Card.parse('7c').toString('l'), 'Seven of Clubs');
	assert.equal(Card.parse('7s').toString('l'), 'Seven of Spades');
	assert.equal(Card.parse('8h').toString('l'), 'Eight of Hearts');
	assert.equal(Card.parse('8d').toString('l'), 'Eight of Diamonds');
	assert.equal(Card.parse('8c').toString('l'), 'Eight of Clubs');
	assert.equal(Card.parse('8s').toString('l'), 'Eight of Spades');
	assert.equal(Card.parse('9h').toString('l'), 'Nine of Hearts');
	assert.equal(Card.parse('9d').toString('l'), 'Nine of Diamonds');
	assert.equal(Card.parse('9c').toString('l'), 'Nine of Clubs');
	assert.equal(Card.parse('9s').toString('l'), 'Nine of Spades');
	assert.equal(Card.parse('Th').toString('l'), 'Ten of Hearts');
	assert.equal(Card.parse('Td').toString('l'), 'Ten of Diamonds');
	assert.equal(Card.parse('Tc').toString('l'), 'Ten of Clubs');
	assert.equal(Card.parse('Ts').toString('l'), 'Ten of Spades');
	assert.equal(Card.parse('Jh').toString('l'), 'Jack of Hearts');
	assert.equal(Card.parse('Jd').toString('l'), 'Jack of Diamonds');
	assert.equal(Card.parse('Jc').toString('l'), 'Jack of Clubs');
	assert.equal(Card.parse('Js').toString('l'), 'Jack of Spades');
	assert.equal(Card.parse('Qh').toString('l'), 'Queen of Hearts');
	assert.equal(Card.parse('Qd').toString('l'), 'Queen of Diamonds');
	assert.equal(Card.parse('Qc').toString('l'), 'Queen of Clubs');
	assert.equal(Card.parse('Qs').toString('l'), 'Queen of Spades');
	assert.equal(Card.parse('Kh').toString('l'), 'King of Hearts');
	assert.equal(Card.parse('Kd').toString('l'), 'King of Diamonds');
	assert.equal(Card.parse('Kc').toString('l'), 'King of Clubs');
	assert.equal(Card.parse('Ks').toString('l'), 'King of Spades');
	assert.equal(Card.parse('Ah').toString('l'), 'Ace of Hearts');
	assert.equal(Card.parse('Ad').toString('l'), 'Ace of Diamonds');
	assert.equal(Card.parse('Ac').toString('l'), 'Ace of Clubs');
	assert.equal(Card.parse('As').toString('l'), 'Ace of Spades');
});

Tests.add('Card.parseShortArray() total nonsense should throw ', () => {
	assert.throws(() => Card.parseShortArray("nothing here is real"), /^Invalid card string: "nothing"$/);
});

Tests.add('Card.parseShortArray() nonsense within valid should throw ', () => {
	assert.throws(() => Card.parseShortArray("2h nonsense As"), /^Invalid card string: "nonsense"$/);
});

Tests.add('Card.parseShortArray() single long card should throw ', () => {
	assert.throws(() => Card.parseShortArray("Ace of Spades"), /^Invalid card string: "Ace"$/);
});

Tests.add('Card.parseShortArray() long card in shorts should throw ', () => {
	assert.throws(() => Card.parseShortArray("2h Ace of Spades Kc"), /^Invalid card string: "Ace"$/);
});

Tests.add('Card.parseShortArray() empty should return empty', () => {
	assert.equal(Card.parseShortArray("").length, 0);
});

Tests.add('Card.parseShortArray() whitespace should return empty', () => {
	assert.equal(Card.parseShortArray(" \t \r \n ").length, 0);
});

Tests.add('Card.parseShortArray() extra whitespace should return valid', () => {
	let actual = Card.parseShortArray(" \t 2h\r \nKs ")
	assert.equal(actual.length, 2);
	assert.equal(actual[0].uint8(), 0x21);
	assert.equal(actual[1].uint8(), 0xD4);
});

Tests.add('Card.parseShortArray() single should return single', () => {
	let actual = Card.parseShortArray("7d");
	assert.equal(actual.length, 1);
	assert.equal(actual[0].uint8(), 0x72);
});

Tests.add('Card.parseShortArray() multiple should return multiple', () => {
	let actual = Card.parseShortArray("2h 3d 4c 5s 6h 7d");
	assert.equal(actual.length, 6);
	assert.equal(actual[0].uint8(), 0x21);
	assert.equal(actual[1].uint8(), 0x32);
	assert.equal(actual[2].uint8(), 0x43);
	assert.equal(actual[3].uint8(), 0x54);
	assert.equal(actual[4].uint8(), 0x61);
	assert.equal(actual[5].uint8(), 0x72);
});

Tests.add('Card.parse() not a number or a string should return null', () => {
	assert.ok(!Card.parse());
	assert.ok(!Card.parse(null));
	assert.ok(!Card.parse(new Object()));
	assert.ok(!Card.parse(new Card()));
});

Tests.add('Card.parse() number invalid face should return null', () => {
	assert.ok(!Card.parse(0x01));
	assert.ok(!Card.parse(0x02));
	assert.ok(!Card.parse(0x03));
	assert.ok(!Card.parse(0x04));
	assert.ok(!Card.parse(0xF1));
	assert.ok(!Card.parse(0xF2));
	assert.ok(!Card.parse(0xF3));
	assert.ok(!Card.parse(0xF4));
});

Tests.add('Card.parse() number invalid suit should return null', () => {
	assert.ok(!Card.parse(0x20));
	assert.ok(!Card.parse(0x25));
	assert.ok(!Card.parse(0x30));
	assert.ok(!Card.parse(0x35));
});

Tests.add('Card.parse() number valid should return card', () => {
	assert.ok(Card.parse(0x21).equals(Card.parse('2h')));
	assert.ok(Card.parse(0x22).equals(Card.parse('2d')));
	assert.ok(Card.parse(0x23).equals(Card.parse('2c')));
	assert.ok(Card.parse(0x24).equals(Card.parse('2s')));
	assert.ok(Card.parse(0x31).equals(Card.parse('3h')));
	assert.ok(Card.parse(0x32).equals(Card.parse('3d')));
	assert.ok(Card.parse(0x33).equals(Card.parse('3c')));
	assert.ok(Card.parse(0x34).equals(Card.parse('3s')));
	assert.ok(Card.parse(0x41).equals(Card.parse('4h')));
	assert.ok(Card.parse(0x42).equals(Card.parse('4d')));
	assert.ok(Card.parse(0x43).equals(Card.parse('4c')));
	assert.ok(Card.parse(0x44).equals(Card.parse('4s')));
	assert.ok(Card.parse(0x51).equals(Card.parse('5h')));
	assert.ok(Card.parse(0x52).equals(Card.parse('5d')));
	assert.ok(Card.parse(0x53).equals(Card.parse('5c')));
	assert.ok(Card.parse(0x54).equals(Card.parse('5s')));
	assert.ok(Card.parse(0x61).equals(Card.parse('6h')));
	assert.ok(Card.parse(0x62).equals(Card.parse('6d')));
	assert.ok(Card.parse(0x63).equals(Card.parse('6c')));
	assert.ok(Card.parse(0x64).equals(Card.parse('6s')));
	assert.ok(Card.parse(0x71).equals(Card.parse('7h')));
	assert.ok(Card.parse(0x72).equals(Card.parse('7d')));
	assert.ok(Card.parse(0x73).equals(Card.parse('7c')));
	assert.ok(Card.parse(0x74).equals(Card.parse('7s')));
	assert.ok(Card.parse(0x81).equals(Card.parse('8h')));
	assert.ok(Card.parse(0x82).equals(Card.parse('8d')));
	assert.ok(Card.parse(0x83).equals(Card.parse('8c')));
	assert.ok(Card.parse(0x84).equals(Card.parse('8s')));
	assert.ok(Card.parse(0x91).equals(Card.parse('9h')));
	assert.ok(Card.parse(0x92).equals(Card.parse('9d')));
	assert.ok(Card.parse(0x93).equals(Card.parse('9c')));
	assert.ok(Card.parse(0x94).equals(Card.parse('9s')));
	assert.ok(Card.parse(0xA1).equals(Card.parse('Th')));
	assert.ok(Card.parse(0xA2).equals(Card.parse('Td')));
	assert.ok(Card.parse(0xA3).equals(Card.parse('Tc')));
	assert.ok(Card.parse(0xA4).equals(Card.parse('Ts')));
	assert.ok(Card.parse(0xB1).equals(Card.parse('Jh')));
	assert.ok(Card.parse(0xB2).equals(Card.parse('Jd')));
	assert.ok(Card.parse(0xB3).equals(Card.parse('Jc')));
	assert.ok(Card.parse(0xB4).equals(Card.parse('Js')));
	assert.ok(Card.parse(0xC1).equals(Card.parse('Qh')));
	assert.ok(Card.parse(0xC2).equals(Card.parse('Qd')));
	assert.ok(Card.parse(0xC3).equals(Card.parse('Qc')));
	assert.ok(Card.parse(0xC4).equals(Card.parse('Qs')));
	assert.ok(Card.parse(0xD1).equals(Card.parse('Kh')));
	assert.ok(Card.parse(0xD2).equals(Card.parse('Kd')));
	assert.ok(Card.parse(0xD3).equals(Card.parse('Kc')));
	assert.ok(Card.parse(0xD4).equals(Card.parse('Ks')));
	assert.ok(Card.parse(0xE1).equals(Card.parse('Ah')));
	assert.ok(Card.parse(0xE2).equals(Card.parse('Ad')));
	assert.ok(Card.parse(0xE3).equals(Card.parse('Ac')));
	assert.ok(Card.parse(0xE4).equals(Card.parse('As')));
});

Tests.add('Card.parse() short string invalid face should return null', () => {
	assert.ok(!Card.parse('1h'));
	assert.ok(!Card.parse('1d'));
	assert.ok(!Card.parse('Xc'));
	assert.ok(!Card.parse('Ys'));
	assert.ok(!Card.parse('Zh'));
});

Tests.add('Card.parse() short string invalid suit should return null', () => {
	assert.ok(!Card.parse('2a'));
	assert.ok(!Card.parse('2b'));
	assert.ok(!Card.parse('2x'));
	assert.ok(!Card.parse('2y'));
	assert.ok(!Card.parse('2z'));
});

Tests.add('Card.parse() short string valid should return card', () => {
	assert.equal(Card.parse('2h').uint8(), 0x21);
	assert.equal(Card.parse('2d').uint8(), 0x22);
	assert.equal(Card.parse('2c').uint8(), 0x23);
	assert.equal(Card.parse('2s').uint8(), 0x24);
	assert.equal(Card.parse('3h').uint8(), 0x31);
	assert.equal(Card.parse('3d').uint8(), 0x32);
	assert.equal(Card.parse('3c').uint8(), 0x33);
	assert.equal(Card.parse('3s').uint8(), 0x34);
	assert.equal(Card.parse('4h').uint8(), 0x41);
	assert.equal(Card.parse('4d').uint8(), 0x42);
	assert.equal(Card.parse('4c').uint8(), 0x43);
	assert.equal(Card.parse('4s').uint8(), 0x44);
	assert.equal(Card.parse('5h').uint8(), 0x51);
	assert.equal(Card.parse('5d').uint8(), 0x52);
	assert.equal(Card.parse('5c').uint8(), 0x53);
	assert.equal(Card.parse('5s').uint8(), 0x54);
	assert.equal(Card.parse('6h').uint8(), 0x61);
	assert.equal(Card.parse('6d').uint8(), 0x62);
	assert.equal(Card.parse('6c').uint8(), 0x63);
	assert.equal(Card.parse('6s').uint8(), 0x64);
	assert.equal(Card.parse('7h').uint8(), 0x71);
	assert.equal(Card.parse('7d').uint8(), 0x72);
	assert.equal(Card.parse('7c').uint8(), 0x73);
	assert.equal(Card.parse('7s').uint8(), 0x74);
	assert.equal(Card.parse('8h').uint8(), 0x81);
	assert.equal(Card.parse('8d').uint8(), 0x82);
	assert.equal(Card.parse('8c').uint8(), 0x83);
	assert.equal(Card.parse('8s').uint8(), 0x84);
	assert.equal(Card.parse('9h').uint8(), 0x91);
	assert.equal(Card.parse('9d').uint8(), 0x92);
	assert.equal(Card.parse('9c').uint8(), 0x93);
	assert.equal(Card.parse('9s').uint8(), 0x94);
	assert.equal(Card.parse('Th').uint8(), 0xA1);
	assert.equal(Card.parse('Td').uint8(), 0xA2);
	assert.equal(Card.parse('Tc').uint8(), 0xA3);
	assert.equal(Card.parse('Ts').uint8(), 0xA4);
	assert.equal(Card.parse('Jh').uint8(), 0xB1);
	assert.equal(Card.parse('Jd').uint8(), 0xB2);
	assert.equal(Card.parse('Jc').uint8(), 0xB3);
	assert.equal(Card.parse('Js').uint8(), 0xB4);
	assert.equal(Card.parse('Qh').uint8(), 0xC1);
	assert.equal(Card.parse('Qd').uint8(), 0xC2);
	assert.equal(Card.parse('Qc').uint8(), 0xC3);
	assert.equal(Card.parse('Qs').uint8(), 0xC4);
	assert.equal(Card.parse('Kh').uint8(), 0xD1);
	assert.equal(Card.parse('Kd').uint8(), 0xD2);
	assert.equal(Card.parse('Kc').uint8(), 0xD3);
	assert.equal(Card.parse('Ks').uint8(), 0xD4);
	assert.equal(Card.parse('Ah').uint8(), 0xE1);
	assert.equal(Card.parse('Ad').uint8(), 0xE2);
	assert.equal(Card.parse('Ac').uint8(), 0xE3);
	assert.equal(Card.parse('As').uint8(), 0xE4);
});

Tests.add('Card.parse() long string invalid face should return null', () => {
	assert.ok(!Card.parse('Unknown of Hearts'));
	assert.ok(!Card.parse('Invalid of Diamonds'));
	assert.ok(!Card.parse('Example of Clubs'));
	assert.ok(!Card.parse('2 of Spades'));
});

Tests.add('Card.parse() long string invalid suit should return null', () => {
	assert.ok(!Card.parse('Two of Unknown'));
	assert.ok(!Card.parse('Two of Invlaid'));
	assert.ok(!Card.parse('Two of Example'));
	assert.ok(!Card.parse('Two of 1'));
});

Tests.add('Card.parse() long string valid should return card', () => {
	assert.equal(Card.parse('Two of Hearts').toString(), '2h');
	assert.equal(Card.parse('Two of Diamonds').toString(), '2d');
	assert.equal(Card.parse('Two of Clubs').toString(), '2c');
	assert.equal(Card.parse('Two of Spades').toString(), '2s');
	assert.equal(Card.parse('Three of Hearts').toString(), '3h');
	assert.equal(Card.parse('Three of Diamonds').toString(), '3d');
	assert.equal(Card.parse('Three of Clubs').toString(), '3c');
	assert.equal(Card.parse('Three of Spades').toString(), '3s');
	assert.equal(Card.parse('Four of Hearts').toString(), '4h');
	assert.equal(Card.parse('Four of Diamonds').toString(), '4d');
	assert.equal(Card.parse('Four of Clubs').toString(), '4c');
	assert.equal(Card.parse('Four of Spades').toString(), '4s');
	assert.equal(Card.parse('Five of Hearts').toString(), '5h');
	assert.equal(Card.parse('Five of Diamonds').toString(), '5d');
	assert.equal(Card.parse('Five of Clubs').toString(), '5c');
	assert.equal(Card.parse('Five of Spades').toString(), '5s');
	assert.equal(Card.parse('Six of Hearts').toString(), '6h');
	assert.equal(Card.parse('Six of Diamonds').toString(), '6d');
	assert.equal(Card.parse('Six of Clubs').toString(), '6c');
	assert.equal(Card.parse('Six of Spades').toString(), '6s');
	assert.equal(Card.parse('Seven of Hearts').toString(), '7h');
	assert.equal(Card.parse('Seven of Diamonds').toString(), '7d');
	assert.equal(Card.parse('Seven of Clubs').toString(), '7c');
	assert.equal(Card.parse('Seven of Spades').toString(), '7s');
	assert.equal(Card.parse('Eight of Hearts').toString(), '8h');
	assert.equal(Card.parse('Eight of Diamonds').toString(), '8d');
	assert.equal(Card.parse('Eight of Clubs').toString(), '8c');
	assert.equal(Card.parse('Eight of Spades').toString(), '8s');
	assert.equal(Card.parse('Nine of Hearts').toString(), '9h');
	assert.equal(Card.parse('Nine of Diamonds').toString(), '9d');
	assert.equal(Card.parse('Nine of Clubs').toString(), '9c');
	assert.equal(Card.parse('Nine of Spades').toString(), '9s');
	assert.equal(Card.parse('Ten of Hearts').toString(), 'Th');
	assert.equal(Card.parse('Ten of Diamonds').toString(), 'Td');
	assert.equal(Card.parse('Ten of Clubs').toString(), 'Tc');
	assert.equal(Card.parse('Ten of Spades').toString(), 'Ts');
	assert.equal(Card.parse('Jack of Hearts').toString(), 'Jh');
	assert.equal(Card.parse('Jack of Diamonds').toString(), 'Jd');
	assert.equal(Card.parse('Jack of Clubs').toString(), 'Jc');
	assert.equal(Card.parse('Jack of Spades').toString(), 'Js');
	assert.equal(Card.parse('Queen of Hearts').toString(), 'Qh');
	assert.equal(Card.parse('Queen of Diamonds').toString(), 'Qd');
	assert.equal(Card.parse('Queen of Clubs').toString(), 'Qc');
	assert.equal(Card.parse('Queen of Spades').toString(), 'Qs');
	assert.equal(Card.parse('King of Hearts').toString(), 'Kh');
	assert.equal(Card.parse('King of Diamonds').toString(), 'Kd');
	assert.equal(Card.parse('King of Clubs').toString(), 'Kc');
	assert.equal(Card.parse('King of Spades').toString(), 'Ks');
	assert.equal(Card.parse('Ace of Hearts').toString(), 'Ah');
	assert.equal(Card.parse('Ace of Diamonds').toString(), 'Ad');
	assert.equal(Card.parse('Ace of Clubs').toString(), 'Ac');
	assert.equal(Card.parse('Ace of Spades').toString(), 'As');
});

Tests.add('Card.sort() full deck not asc or desc should throw', () => {
	let deck = new Deck();
	let cards = deck.getCards();
	assert.throws(() => Card.sort(cards), /^Must specify a direction when sorting cards: asc or desc$/);
	assert.throws(() => Card.sort(cards, null), /^Must specify a direction when sorting cards: asc or desc$/);
	assert.throws(() => Card.sort(cards, new Object()), /^Must specify a direction when sorting cards: asc or desc$/);
	assert.throws(() => Card.sort(cards, 123), /^Must specify a direction when sorting cards: asc or desc$/);
	assert.throws(() => Card.sort(cards, ''), /^Must specify a direction when sorting cards: asc or desc$/);
	assert.throws(() => Card.sort(cards, 'unknown'), /^Must specify a direction when sorting cards: asc or desc$/);
});

Tests.add('Card.sort() full deck asc should sort', () => {
	let deck = new Deck();
	let cards = deck.getCards();
	let actual = Card.sort(cards, 'asc');
	assert.equal(actual[0].uint8(), 0x21);
	assert.equal(actual[1].uint8(), 0x22);
	assert.equal(actual[2].uint8(), 0x23);
	assert.equal(actual[3].uint8(), 0x24);
	assert.equal(actual[4].uint8(), 0x31);
	assert.equal(actual[5].uint8(), 0x32);
	assert.equal(actual[6].uint8(), 0x33);
	assert.equal(actual[7].uint8(), 0x34);
	assert.equal(actual[8].uint8(), 0x41);
	assert.equal(actual[9].uint8(), 0x42);
	assert.equal(actual[10].uint8(), 0x43);
	assert.equal(actual[11].uint8(), 0x44);
	assert.equal(actual[12].uint8(), 0x51);
	assert.equal(actual[13].uint8(), 0x52);
	assert.equal(actual[14].uint8(), 0x53);
	assert.equal(actual[15].uint8(), 0x54);
	assert.equal(actual[16].uint8(), 0x61);
	assert.equal(actual[17].uint8(), 0x62);
	assert.equal(actual[18].uint8(), 0x63);
	assert.equal(actual[19].uint8(), 0x64);
	assert.equal(actual[20].uint8(), 0x71);
	assert.equal(actual[21].uint8(), 0x72);
	assert.equal(actual[22].uint8(), 0x73);
	assert.equal(actual[23].uint8(), 0x74);
	assert.equal(actual[24].uint8(), 0x81);
	assert.equal(actual[25].uint8(), 0x82);
	assert.equal(actual[26].uint8(), 0x83);
	assert.equal(actual[27].uint8(), 0x84);
	assert.equal(actual[28].uint8(), 0x91);
	assert.equal(actual[29].uint8(), 0x92);
	assert.equal(actual[30].uint8(), 0x93);
	assert.equal(actual[31].uint8(), 0x94);
	assert.equal(actual[32].uint8(), 0xA1);
	assert.equal(actual[33].uint8(), 0xA2);
	assert.equal(actual[34].uint8(), 0xA3);
	assert.equal(actual[35].uint8(), 0xA4);
	assert.equal(actual[36].uint8(), 0xB1);
	assert.equal(actual[37].uint8(), 0xB2);
	assert.equal(actual[38].uint8(), 0xB3);
	assert.equal(actual[39].uint8(), 0xB4);
	assert.equal(actual[40].uint8(), 0xC1);
	assert.equal(actual[41].uint8(), 0xC2);
	assert.equal(actual[42].uint8(), 0xC3);
	assert.equal(actual[43].uint8(), 0xC4);
	assert.equal(actual[44].uint8(), 0xD1);
	assert.equal(actual[45].uint8(), 0xD2);
	assert.equal(actual[46].uint8(), 0xD3);
	assert.equal(actual[47].uint8(), 0xD4);
	assert.equal(actual[48].uint8(), 0xE1);
	assert.equal(actual[49].uint8(), 0xE2);
	assert.equal(actual[50].uint8(), 0xE3);
	assert.equal(actual[51].uint8(), 0xE4);
});

Tests.add('Card.sort() full deck desc should sort', () => {
	let deck = new Deck();
	let cards = deck.getCards();
	let actual = Card.sort(cards, 'desc');
	assert.equal(actual[51].uint8(), 0x21);
	assert.equal(actual[50].uint8(), 0x22);
	assert.equal(actual[49].uint8(), 0x23);
	assert.equal(actual[48].uint8(), 0x24);
	assert.equal(actual[47].uint8(), 0x31);
	assert.equal(actual[46].uint8(), 0x32);
	assert.equal(actual[45].uint8(), 0x33);
	assert.equal(actual[44].uint8(), 0x34);
	assert.equal(actual[43].uint8(), 0x41);
	assert.equal(actual[42].uint8(), 0x42);
	assert.equal(actual[41].uint8(), 0x43);
	assert.equal(actual[40].uint8(), 0x44);
	assert.equal(actual[39].uint8(), 0x51);
	assert.equal(actual[38].uint8(), 0x52);
	assert.equal(actual[37].uint8(), 0x53);
	assert.equal(actual[36].uint8(), 0x54);
	assert.equal(actual[35].uint8(), 0x61);
	assert.equal(actual[34].uint8(), 0x62);
	assert.equal(actual[33].uint8(), 0x63);
	assert.equal(actual[32].uint8(), 0x64);
	assert.equal(actual[31].uint8(), 0x71);
	assert.equal(actual[30].uint8(), 0x72);
	assert.equal(actual[29].uint8(), 0x73);
	assert.equal(actual[28].uint8(), 0x74);
	assert.equal(actual[27].uint8(), 0x81);
	assert.equal(actual[26].uint8(), 0x82);
	assert.equal(actual[25].uint8(), 0x83);
	assert.equal(actual[24].uint8(), 0x84);
	assert.equal(actual[23].uint8(), 0x91);
	assert.equal(actual[22].uint8(), 0x92);
	assert.equal(actual[21].uint8(), 0x93);
	assert.equal(actual[20].uint8(), 0x94);
	assert.equal(actual[19].uint8(), 0xA1);
	assert.equal(actual[18].uint8(), 0xA2);
	assert.equal(actual[17].uint8(), 0xA3);
	assert.equal(actual[16].uint8(), 0xA4);
	assert.equal(actual[15].uint8(), 0xB1);
	assert.equal(actual[14].uint8(), 0xB2);
	assert.equal(actual[13].uint8(), 0xB3);
	assert.equal(actual[12].uint8(), 0xB4);
	assert.equal(actual[11].uint8(), 0xC1);
	assert.equal(actual[10].uint8(), 0xC2);
	assert.equal(actual[9].uint8(), 0xC3);
	assert.equal(actual[8].uint8(), 0xC4);
	assert.equal(actual[7].uint8(), 0xD1);
	assert.equal(actual[6].uint8(), 0xD2);
	assert.equal(actual[5].uint8(), 0xD3);
	assert.equal(actual[4].uint8(), 0xD4);
	assert.equal(actual[3].uint8(), 0xE1);
	assert.equal(actual[2].uint8(), 0xE2);
	assert.equal(actual[1].uint8(), 0xE3);
	assert.equal(actual[0].uint8(), 0xE4);
});
