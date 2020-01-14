using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Gravical.Poker.Core.Tests
{
    [ExcludeFromCodeCoverage]
    public class SampleHand
    {
        public List<SamplePlay> Plays { get; } = new List<SamplePlay>();

        public static IEnumerable<SampleHand> Load(string filename, bool validWinsOnly)
        {
            const int sizeeach = 15;
            var buffer = new byte[sizeeach];

            using (var input = new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
            {
                var nHands = input.ReadInt32();

                for (int iHand = 0; iHand < nHands; iHand++)
                {
                    var nPlayers = input.ReadInt32();
                    var hand = new SampleHand();

                    for (int iPlayer = 0; iPlayer < nPlayers; iPlayer++)
                    {
                        input.Read(buffer, 0, sizeeach);
                        hand.Plays.Add(new SamplePlay(buffer));
                    }

                    // If we need to know exact winners and losers, don't keep anything that isn't an accurate record
                    if (validWinsOnly)
                    {
                        if (hand.Plays.Any(_ => !_.WinnerSpecified)) continue;
                        if (hand.Plays.Count(_ => _.Won == 'y') != 1) continue;
                    }

                    yield return hand;
                }
            }
        }
    }
}