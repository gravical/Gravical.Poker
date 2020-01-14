using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;


namespace Gravical.Poker.Core.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class MadeHandSamplePlayTests : TestBase
    {
        [TestMethod]
        public void SamplePlays_Pure_ShouldMatchResults()
        {
            var counts = new CountsBy<X>();

            Parallel.ForEach(SampleHand.Load(@"PokerPlays.bin", true), hand =>
            {
                int? expectedWinner = null;
                int? actualWinner = null;
                var madePlays = new List<MadeHand>();

                // Run every player's hand
                for (int iPlayer = 0; iPlayer < hand.Plays.Count; iPlayer++)
                {
                    var play = hand.Plays[iPlayer];

                    var cards = play.Pocket.Concat(play.Board).Where(_ => _.IsValid).ToArray();

                    var made = MadeHand.MakeHand(cards);
                    madePlays.Add(made);

                    if (play.BestHandType != made.Type) throw new Exception("Failed");
                    if (play.Won == 'y' && expectedWinner != null) throw new Exception("Failed");
                    if (play.Won == 'y') expectedWinner = iPlayer;

                    play.Result = made.Played;

                    if (actualWinner == null || madePlays[actualWinner.Value].Score < made.Score)
                        actualWinner = iPlayer;
                }

                var match = expectedWinner == actualWinner;
                counts.Increment(X.X, match);
            });

            counts.Report();
            Assert.IsTrue(counts.Success, $"Some failed");
        }

        private enum X
        {
            X
        }

        private class CountsBy<T>
        {
            private readonly Dictionary<T, int> success = new Dictionary<T, int>();
            private readonly Dictionary<T, int> failure = new Dictionary<T, int>();

            public bool Success => failure.Sum(_ => _.Value) == 0;

            public CountsBy()
            {
                foreach (var type in Enum.GetValues(typeof(T)).Cast<T>())
                {
                    success.Add(type, 0);
                    failure.Add(type, 0);
                }
            }

            public void Increment(T type, bool condition)
            {
                if (condition)
                    success[type]++;
                else
                    failure[type]++;
            }

            public void Report()
            {
                Console.WriteLine($"Total tests: {success.Sum(_ => _.Value) + failure.Sum(_ => _.Value):#,0}");
                Report("Success", success);
                Report("Failure", failure);
            }

            private void Report(string key, Dictionary<T, int> counts)
            {
                Console.WriteLine($"{key}:");
                if (counts.Values.All(_ => _ == 0))
                {
                    Console.WriteLine("\tNONE");
                    return;
                }
                foreach (var count in counts.Where(_ => _.Value > 0))
                {
                    Console.WriteLine($"\t{count.Key}: {count.Value:#,0}");
                }
            }
        }

        private bool Match(Card[] expected, MadeHand actual)
        {
            Guards.OperationSuccess(expected.Length == actual.Played.Length, "Impossible match");

            var expectedSorted = expected.SortByFace();
            var actualSorted = actual.Played.SortByFace();

            // Run each position
            for (int i = 0; i < expectedSorted.Length; i++)
            {
                var expect = expectedSorted[i];

                // If it is not a match in the hand
                if (!expect.Equals(actualSorted[i]))
                {
                    // Check to see if it is an alternate
                    if (!(actual.Alternates?.ContainsKey(actualSorted[i]) ?? false)) return false;
                    if (!actual.Alternates[actualSorted[i]].Any(_ => _.Equals(expect))) return false;
                }
            }

            return true;
        }
    }
}