import { strict as assert } from 'assert';
import * as colors from "colors";
import * as process from "process";

const MS_PER_SEC = 1e6;

export class TestResult {
	constructor(name) {
		this.name = name;
		this.success = false;
		this.exception = null;
		this.started = BigInt(0);
		this.finished = BigInt(0);
		this.duration = 0;
	}

	start() {
		this.started = process.hrtime.bigint();
	}

	stop(exception) {
		this.finished = process.hrtime.bigint();
		this.duration = Number(this.finished - this.started) / MS_PER_SEC;
		this.success = !exception;
		this.exception = exception;
	}

	report() {
		console.log('TEST: '.green + this.name.yellow + '...'.green);
		if (this.success)
			console.log("\tSUCCESS".green, `${this.duration} ms`);
		else
			console.log("\tFAILED".red, this.exception);
	}
}

export class Tests {

	static add(name, method) {
		if(Tests.map.has(name)) assert.fail('Duplicate test name.');
		Tests.map.set(name, method);
	}

	static run() {

		let args = process.argv.slice(2);
		for (const arg of args) {
			if (arg == "--terse") {
				Tests.terse = true;
			}
		}

		let results = [];
		let start = process.hrtime.bigint();

		for(const [name, method] of Tests.map.entries()) {

			let result = new TestResult(name);
			results.push(result);

			try {
				result.start();
				method();
				result.stop();
			} catch(ex) {
				result.stop(ex);
			}

			if (!Tests.terse || !result.success) {
				result.report();
			}
		}

		let duration = Number(process.hrtime.bigint() - start) / MS_PER_SEC;

		let success = results.reduce((n, result) => n + (result.success ? 1 : 0), 0);
		let failures = results.filter(result => !result.success);
		
		console.log(`${success} tests succeeded.`.green, `${duration} ms`);

		if(failures.length > 0) {
			console.log(`${failures.length} tests failed.`.red);
			for (const result of failures) {
				console.log(`\t${result.name}`.red);
			}
		}
	}
}

Tests.map = new Map();
Tests.terse = true;
