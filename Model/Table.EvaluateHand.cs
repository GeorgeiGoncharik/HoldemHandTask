using System.Collections.Generic;
using System.Linq;

namespace HoldemHand.Model
{
    internal partial class Table
    {
        private List<Card> IsStraightFlush(List<Card> hList) //Five cards in numerical order, all of identical suits //possible errors
        {
            var st = 0;
            _tempList.Clear();

            // Flush check
            foreach (var t in hList)
            {
                var cardCount = hList.Count(card => card.Suit == t.Suit);
                if (cardCount >= 5)
                    _tempList.Add(t);
            }
            if (_tempList.Count < 5)
            {
                return new List<Card>();
            }

            //Straight check
            hList = _tempList.ToList();
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
                    return _tempList; //1-5 straight high to low
                }
            }

            st = 0;
            _tempList.Clear();
            if (hList[0].Value == 13) //Ace through 5
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
                    if (st == 4)
                    {
                        _tempList.Add(hList.First());
                        return _tempList.OrderBy(x => x.Value).ToList(); //1-5 low to high
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
                if (cardCount >= 4)
                    _tempList.Add(t);

                if (_tempList.Count == 4)
                {
                    hList = hList.Except(_tempList).ToList();
                    _tempList.Add(hList.First());
                    return _tempList; //1-4 four of a kind 5 kicker highest
                }
            }
            return new List<Card>();
        }

        private List<Card> IsFullHouse(List<Card> hList) //A full house is the combination of three of a kind and a pair
        {
            int cardCount;
            _tempList.Clear();

            foreach (var t in hList)
            {
                cardCount = hList.Count(card => card.Value == t.Value);
                if (cardCount >= 3)
                    _tempList.Add(t);
                if (_tempList.Count == 3)
                    break;
            }
            if (_tempList.Count != 3)
            {
                return new List<Card>();
            }

            hList = hList.Except(_tempList).ToList();
            foreach (var t in hList)
            {
                cardCount = hList.Count(card => card.Value == t.Value);
                if (cardCount >= 2)
                    _tempList.Add(t);
                if (_tempList.Count == 5)
                    break;
            }
            if (_tempList.Count == 5)
            {
                return _tempList; //1-3 three of a kind 4-5 pair
            }
            return new List<Card>();
        }

        private List<Card> IsFlush(List<Card> hList) //Five cards of the same suit.
        {
            _tempList.Clear();

            foreach (var t in hList)
            {
                var cardCount = hList.Count(card => card.Suit == t.Suit);
                if (cardCount >= 5)
                    _tempList.Add(t);
                if (_tempList.Count == 5)
                {
                    return _tempList; //1-5 flush high to low
                }
            }
            return new List<Card>();
        }

        private List<Card> IsStraight(List<Card> hList) //Five cards in sequence.
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
                    return _tempList; //1-5 straight high to low
                }
            }

            st = 0;
            _tempList.Clear();
            if (hList[0].Value == 13) //Ace through 5
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
                    if (st == 4)
                    {
                        _tempList.Add(hList.First());
                        return _tempList.OrderBy(x => x.Value).ToList(); //1-5 low to high
                    }
                }
            return new List<Card>();
        }

        private List<Card> IsThreeKind(List<Card> hList) //Three cards of the same rank, and two 
        {                                                               //unrelated side cards
            _tempList.Clear();

            foreach (var t in hList)
            {
                var cardCount = hList.Count(card => card.Value == t.Value);
                if (cardCount >= 3)
                    _tempList.Add(t);
                if (_tempList.Count == 3)
                {
                    hList = hList.Except(_tempList).ToList();
                    return _tempList.Concat(hList.GetRange(0, 2)).ToList(); //1-3 three of a kind 4-5 kickers high to low
                }
            }
            return new List<Card>();
        }

        private List<Card> IsTwoPair(List<Card> hList) //Two cards of a matching rank, another two cards of a different 
        {                                              //matching rank, and one side card.
            int cardCount;
            _tempList.Clear();

            foreach (var t in hList)
            {
                cardCount = hList.Count(card => card.Value == t.Value);
                if (cardCount >= 2)
                    _tempList.Add(t);
                if (_tempList.Count == 2)
                    break;
            }
            if (_tempList.Count != 2)
                return new List<Card>();


            hList = hList.Except(_tempList).ToList();
            foreach (var t in hList)
            {
                cardCount = hList.Count(card => card.Value == t.Value);
                if (cardCount >= 2)
                    _tempList.Add(t);
                if (_tempList.Count == 4)
                    break;
            }
            if (_tempList.Count != 4)
                return new List<Card>();

            hList = hList.Except(_tempList).ToList();
            _tempList.Add(hList.First());

            return _tempList; //1-2 high pair 3-4 low pair 5 kicker
        }

        private List<Card> IsOnePair(List<Card> hList) //Two cards of a matching rank, and three unrelated side cards.
        {
            _tempList.Clear();

            foreach (var t in hList)
            {
                var cardCount = hList.Count(card => card.Value == t.Value);
                if (cardCount >= 2)
                    _tempList.Add(t);
                if (_tempList.Count == 2)
                {
                    hList = hList.Except(_tempList).ToList();
                    return _tempList.Concat(hList.GetRange(0,3)).ToList(); //1-2 pair 3-5 kickers high to low
                }
            }
            return new List<Card>();
        }

        private List<Card> HighCard(List<Card> hList)
        {
            hList.RemoveRange(5, 2); //1-5 high to low
            return hList;
        }
    }
}