import { strict as assert } from 'assert';
import * as fs from 'fs';
import { Tests } from './TestFixture.mjs';
import { Enums } from '../src/Enums.mjs';
import { Card } from '../src/Card.mjs';
import { MadeHand } from '../src/MadeHand.mjs';

class SamplePlay {

	constructor(binary) {

		this.bestHandType = binary[0];
		var face1 = (binary[1] >> 4) & 0xF;
		var face2 = (binary[1] >> 4) & 0xF;
		if (face1 != 0 && face2 != 0)
			this.bestFaces = [ face1, face2 ];
		else if (face1 == 0 && face2 != 0)
			throw "Invalid";
		else if (face1 != 0)
			this.bestFaces = [ face1 ];
		else
			this.bestFaces = null;

		// Set board
		this.board = [];
		for (let i = 0; i < 5; i++) {
			let card = Card.parse(binary[i + 2]);
			if (!card) card = this.board[i];
			this.board.push(card);
		}

		// Set pocket
		this.pocket = [];
		for (let i = 0; i < 2; i++) {
			this.pocket.push(Card.parse(binary[i + 7]));
		}

		// Set result
		this.result = [];
		for (let i = 0; i < 5; i++) {
			let card = Card.parse(binary[i + 9]);
			if (!card) card = this.result[i]
			this.result.push(card);
		}

		// Set won
		this.won = String.fromCharCode(binary[14]);
	}

	winnerSpecified() {
		return this.won == 'y' || this.won == 'n';
	}

	isComplete() {
		return this.board != null
				&& this.pocket != null
				&& this.result != null
				&& this.board.length == 5
				&& this.pocket.length == 2
				&& this.result.length == 5
				&& this.board[0].isValid()
				&& this.board[1].isValid()
				&& this.board[2].isValid()
				&& this.board[3].isValid()
				&& this.board[4].isValid()
				&& this.pocket[0].isValid()
				&& this.pocket[1].isValid()
				&& this.result[0].isValid()
				&& this.result[1].isValid()
				&& this.result[2].isValid()
				&& this.result[3].isValid()
				&& this.result[4].isValid();
	}
}

let binaryInt32 = Buffer.alloc(4);
function readInt32(file) {
	if (4 != fs.readSync(file, binaryInt32, 0, 4, null))
		throw 'Failed to read in32';
	else
		return binaryInt32.readInt32LE(0);
}

let binaryHand = Buffer.alloc(15);
function readHand(file) {
	let plays = [];
	let countOfPlayers = readInt32(file);
	for (let iPlayer = 0; iPlayer < countOfPlayers; iPlayer++) {
		if (15 != fs.readSync(file, binaryHand, 0, 15, null))
			throw 'Failed to read hand';
		plays.push(new SamplePlay(binaryHand));
	}
	return plays;
}

Tests.add('MadeHands from file tests', () => {

	let file = fs.openSync('../CSharp/Gravical.Poker.Core.Tests/PokerPlays.bin', 'r');
	let countOfHands = readInt32(file);
	let countOfHandsMatched = 0;
	let maxPlayersInHand = 0;

	for (let iHand = 0; iHand < countOfHands; iHand++) {

		let plays = readHand(file);

		// If we need to know exact winners and losers, don't keep anything that isn't an accurate record
		let countNoWinner = plays.reduce((count, play) => count + (play.winnerSpecified() ? 0 : 1), 0);
		let countWon = plays.reduce((count, play) => count + (play.won == 'y' ? 1 : 0), 0);
		if (countNoWinner > 0 || countWon != 1) continue;

		// Tally up some numbers
		countOfHandsMatched++;
		if (plays.length > maxPlayersInHand) maxPlayersInHand = plays.length;

		// Test this hand
		let expectedWinner = null;
		let actualWinner = null;
		let madePlays = [];

		// Run every player's hand
		for (let iPlayer = 0; iPlayer < plays.length; iPlayer++) {
			var play = plays[iPlayer];
			var cardsPlayed = play.pocket.concat(play.board).filter(_ => _.isValid());

			var made = MadeHand.makeHand(cardsPlayed);
			madePlays.push(made);

			if (play.bestHandType != made.type) throw `Failed on ${iHand}.${iPlayer} due to wrong hand type expected "${Enums.HandTypes.toString(play.bestHandType)}" vs actual "${made.toString()}" from ${Card.toCardsString(cardsPlayed)}`;
			if (play.won == 'y' && expectedWinner != null) throw `Failed on ${iHand}.${iPlayer} due to wrong winner`;
			if (play.won == 'y') expectedWinner = iPlayer;

			play.Result = made.played;

			if (actualWinner == null || madePlays[actualWinner].score < made.score)
				actualWinner = iPlayer;
		}
	}

	fs.closeSync(file);

	console.log('countOfHandsMatched', countOfHandsMatched);
	console.log('maxPlayersInHand', maxPlayersInHand);
});