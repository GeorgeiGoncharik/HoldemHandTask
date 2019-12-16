using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HoldemHand.Model
{
    internal class Table
    {
        private readonly List<Card> _tableCards;

        private List<Card> _tempList = new List<Card>();
        private List<Card> _sortedList = new List<Card>();

        public Table(string info)
        {
            _tableCards = new List<Card>();
            if (info.Length == 10)
            {
                var infoList = Enumerable.Range(0, info.Length / 2)
                    .Select(i => info.Substring(i * 2, 2)).ToList();
                foreach (var cardInfo in infoList)
                {
                    _tableCards.Add(new Card(cardInfo));
                }
            }
            else
                throw new ArgumentOutOfRangeException(nameof(info), "Incorrect input. You entered more or less than 5 board cards");
                
        }

        private static void DealCardTo(Hand hand, string cardInfo)
        {
            var card = new Card(cardInfo);
            hand.AddCard(card);
        }

        public static void DealManyCardsTo(Hand hand, string info)
        {
            var infoList = Enumerable.Range(0, info.Length / 2)
                .Select(i => info.Substring(i * 2, 2)).ToList();
            foreach (var infoVal in infoList)
                DealCardTo(hand, infoVal);
        }


        public List<(int,Hand)> EvaluateManyHands(IEnumerable<Hand> hands)
        {
            var resultHands = new List<(int, Hand)>();
            foreach (var hand in hands)
            {
                var cards = hand.Cards();
                cards.AddRange(_tableCards);
                var theValueHand = SortByValue(cards);
                var theSuitHand = SortBySuit(cards);
                _tempList = IsStraightFlush(theValueHand);
                if (_tempList.Count > 0)
                {
                    cards.RemoveRange(2,5);
                    resultHands.Add((9, hand));
                    continue;
                }

                _tempList = IsFourKind(theValueHand);
                if (_tempList.Count > 0)
                {
                    cards.RemoveRange(2, 5);
                    resultHands.Add((8, hand));
                    continue;
                }

                _tempList = IsFullHouse(theValueHand);
                if (_tempList.Count > 0)
                {
                    cards.RemoveRange(2, 5);
                    resultHands.Add((7, hand));
                    continue;
                }

                _tempList = IsFlush(theSuitHand);
                if (_tempList.Count > 0)
                {
                    cards.RemoveRange(2, 5);
                    resultHands.Add((6, hand));
                    continue;
                }

                _tempList = IsStraight(theValueHand);
                if (_tempList.Count > 0)
                {
                    cards.RemoveRange(2, 5);
                    resultHands.Add((5, hand));
                    continue;
                }

                _tempList = IsThreeKind(theValueHand);
                if (_tempList.Count > 0)
                {
                    cards.RemoveRange(2, 5);
                    resultHands.Add((4, hand));
                    continue;
                }

                _tempList = IsTwoPair(theValueHand);
                if (_tempList.Count > 0)
                {
                    cards.RemoveRange(2, 5);
                    resultHands.Add((3, hand));
                    continue;
                }

                _tempList = IsOnePair(theValueHand);
                if (_tempList.Count > 0)
                {
                    cards.RemoveRange(2, 5);
                    resultHands.Add((2, hand));
                    continue;
                }

                _tempList = HighCard(theValueHand);
                if (_tempList.Count <= 0) continue;
                cards.RemoveRange(2, 5);
                resultHands.Add((1, hand));
            }

            return resultHands.OrderBy(x => x.Item1).ThenBy(x => DisplayCards(x.Item2.Cards())).ToList(); //.Select(v => v.Item2).ToList();
        }

        public string DisplayCards(IEnumerable<Card> input)
        {
            var stringBuilder = new StringBuilder(input.Aggregate("", (current, card) => current + card.CardName));
            return stringBuilder.ToString();
        }

        private List<Card> SortByValue(IEnumerable<Card> hand)
        {
            return hand.OrderByDescending(cardName => cardName.Value).ToList();
        }


        private List<Card> SortBySuit(IEnumerable<Card> hand)
        {
            return hand.OrderByDescending(cardName => cardName.Suit).ToList();
        }


        private List<Card> IsStraightFlush(List<Card> hList) //Five cards in numerical order, all of identical suits
        {
            var st = 0;
            _tempList.Clear();
            List<Card> tempList2 = new List<Card>();

            //Straight check
            foreach (var t in hList)
            {
                var cardCount = hList.Count(card => card.Suit == t.Suit);
                if (cardCount == 5)
                    _tempList.Add(t);

                if (_tempList.Count == 5)
                    break;
            }
            if (_tempList.Count == 0)
                return new List<Card>();

            // Flush check
            for (var i = 0; i < _tempList.Count - 1; i++)
            {
                if (_tempList[i].Value != _tempList[i + 1].Value)
                    if ((_tempList[i].Value - _tempList[i + 1].Value) == 1)
                        st++;
                    else
                        st = 0;
                if (st == 4)
                {
                    _tempList.Add(hList[i + 1]);
                    return tempList2;
                }
            }

            return new List<Card>();

        }

        private List<Card> IsFourKind(List<Card> hList) //Four cards of the same rank, and one side card or ‘kicker’.
        {
            _tempList.Clear();

            foreach (var t in hList)
            {
                var cardCount = hList.Count(card => card.Value == t.Value);
                if (cardCount == 4)
                    _tempList.Add(t);

                if (_tempList.Count == 4)
                    return _tempList;
            }

            return new List<Card>();
        }

        private List<Card> IsFullHouse(List<Card> hList) //Three cards of the same rank, and two cards of
        {
            int cardCount;
            _tempList.Clear();
            List<Card> tempList2 = new List<Card>();

            foreach (var t in hList)
            {
                cardCount = hList.Count(card => card.Value == t.Value);
                if (cardCount == 3)
                    _tempList.Add(t);
            }
            if (_tempList.Count == 0)
                return new List<Card>();

            var hList2 = (hList.Except(_tempList)).ToList();
            foreach (var t in hList2)
            {
                cardCount = hList2.Count(card => card.Value == t.Value);
                if (cardCount == 2)
                    tempList2.Add(t);
            }
            if (tempList2.Count == 0)
                return new List<Card>();

            _tempList.AddRange(tempList2);

            return _tempList;
        }

        private List<Card> IsFlush(List<Card> hList) //Five cards of the same suit.
        {
            _tempList.Clear();

            foreach (var t in hList)
            {
                var cardCount = hList.Count(card => card.Suit == t.Suit);
                if (cardCount == 5)
                    _tempList.Add(t);
                if (_tempList.Count == 5)
                    return _tempList;
            }

            return new List<Card>();
        }

        private List<Card> IsStraight(List<Card> hList) //Five cards in sequence.
        {
            var st = 0;
            _tempList.Clear();
            _sortedList = hList;
            for (var i = 0; i < _sortedList.Count - 1; i++)
            {
                if (_sortedList[i].Value != _sortedList[i + 1].Value)
                    if ((_sortedList[i].Value - _sortedList[i + 1].Value) == 1)
                    {
                        st++;
                        _tempList.Add(_sortedList[i]);
                    }
                    else
                        st = 0;
                if (st == 4)
                {
                    _tempList.Add(_sortedList[i + 1]);
                    return _tempList;
                }
            }
            return new List<Card>();
        }


        private List<Card> IsThreeKind(List<Card> hList) //Three cards of the same rank, and two 
        {                                               //unrelated side cards
            _tempList.Clear();

            foreach (var t in hList)
            {
                var cardCount = hList.Count(card => card.Value == t.Value);
                if (cardCount == 3)
                    _tempList.Add(t);
                if (_tempList.Count == 3)
                    return _tempList;
            }
            return new List<Card>();
        }

        private List<Card> IsTwoPair(List<Card> hList) //Two cards of a matching rank, another two cards of a different 
        {                                             //matching rank, and one side card.
            int cardCount;
            _tempList.Clear();
            List<Card> tempList2 = new List<Card>();

            foreach (var t in hList)
            {
                cardCount = hList.Count(card => card.Value == t.Value);
                if (cardCount == 2)
                    _tempList.Add(t);
                if (_tempList.Count == 2)
                    break;
            }
            if (_tempList.Count < 2)
                return new List<Card>();

            var hList2 = (hList.Except(_tempList)).ToList();

            foreach (var t in hList2)
            {
                cardCount = hList2.Count(card => card.Value == t.Value);
                if (cardCount == 2)
                    tempList2.Add(t);
                if (tempList2.Count == 2)
                    break;
            }
            if (tempList2.Count != 2)
                return new List<Card>();

            _tempList.AddRange(tempList2);

            return _tempList;
        }


        private List<Card> IsOnePair(List<Card> hList) //Two cards of a matching rank, and three unrelated side cards.
        {
            _tempList.Clear();

            foreach (var t in hList)
            {
                var cardCount = hList.Count(card => card.Value == t.Value);
                if (cardCount == 2)
                    _tempList.Add(t);
                if (_tempList.Count == 2)
                    return _tempList;
            }
            return new List<Card>();
        }

        private List<Card> HighCard(List<Card> hList)
        {
            _tempList.Add(hList[0]);
            return _tempList;
        }
    }
}
