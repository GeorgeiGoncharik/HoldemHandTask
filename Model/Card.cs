using System;

namespace HoldemHand.Model
{
    internal class Card
    {
        public int Value { get; }
        public int Suit { get; }
        public string CardName { get; }

        public Card(string infoCard)
        {
            if (!int.TryParse(infoCard[0].ToString(), out var intValue))
            {
                var charValue = infoCard[0];
                if (charValue == 'T')
                    Value = 9;
                else if (charValue == 'J')
                    Value = 10;
                else if (charValue == 'Q')
                    Value = 11;
                else if (charValue == 'K')
                    Value = 12;
                else if(charValue == 'A')
                    Value = 13;
            }
            else if(intValue >= 2 && intValue <= 9)
                Value = intValue - 1;
            else 
                throw new ArgumentException(
                    $"Incorrect input. first character representing the rank of card {infoCard} is not correctly entered.",
                    nameof(infoCard),
                    null);

            var suit = infoCard[1];
            if (suit == 'c')
                Suit = 0;
            else if (suit == 'd')
                Suit = 1;
            else if (suit == 'h')
                Suit = 2;
            else if(suit == 's')
                Suit = 3;
            else
                throw new ArgumentException(
                    $"Incorrect input. second character representing the suit of card {infoCard} is not correctly entered.",
                    nameof(infoCard),
                    null);

            CardName = infoCard;
        }
    }
}
