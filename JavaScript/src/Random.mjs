export class Random {

	static setAllowNonCryptographic(allowNonCryptographic = false) {
		Random.allowNonCryptographic = allowNonCryptographic;
	}

	static getUint32Array(length) {

		let values = new Uint32Array(length);

		// Try this using the browser
		let windowCrypto = Random.tryGetWindowCrypto();
		if (windowCrypto) {
			windowCrypto.getRandomValues(values);
			return values;
		}

		// The default when we dont have cryptographically random is to fail
		if (!Random.allowNonCryptographic) {
			throw 'No cryptographic random number generator is available.';
		}

		// Use non-cryptographic random numbers
		for (let i = 0; i < length; i++) {
			values[i] = (Math.random() * 0xFFFFFFFF) % 0x100;
		}
		return values;
	}

	static tryGetWindowCrypto() {
		if (typeof(window) !== 'undefined'
			&& typeof(window.crypto) !== 'undefined'
			&& typeof(window.crypto.getRandomValues) !== 'undefined') {
			return window.crypto;
		}
	}
}

Random.allowNonCryptographic = false;
