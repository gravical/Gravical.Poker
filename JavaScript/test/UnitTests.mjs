import { Random } from "../src/Random.mjs"
import { Tests } from './TestFixture.mjs';
import * as EnumTests from "./EnumsTests.mjs"
import * as CardTests from "./CardTests.mjs"
import * as DeckTests from "./DeckTests.mjs"
import * as HandGroupingTests from "./HandGroupingTests.mjs"
import * as PocketTests from "./PocketTests.mjs"
import * as TableFinalTests from "./TableFinalTests.mjs"
import * as MadeHandTests from "./MadeHandTests.mjs"
import * as MadeHandsFromFileTests from "./MadeHandsFromFileTests.mjs"

// Because window.crypto isn't available in node and not really required for unit tests
Random.setAllowNonCryptographic(true);

Tests.run();
