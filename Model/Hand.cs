using System.Collections.Generic;

namespace HoldemHand.Model
{
    internal class Hand
    {
        private readonly List<Card> _cards = new List<Card>();
        public void AddCard(Card card) => _cards.Add(card);
        public List<Card> Cards() => _cards;
    }
}