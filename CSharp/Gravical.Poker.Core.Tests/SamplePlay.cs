using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Gravical.Poker.Core.Tests
{
    [ExcludeFromCodeCoverage]
    public class SamplePlay
    {
        public HandTypes BestHandType { get; set; }
        public Face[] BestFaces { get; set; }
        public Card[] Board { get; set; }
        public Card[] Pocket { get; set; }
        public Card[] Result { get; set; }
        public char Won { get; set; }

        public SamplePlay(byte[] binary)
        {
            if (binary == null || binary.Length != 15) throw new Exception("Invalid binary PlayerInfo");

            // Set best hand
            BestHandType = (HandTypes)binary[0];
            var face1 = (binary[1] >> 4) & 0xF;
            var face2 = (binary[1] >> 4) & 0xF;
            if (face1 != 0 && face2 != 0)
                BestFaces = new[] { (Face)face1, (Face)face2 };
            else if (face1 == 0 && face2 != 0)
                throw new Exception("Invalid");
            else if (face1 != 0)
                BestFaces = new[] { (Face)face1 };
            else
                BestFaces = null;

            // Set board
            Board = new Card[5];
            for (int i = 0; i < 5; i++)
            {
                Board[i] = binary[i + 2].ParseCard() ?? Board[i];
            }

            // Set pocket
            Pocket = new Card[2];
            for (int i = 0; i < 2; i++)
            {
                Pocket[i] = binary[i + 7].ParseCard().Value;
            }

            // Set result
            Result = new Card[5];
            for (int i = 0; i < 5; i++)
            {
                Result[i] = binary[i + 9].ParseCard() ?? Result[i];
            }

            // Set won
            Won = (char)binary[14];
        }

        public bool WinnerSpecified => Won == 'y' || Won == 'n';

        public bool IsComplete => Board != null
                                  && Pocket != null
                                  && Result != null
                                  && Board.Length == 5
                                  && Pocket.Length == 2
                                  && Result.Length == 5
                                  && Board[0].IsValid
                                  && Board[1].IsValid
                                  && Board[2].IsValid
                                  && Board[3].IsValid
                                  && Board[4].IsValid
                                  && Pocket[0].IsValid
                                  && Pocket[1].IsValid
                                  && Result[0].IsValid
                                  && Result[1].IsValid
                                  && Result[2].IsValid
                                  && Result[3].IsValid
                                  && Result[4].IsValid;

        public static IEnumerable<SamplePlay> Load(string filename)
        {
            const int sizeeach = 15;
            using (var input = new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
            {
                var length = input.BaseStream.Length;
                if ((length % sizeeach) != 0) throw new Exception("Invalid file");

                var count = (int)(length / sizeeach);

                var buffer = new byte[sizeeach];
                for (int i = 0; i < count; i++)
                {
                    input.Read(buffer, 0, sizeeach);
                    yield return new SamplePlay(buffer);
                }
            }
        }
    }
}