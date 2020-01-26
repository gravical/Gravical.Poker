using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


namespace Gravical.Poker.Core.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PokerExtensionsTests : TestBase
    {
        private Card SimpleCard = new Card(Face.Two, Suit.Heart);

        private Card[] SimpleCards = new[]
        {
            new Card(Face.Two, Suit.Heart),
            new Card(Face.Jack, Suit.Spade),
            new Card(Face.Ace, Suit.Club),
            new Card(Face.Seven, Suit.Diamond),
            new Card(Face.Seven, Suit.Spade),
        };

        #region  SuitIsValid

        [TestMethod]
        public void SuitIsValid_Negative_ShouldReturnFalse()
        {
            Assert.IsFalse(((Suit)(-1)).IsValid());
        }

        [TestMethod]
        public void SuitIsValid_Zero_ShouldReturnFalse()
        {
            Assert.IsFalse(((Suit)0).IsValid());
        }

        [TestMethod]
        public void SuitIsValid_TooLarge_ShouldReturnFalse()
        {
            Assert.IsFalse(((Suit)5).IsValid());
        }

        [TestMethod]
        public void SuitIsValid_Valid_ShouldReturnTrue()
        {
            Assert.IsTrue(Suit.Heart.IsValid());
        }

        #endregion

        #region  FaceIsValid

        [TestMethod]
        public void FaceIsValid_Negative_ShouldReturnFalse()
        {
            Assert.IsFalse(((Face)(-1)).IsValid());
        }

        [TestMethod]
        public void FaceIsValid_Zero_ShouldReturnFalse()
        {
            Assert.IsFalse(((Face)0).IsValid());
        }

        [TestMethod]
        public void FaceIsValid_TooSmall_ShouldReturnFalse()
        {
            Assert.IsFalse(((Face)1).IsValid());
        }

        [TestMethod]
        public void FaceIsValid_TooLarge_ShouldReturnFalse()
        {
            Assert.IsFalse(((Face)15).IsValid());
        }

        [TestMethod]
        public void FaceIsValid_Valid_ShouldReturnTrue()
        {
            Assert.IsTrue(Face.Two.IsValid());
        }

        #endregion

        #region  TableStatusIsValid

        [TestMethod]
        public void TableStatusIsValid_Negative_ShouldReturnFalse()
        {
            Assert.IsFalse(((TableStatus)(-1)).IsValid());
        }

        [TestMethod]
        public void TableStatusIsValid_Zero_ShouldReturnFalse()
        {
            Assert.IsFalse(((TableStatus)0).IsValid());
        }

        [TestMethod]
        public void TableStatusIsValid_TooLarge_ShouldReturnFalse()
        {
            Assert.IsFalse(((TableStatus)5).IsValid());
        }

        [TestMethod]
        public void TableStatusIsValid_Valid_ShouldReturnTrue()
        {
            Assert.IsTrue(TableStatus.BeforeFlop.IsValid());
        }

        #endregion

        #region  HandTypesIsValid

        [TestMethod]
        public void HandTypesIsValid_Negative_ShouldReturnFalse()
        {
            Assert.IsFalse(((HandTypes)(-1)).IsValid());
        }

        [TestMethod]
        public void HandTypesIsValid_Zero_ShouldReturnFalse()
        {
            Assert.IsFalse(((HandTypes)0).IsValid());
        }

        [TestMethod]
        public void HandTypesIsValid_TooLarge_ShouldReturnFalse()
        {
            Assert.IsFalse(((HandTypes)11).IsValid());
        }

        [TestMethod]
        public void HandTypesIsValid_Valid_ShouldReturnTrue()
        {
            Assert.IsTrue(HandTypes.Flush.IsValid());
        }

        #endregion

        #region CardAnyIntersection

        [TestMethod]
        public void CardAnyIntersectionSingle_NullWithin_ShouldReturnFalse()
        {
            Assert.IsFalse(SimpleCard.AnyIntersection(null));
        }

        [TestMethod]
        public void CardAnyIntersectionSingle_EmptyWithin_ShouldReturnFalse()
        {
            Assert.IsFalse(SimpleCard.AnyIntersection(new Card[0]));
        }

        [TestMethod]
        public void CardAnyIntersectionSingle_NotWithin_ShouldReturnFalse()
        {
            Assert.IsFalse(SimpleCard.AnyIntersection(new Card(Face.Three, Suit.Heart), new Card(Face.Two, Suit.Club)));
        }

        [TestMethod]
        public void CardAnyIntersectionSingle_IsWithin_ShouldReturnTrue()
        {
            Assert.IsTrue(SimpleCard.AnyIntersection(new Card(Face.Three, Suit.Heart), new Card(Face.Two, Suit.Heart)));
        }

        [TestMethod]
        public void CardAnyIntersectionMultiple_NullCards_ShouldReturnFalse()
        {
            Assert.IsFalse((null as IEnumerable<Card>).AnyIntersection(null));
        }

        [TestMethod]
        public void CardAnyIntersectionMultiple_NullWithin_ShouldReturnFalse()
        {
            Assert.IsFalse(SimpleCards.AnyIntersection(null));
        }

        [TestMethod]
        public void CardAnyIntersectionMultiple_EmptyWithin_ShouldReturnFalse()
        {
            Assert.IsFalse(SimpleCards.AnyIntersection(new Card[0]));
        }

        [TestMethod]
        public void CardAnyIntersectionMultiple_NotWithin_ShouldReturnFalse()
        {
            Assert.IsFalse(SimpleCards.AnyIntersection(new Card(Face.Three, Suit.Heart), new Card(Face.Two, Suit.Club)));
        }

        [TestMethod]
        public void CardAnyIntersectionMultiple_IsWithin_ShouldReturnTrue()
        {
            Assert.IsTrue(SimpleCards.AnyIntersection(new Card(Face.Three, Suit.Heart), new Card(Face.Two, Suit.Heart)));
        }

        #endregion

        #region CardsSort

        [TestMethod]
        public void CardsSortByFace_Null_ShouldReturnNull()
        {
            Assert.IsNull((null as Card[]).SortByFace());
        }

        [TestMethod]
        public void CardsSortByFace_Sort_ShouldSortByFaceThenSuit()
        {
            var actual = SimpleCards.SortByFace();
            Assert.AreEqual(5, actual.Length);
            Assert.AreEqual(new Card(Face.Ace, Suit.Club), actual[0]);
            Assert.AreEqual(new Card(Face.Jack, Suit.Spade), actual[1]);
            Assert.AreEqual(new Card(Face.Seven, Suit.Spade), actual[2]);
            Assert.AreEqual(new Card(Face.Seven, Suit.Diamond), actual[3]);
            Assert.AreEqual(new Card(Face.Two, Suit.Heart), actual[4]);
        }

        #endregion

        #region ParseCardSuit

        [TestMethod]
        public void ParseCardSuit_Null_ShouldReturnNull()
        {
            Assert.IsNull((null as string).ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_Empty_ShouldReturnNull()
        {
            Assert.IsNull(string.Empty.ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_Nonsense_ShouldReturnNull()
        {
            Assert.IsNull("1".ParseCardSuit());
            Assert.IsNull("*".ParseCardSuit());
            Assert.IsNull("abc".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_h_ShouldReturnHearts()
        {
            Assert.AreEqual(Suit.Heart, "h".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_H_ShouldReturnHearts()
        {
            Assert.AreEqual(Suit.Heart, "H".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_heart_ShouldReturnHearts()
        {
            Assert.AreEqual(Suit.Heart, "heart".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_Heart_ShouldReturnHearts()
        {
            Assert.AreEqual(Suit.Heart, "Heart".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_HEART_ShouldReturnHearts()
        {
            Assert.AreEqual(Suit.Heart, "HEART".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_HEARTS_ShouldReturnHearts()
        {
            Assert.AreEqual(Suit.Heart, "HEARTS".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_d_ShouldReturnDiamonds()
        {
            Assert.AreEqual(Suit.Diamond, "d".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_D_ShouldReturnDiamonds()
        {
            Assert.AreEqual(Suit.Diamond, "D".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_diamond_ShouldReturnDiamonds()
        {
            Assert.AreEqual(Suit.Diamond, "diamond".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_Diamond_ShouldReturnDiamonds()
        {
            Assert.AreEqual(Suit.Diamond, "Diamond".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_DIAMOND_ShouldReturnDiamonds()
        {
            Assert.AreEqual(Suit.Diamond, "DIAMOND".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_DIAMONDS_ShouldReturnDiamonds()
        {
            Assert.AreEqual(Suit.Diamond, "DIAMONDS".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_c_ShouldReturnClubs()
        {
            Assert.AreEqual(Suit.Club, "c".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_C_ShouldReturnClubs()
        {
            Assert.AreEqual(Suit.Club, "C".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_club_ShouldReturnClubs()
        {
            Assert.AreEqual(Suit.Club, "club".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_Club_ShouldReturnClubs()
        {
            Assert.AreEqual(Suit.Club, "Club".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_CLUB_ShouldReturnClubs()
        {
            Assert.AreEqual(Suit.Club, "CLUB".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_CLUBS_ShouldReturnClubs()
        {
            Assert.AreEqual(Suit.Club, "CLUBS".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_s_ShouldReturnSpades()
        {
            Assert.AreEqual(Suit.Spade, "s".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_S_ShouldReturnSpades()
        {
            Assert.AreEqual(Suit.Spade, "S".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_spade_ShouldReturnSpades()
        {
            Assert.AreEqual(Suit.Spade, "spade".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_Spade_ShouldReturnSpades()
        {
            Assert.AreEqual(Suit.Spade, "Spade".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_SPADE_ShouldReturnSpades()
        {
            Assert.AreEqual(Suit.Spade, "SPADE".ParseCardSuit());
        }

        [TestMethod]
        public void ParseCardSuit_SPADES_ShouldReturnSpades()
        {
            Assert.AreEqual(Suit.Spade, "SPADES".ParseCardSuit());
        }

        #endregion

        #region ParseCardFace

        [TestMethod]
        public void ParseCardFace_Null_ShouldReturnNull()
        {
            Assert.IsNull((null as string).ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_Empty_ShouldReturnNull()
        {
            Assert.IsNull(string.Empty.ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_Nonsense_ShouldReturnNull()
        {
            Assert.IsNull("1".ParseCardFace());
            Assert.IsNull("*".ParseCardFace());
            Assert.IsNull("abc".ParseCardFace());
        }

        // two
        [TestMethod]
        public void ParseCardFace_2_ShouldReturnTwo()
        {
            Assert.AreEqual(Face.Two, "2".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_two_ShouldReturnTwo()
        {
            Assert.AreEqual(Face.Two, "two".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_Two_ShouldReturnTwo()
        {
            Assert.AreEqual(Face.Two, "Two".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_TWO_ShouldReturnTwo()
        {
            Assert.AreEqual(Face.Two, "TWO".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_TWOS_ShouldReturnTwo()
        {
            Assert.AreEqual(Face.Two, "TWOS".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_deuce_ShouldReturnTwo()
        {
            Assert.AreEqual(Face.Two, "deuce".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_Deuce_ShouldReturnTwo()
        {
            Assert.AreEqual(Face.Two, "Deuce".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_DEUCE_ShouldReturnTwo()
        {
            Assert.AreEqual(Face.Two, "DEUCE".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_DEUCES_ShouldReturnTwo()
        {
            Assert.AreEqual(Face.Two, "DEUCES".ParseCardFace());
        }

        // three
        [TestMethod]
        public void ParseCardFace_3_ShouldReturnThree()
        {
            Assert.AreEqual(Face.Three, "3".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_three_ShouldReturnThree()
        {
            Assert.AreEqual(Face.Three, "three".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_Three_ShouldReturnThree()
        {
            Assert.AreEqual(Face.Three, "Three".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_THREE_ShouldReturnThree()
        {
            Assert.AreEqual(Face.Three, "THREE".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_THREES_ShouldReturnThree()
        {
            Assert.AreEqual(Face.Three, "THREES".ParseCardFace());
        }

        // four
        [TestMethod]
        public void ParseCardFace_4_ShouldReturnFour()
        {
            Assert.AreEqual(Face.Four, "4".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_four_ShouldReturnFour()
        {
            Assert.AreEqual(Face.Four, "four".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_Four_ShouldReturnFour()
        {
            Assert.AreEqual(Face.Four, "Four".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_FOUR_ShouldReturnFour()
        {
            Assert.AreEqual(Face.Four, "FOUR".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_FOURS_ShouldReturnFour()
        {
            Assert.AreEqual(Face.Four, "FOURS".ParseCardFace());
        }

        // five
        [TestMethod]
        public void ParseCardFace_5_ShouldReturnFive()
        {
            Assert.AreEqual(Face.Five, "5".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_five_ShouldReturnFive()
        {
            Assert.AreEqual(Face.Five, "five".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_Five_ShouldReturnFive()
        {
            Assert.AreEqual(Face.Five, "Five".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_FIVE_ShouldReturnFive()
        {
            Assert.AreEqual(Face.Five, "FIVE".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_FIVES_ShouldReturnFive()
        {
            Assert.AreEqual(Face.Five, "FIVES".ParseCardFace());
        }

        // six
        [TestMethod]
        public void ParseCardFace_6_ShouldReturnSix()
        {
            Assert.AreEqual(Face.Six, "6".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_six_ShouldReturnSix()
        {
            Assert.AreEqual(Face.Six, "six".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_Six_ShouldReturnSix()
        {
            Assert.AreEqual(Face.Six, "Six".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_SIX_ShouldReturnSix()
        {
            Assert.AreEqual(Face.Six, "SIX".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_SIXES_ShouldReturnSix()
        {
            Assert.AreEqual(Face.Six, "SIXES".ParseCardFace());
        }

        // seven
        [TestMethod]
        public void ParseCardFace_7_ShouldReturnSeven()
        {
            Assert.AreEqual(Face.Seven, "7".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_seven_ShouldReturnSeven()
        {
            Assert.AreEqual(Face.Seven, "seven".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_Seven_ShouldReturnSeven()
        {
            Assert.AreEqual(Face.Seven, "Seven".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_SEVEN_ShouldReturnSeven()
        {
            Assert.AreEqual(Face.Seven, "SEVEN".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_SEVENS_ShouldReturnSeven()
        {
            Assert.AreEqual(Face.Seven, "SEVENS".ParseCardFace());
        }

        // eight
        [TestMethod]
        public void ParseCardFace_8_ShouldReturnEight()
        {
            Assert.AreEqual(Face.Eight, "8".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_eight_ShouldReturnEight()
        {
            Assert.AreEqual(Face.Eight, "eight".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_Eight_ShouldReturnEight()
        {
            Assert.AreEqual(Face.Eight, "Eight".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_EIGHT_ShouldReturnEight()
        {
            Assert.AreEqual(Face.Eight, "EIGHT".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_EIGHTS_ShouldReturnEight()
        {
            Assert.AreEqual(Face.Eight, "EIGHTS".ParseCardFace());
        }

        // nine
        [TestMethod]
        public void ParseCardFace_9_ShouldReturnNine()
        {
            Assert.AreEqual(Face.Nine, "9".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_nine_ShouldReturnNine()
        {
            Assert.AreEqual(Face.Nine, "nine".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_Nine_ShouldReturnNine()
        {
            Assert.AreEqual(Face.Nine, "Nine".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_NINE_ShouldReturnNine()
        {
            Assert.AreEqual(Face.Nine, "NINE".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_NINES_ShouldReturnNine()
        {
            Assert.AreEqual(Face.Nine, "NINES".ParseCardFace());
        }

        // ten
        [TestMethod]
        public void ParseCardFace_10_ShouldReturnTen()
        {
            Assert.AreEqual(Face.Ten, "10".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_t_ShouldReturnTen()
        {
            Assert.AreEqual(Face.Ten, "t".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_T_ShouldReturnTen()
        {
            Assert.AreEqual(Face.Ten, "T".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_ten_ShouldReturnTen()
        {
            Assert.AreEqual(Face.Ten, "ten".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_Ten_ShouldReturnTen()
        {
            Assert.AreEqual(Face.Ten, "Ten".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_TEN_ShouldReturnTen()
        {
            Assert.AreEqual(Face.Ten, "TEN".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_TENS_ShouldReturnTen()
        {
            Assert.AreEqual(Face.Ten, "TENS".ParseCardFace());
        }

        // jack
        [TestMethod]
        public void ParseCardFace_j_ShouldReturnJack()
        {
            Assert.AreEqual(Face.Jack, "j".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_J_ShouldReturnJack()
        {
            Assert.AreEqual(Face.Jack, "J".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_jack_ShouldReturnJack()
        {
            Assert.AreEqual(Face.Jack, "jack".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_Jack_ShouldReturnJack()
        {
            Assert.AreEqual(Face.Jack, "Jack".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_JACK_ShouldReturnJack()
        {
            Assert.AreEqual(Face.Jack, "JACK".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_JACKS_ShouldReturnJack()
        {
            Assert.AreEqual(Face.Jack, "JACKS".ParseCardFace());
        }

        // queen
        [TestMethod]
        public void ParseCardFace_q_ShouldReturnQueen()
        {
            Assert.AreEqual(Face.Queen, "q".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_Q_ShouldReturnQueen()
        {
            Assert.AreEqual(Face.Queen, "Q".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_queen_ShouldReturnQueen()
        {
            Assert.AreEqual(Face.Queen, "queen".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_Queen_ShouldReturnQueen()
        {
            Assert.AreEqual(Face.Queen, "Queen".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_QUEEN_ShouldReturnQueen()
        {
            Assert.AreEqual(Face.Queen, "QUEEN".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_QUEENS_ShouldReturnQueen()
        {
            Assert.AreEqual(Face.Queen, "QUEENS".ParseCardFace());
        }

        // king
        [TestMethod]
        public void ParseCardFace_k_ShouldReturnKing()
        {
            Assert.AreEqual(Face.King, "k".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_K_ShouldReturnKing()
        {
            Assert.AreEqual(Face.King, "K".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_king_ShouldReturnKing()
        {
            Assert.AreEqual(Face.King, "king".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_King_ShouldReturnKing()
        {
            Assert.AreEqual(Face.King, "King".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_KING_ShouldReturnKing()
        {
            Assert.AreEqual(Face.King, "KING".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_KINGS_ShouldReturnKing()
        {
            Assert.AreEqual(Face.King, "KINGS".ParseCardFace());
        }

        // ace
        [TestMethod]
        public void ParseCardFace_a_ShouldReturnAce()
        {
            Assert.AreEqual(Face.Ace, "a".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_A_ShouldReturnAce()
        {
            Assert.AreEqual(Face.Ace, "A".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_ace_ShouldReturnAce()
        {
            Assert.AreEqual(Face.Ace, "ace".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_Ace_ShouldReturnAce()
        {
            Assert.AreEqual(Face.Ace, "Ace".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_ACE_ShouldReturnAce()
        {
            Assert.AreEqual(Face.Ace, "ACE".ParseCardFace());
        }

        [TestMethod]
        public void ParseCardFace_ACES_ShouldReturnAce()
        {
            Assert.AreEqual(Face.Ace, "ACES".ParseCardFace());
        }

        #endregion

        #region ParseCardsFromString

        [TestMethod]
        public void ParseCardsFromString_Null_ShouldReturnNull()
        {
            Assert.IsNull((null as string).ParseCards());
        }

        [TestMethod]
        public void ParseCardsFromString_Empty_ShouldReturnEmpty()
        {
            Assert.AreEqual(0, string.Empty.ParseCards().Length);
        }

        [TestMethod]
        public void ParseCardsFromString_InvalidCards_ShouldThrowException()
        {
            ActExpectingArgumentException("from", "One or more cards could not be parsed", () => "Xh 9z".ParseCards());
        }

        [TestMethod]
        public void ParseCardsFromString_InvalidAndValidCards_ShouldThrowException()
        {
            ActExpectingArgumentException("from", "One or more cards could not be parsed", () => "2h 3c Xh 9z As".ParseCards());
        }

        [TestMethod]
        public void ParseCardsFromString_ValidSingleCard_ShouldReturnIt()
        {
            var arrange = "2h";
            var actual = arrange.ParseCards();
            Assert.AreEqual(arrange, actual.ToCardString());
        }

        [TestMethod]
        public void ParseCardsFromString_ValidMultipleCards_ShouldReturnThem()
        {
            var arrange = "2h 3d 4c 5d";
            var actual = arrange.ParseCards();
            Assert.AreEqual(arrange, actual.ToCardString());
        }

        #endregion

        #region ParseCardFromString

        [TestMethod]
        public void ParseCardFromString_Null_ShouldReturnNull()
        {
            Assert.IsNull((null as string).ParseCard());
        }

        [TestMethod]
        public void ParseCardFromString_Empty_ShouldReturnNull()
        {
            Assert.IsNull(string.Empty.ParseCard());
        }

        [TestMethod]
        public void ParseCardFromString_Nonsense_ShouldReturnNull()
        {
            Assert.IsNull("1".ParseCard());
            Assert.IsNull("*".ParseCard());
            Assert.IsNull("abc".ParseCard());
        }

        [TestMethod]
        public void ParseCardFromString_ShortInvalidFace_ShouldReturnNull()
        {
            Assert.IsNull("Xs".ParseCard());
        }

        [TestMethod]
        public void ParseCardFromString_ShortInvalidSuit_ShouldReturnNull()
        {
            Assert.IsNull("Ax".ParseCard());
        }

        [TestMethod]
        public void ParseCardFromString_ShortValid_ShouldReturnCard()
        {
            Assert.AreEqual(new Card(Face.Two, Suit.Heart), "2h".ParseCard());
            Assert.AreEqual(new Card(Face.Seven, Suit.Club), "7c".ParseCard());
            Assert.AreEqual(new Card(Face.Jack, Suit.Diamond), "Jd".ParseCard());
            Assert.AreEqual(new Card(Face.Ace, Suit.Spade), "As".ParseCard());
        }

        [TestMethod]
        public void ParseCardFromString_LongInvalidFace_ShouldReturnNull()
        {
            Assert.IsNull("Xface of Spade".ParseCard());
        }

        [TestMethod]
        public void ParseCardFromString_LongInvalidSuit_ShouldReturnNull()
        {
            Assert.IsNull("Ace of Xsuit".ParseCard());
        }

        [TestMethod]
        public void ParseCardFromString_LongValid_ShouldReturnCard()
        {
            Assert.AreEqual(new Card(Face.Two, Suit.Heart), "Two of Heart".ParseCard());
            Assert.AreEqual(new Card(Face.Seven, Suit.Club), "Seven of Club".ParseCard());
            Assert.AreEqual(new Card(Face.Jack, Suit.Diamond), "Jack of Diamond".ParseCard());
            Assert.AreEqual(new Card(Face.Ace, Suit.Spade), "Ace of Spade".ParseCard());
        }

        #endregion

        #region ParseCardFromByte

        [TestMethod]
        public void ParseCardFromByte_Zero_ShouldReturnNull()
        {
            Assert.IsNull(((byte)0).ParseCard());
        }

        [TestMethod]
        public void ParseCardFromByte_ZeroFace_ShouldReturnNull()
        {
            Assert.IsNull(((byte)(1)).ParseCard());
        }

        [TestMethod]
        public void ParseCardFromByte_ZeroSuit_ShouldReturnNull()
        {
            Assert.IsNull(((byte)(2 << 4)).ParseCard());
        }

        [TestMethod]
        public void ParseCardFromByte_TooSmallFace_ShouldReturnNull()
        {
            Assert.IsNull(((byte)((1 << 4) | 1)).ParseCard());
        }

        [TestMethod]
        public void ParseCardFromByte_TooLargeFace_ShouldReturnNull()
        {
            Assert.IsNull(((byte)((15 << 4) | 1)).ParseCard());
        }

        [TestMethod]
        public void ParseCardFromByte_TooLargeSuit_ShouldReturnNull()
        {
            Assert.IsNull(((byte)((2 << 4) | 5)).ParseCard());
        }

        [TestMethod]
        public void ParseCardFromByte_Valid_ShouldReturnCard()
        {
            Assert.IsTrue(((byte)((2 << 4) | 1)).ParseCard().Value.IsValid);
        }

        #endregion

        #region ToCardsString

        [TestMethod]
        public void ToCardString_Null_ShouldReturnNull()
        {
            Assert.IsNull((null as IEnumerable<Card>).ToCardString());
        }

        [TestMethod]
        public void ToCardString_Empty_ShouldReturnEmpty()
        {
            Assert.AreEqual(string.Empty, new Card[0].ToCardString());
        }

        [TestMethod]
        public void ToCardString_Single_ShouldReturnSingle()
        {
            Assert.AreEqual("As", new[] { new Card(Face.Ace, Suit.Spade) }.ToCardString());
        }

        [TestMethod]
        public void ToCardString_Multiple_ShouldReturnSpaced()
        {
            Assert.AreEqual("2c 7h As", new[]
            {
                new Card(Face.Two, Suit.Club),
                new Card(Face.Seven, Suit.Heart),
                new Card(Face.Ace, Suit.Spade)
            }.ToCardString());
        }

        #endregion
    }
}