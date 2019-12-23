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
            int value;
            var resultHands = new List<(int, Hand)>();
            foreach (var hand in hands)
            {
                var cards = hand.Cards();
                cards.AddRange(_tableCards);
                var theValueHand = SortByValue(cards);
                var theSuitHand = SortBySuit(cards);
                _tempList = IsStraightFlush(theValueHand, out value);
                if (_tempList.Count > 0)
                {
                    cards.RemoveRange(2,5);
                    resultHands.Add((value, hand));
                    continue;
                }

                _tempList = IsFourKind(theValueHand, out value);
                if (_tempList.Count > 0)
                {
                    cards.RemoveRange(2, 5);
                    resultHands.Add((value, hand));
                    continue;
                }

                _tempList = IsFullHouse(theValueHand, out value);
                if (_tempList.Count > 0)
                {
                    cards.RemoveRange(2, 5);
                    resultHands.Add((value, hand));
                    continue;
                }

                _tempList = IsFlush(theValueHand, out value);
                if (_tempList.Count > 0)
                {
                    cards.RemoveRange(2, 5);
                    resultHands.Add((value, hand));
                    continue;
                }

                _tempList = IsStraight(theValueHand, out value);
                if (_tempList.Count > 0)
                {
                    cards.RemoveRange(2, 5);
                    resultHands.Add((value, hand));
                    continue;
                }

                _tempList = IsThreeKind(theValueHand, out value);
                if (_tempList.Count > 0)
                {
                    cards.RemoveRange(2, 5);
                    resultHands.Add((value, hand));
                    continue;
                }

                _tempList = IsTwoPair(theValueHand , out value);
                if (_tempList.Count > 0)
                {
                    cards.RemoveRange(2, 5);
                    resultHands.Add((3, hand));
                    continue;
                }

                _tempList = IsOnePair(theValueHand, out value);
                if (_tempList.Count > 0)
                {
                    cards.RemoveRange(2, 5);
                    resultHands.Add((value, hand));
                    continue;
                }

                _tempList = HighCard(theValueHand, out value);
                if (_tempList.Count <= 0) continue;
                cards.RemoveRange(2, 5);
                resultHands.Add((value, hand));
            }

            return resultHands.OrderBy(x => x.Item1).ThenBy(x => DisplayCards(x.Item2.Cards())).ToList();
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


        private List<Card> IsStraightFlush(List<Card> hList, out int value) //Five cards in numerical order, all of identical suits
        {
            var st = 0;
            _tempList.Clear();

            // Flush check
            
            foreach (var t in hList)
            {
                var cardCount = hList.Count(card => card.Suit == t.Suit);
                if (cardCount >= 5)
                    _tempList.Add(t);

                if (_tempList.Count == 5)
                    break;
            }
            if (_tempList.Count == 0)
            {
                value = 0;
                return new List<Card>();
            }

            var tempList2 = _tempList;
            //Straight check

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
                    value = _tempList[0].Value + 13 * 10;
                    return _tempList;
                }
            }

            st = 0;
            if (tempList2[0].Value == 13) //Ace
            {
                for (int i = tempList2.Count - 1, j = 0; i > 0; i--, j++)
                {
                    if (tempList2[i].Value != tempList2[0].Value)
                        if ((tempList2[0].Value - tempList2[i].Value) == 12 - j)
                        {
                            st++;
                            _tempList.Add(tempList2[i]);
                        }
                        else
                            st = 0;
                    if (st == 4)
                    {
                        _tempList.Add(tempList2[0]);
                        value = 1 + 13 * 10;
                        return _tempList;
                    }
                }
            }
            
            value = 0;
            return new List<Card>();

        }

        private List<Card> IsFourKind(List<Card> hList, out int value) //Four cards of the same rank, and one side card or ‘kicker’.
        {
            _tempList.Clear();

            foreach (var t in hList)
            {
                var cardCount = hList.Count(card => card.Value == t.Value);
                if (cardCount >= 4)
                    _tempList.Add(t);

                if (_tempList.Count == 4)
                {
                    value = _tempList[0].Value + 13 * 9;
                    return _tempList;
                }
            }
            value = 0;
            return new List<Card>();
        }

        private List<Card> IsFullHouse(List<Card> hList, out int value) //Three cards of the same rank, and two cards of
        {
            int cardCount;
            _tempList.Clear();
            List<Card> tempList2 = new List<Card>();

            foreach (var t in hList)
            {
                cardCount = hList.Count(card => card.Value == t.Value);
                if (cardCount >= 3)
                    _tempList.Add(t);
                if (_tempList.Count == 3)
                    break;
            }
            if (_tempList.Count == 0)
            {
                value = 0;
                return new List<Card>();
            }
            value = _tempList[0].Value + 13 * 7;

            var hList2 = (hList.Except(_tempList)).ToList();
            foreach (var t in hList2)
            {
                cardCount = hList2.Count(card => card.Value == t.Value);
                if (cardCount >= 2)
                    tempList2.Add(t);
                if (tempList2.Count == 2)
                    break;
            }
            if (tempList2.Count == 0)
            {
                value = 0;
                return new List<Card>();
            }
            value += tempList2[0].Value;
            _tempList.AddRange(tempList2);

            return _tempList;
        }

        private List<Card> IsFlush(List<Card> hList, out int value) //Five cards of the same suit.
        {
            _tempList.Clear();

            foreach (var t in hList)
            {
                var cardCount = hList.Count(card => card.Suit == t.Suit);
                if (cardCount >= 5)
                    _tempList.Add(t);
                if (_tempList.Count == 5)
                {
                    value = _tempList[0].Value + 13 * 6;
                    return _tempList;
                }
            }
            value = 0;
            return new List<Card>();
        }

        private List<Card> IsStraight(List<Card> hList, out int value) //Five cards in sequence.
        {
            var st = 0;
            _tempList.Clear();
            for (var i = 0; i < hList.Count - 1; i++)
            {
                if (hList[i].Value != hList[i + 1].Value)
                    if ((hList[i].Value - hList[i + 1].Value) == 1)
                    {
                        st++;
                        _tempList.Add(hList[i]);
                    }
                    else
                        st = 0;
                if (st == 4)
                {
                    _tempList.Add(hList[i + 1]);
                    value = _tempList[0].Value + 13 * 5;
                    return _tempList;
                }
            }

            st = 0;
            _tempList.Clear();

            if (hList[0].Value == 13) //Ace
                for (int i = hList.Count - 1, j = 0; i > 0; i--, j++)
                {
                    if (hList[i].Value != hList[0].Value)
                        if ((hList[0].Value - hList[i].Value) == 12 - j)
                        {
                            st++;
                            _tempList.Add(hList[i]);
                        }
                        else
                            st = 0;
                    if (st == 5)
                    {
                        value = 1 + 13 * 5;
                        return _tempList;
                    }
                }
            value = 0;
            return new List<Card>();
        }


        private List<Card> IsThreeKind(List<Card> hList, out int value) //Three cards of the same rank, and two 
        {                                               //unrelated side cards
            _tempList.Clear();

            foreach (var t in hList)
            {
                var cardCount = hList.Count(card => card.Value == t.Value);
                if (cardCount >= 3)
                    _tempList.Add(t);
                if (_tempList.Count == 3)
                {
                    value = _tempList[0].Value + 13 * 4;
                    return _tempList;
                }
            }
            value = 0;
            return new List<Card>();
        }

        private List<Card> IsTwoPair(List<Card> hList, out int value) //Two cards of a matching rank, another two cards of a different 
        {                                             //matching rank, and one side card.
            int cardCount;
            _tempList.Clear();
            List<Card> tempList2 = new List<Card>();

            foreach (var t in hList)
            {
                cardCount = hList.Count(card => card.Value == t.Value);
                if (cardCount >= 2)
                    _tempList.Add(t);
                if (_tempList.Count == 2)
                    break;
            }
            if (_tempList.Count < 2)
            {
                value = 0;
                return new List<Card>();
            }
            value = _tempList[0].Value + 13 * 2;

            var hList2 = (hList.Except(_tempList)).ToList();

            foreach (var t in hList2)
            {
                cardCount = hList2.Count(card => card.Value == t.Value);
                if (cardCount >= 2)
                    tempList2.Add(t);
                if (tempList2.Count == 2)
                    break;
            }
            if (tempList2.Count != 2)
            {
                value = 0;
                return new List<Card>();
            }
            value += tempList2[0].Value;
            _tempList.AddRange(tempList2);

            return _tempList;
        }

        private List<Card> IsOnePair(List<Card> hList, out int value) //Two cards of a matching rank, and three unrelated side cards.
        {
            _tempList.Clear();

            foreach (var t in hList)
            {
                var cardCount = hList.Count(card => card.Value == t.Value);
                if (cardCount >= 2)
                    _tempList.Add(t);
                if (_tempList.Count == 2)
                {
                    value = _tempList[0].Value + 13;
                    return _tempList;
                }
            }
            value = 0;
            return new List<Card>();
        }

        private List<Card> HighCard(List<Card> hList, out int value)
        {
            _tempList.Add(hList[0]);
            value = hList[0].Value;
            return _tempList;
        }
    }
}
