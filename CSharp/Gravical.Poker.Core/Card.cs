namespace Gravical.Poker.Core
{
    /// <summary>
    /// Card holds a single poker card with a suit and face value.
    /// </summary>
    public struct Card
    {
        public Face Face { get; }
        public Suit Suit { get; }

        public bool IsValid => Suit.IsValid() && Face.IsValid();
        public byte Key => IsValid ? (byte)GetHashCode() : (byte)0;

        public Card(Face face, Suit suit)
        {
            Guards.ArgumentSuccess(face.IsValid(), nameof(face));
            Guards.ArgumentSuccess(suit.IsValid(), nameof(suit));

            Face = face;
            Suit = suit;
        }

        public Card(byte binary)
        {
            Face = (Face)((binary >> 4) & 0xF);
            Suit = (Suit)(binary & 0xF);

            Guards.ArgumentSuccess(Face.IsValid() && Suit.IsValid(), nameof(binary), "Card is not valid");
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Card)) return false;
            var other = (Card)obj;
            return this.Face == other.Face && this.Suit == other.Suit;
        }

        public override int GetHashCode()
        {
            return ToBinary();
        }

        public byte ToBinary()
        {
            return (byte)((uint)Suit | ((uint)Face << 4));
        }

        public override string ToString()
        {
            return ToString("s");
        }

        public string ToString(string format)
        {
            if (!IsValid) return "Invalid";

            switch (format)
            {
                case "l":
                    return $"{Face} of {Suit}";
                case "s":
                default:
                    var face = (int)Face < 10
                        ? ((int)Face).ToString()
                        : $"{Face.ToString()[0]}";
                    var suit = char.ToLower(Suit.ToString()[0]);
                    return $"{face}{suit}";
            }
        }
    }
}