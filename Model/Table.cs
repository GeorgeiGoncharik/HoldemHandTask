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


        public List<(int,Hand)> EvaluateManyHands(IEnumerable<Hand> hands)
        {
            var resultHands = new List<(int, Hand)>();

            var unsortedStraightFlushes = new List<(Hand, List<Card>)>();
            var unsortedFourKinds = new List<(Hand, List<Card>)>();
            var unsortedFullHouses = new List<(Hand, List<Card>)>();
            var unsortedFlushes = new List<(Hand, List<Card>)>();
            var unsortedStraights = new List<(Hand, List<Card>)>();
            var unsortedThreeKinds = new List<(Hand, List<Card>)>();
            var unsortedTwoPairs = new List<(Hand, List<Card>)>();
            var unsortedOnePairs = new List<(Hand, List<Card>)>();
            var unsortedHighCards = new List<(Hand, List<Card>)>();
            foreach (var hand in hands)
            {
                var cards = hand.Cards();
                cards.AddRange(_tableCards);
                var theValueHand = SortByValue(cards);
                var theSuitHand = SortBySuit(cards);
                _tempList = IsStraightFlush(theValueHand);
                if (_tempList.Count == 5)
                {
                    cards.RemoveRange(2,5);
                    unsortedStraightFlushes.Add((hand, new List<Card>().Concat(_tempList).ToList()));
                    continue;
                }

                _tempList = IsFourKind(theValueHand);
                if (_tempList.Count == 5)
                {
                    cards.RemoveRange(2, 5);
                    unsortedFourKinds.Add((hand, new List<Card>().Concat(_tempList).ToList()));
                    continue;
                }

                _tempList = IsFullHouse(theValueHand);
                if (_tempList.Count == 5)
                {
                    cards.RemoveRange(2, 5);
                    unsortedFullHouses.Add((hand, new List<Card>().Concat(_tempList).ToList()));
                    continue;
                }

                _tempList = IsFlush(theValueHand);
                if (_tempList.Count == 5)
                {
                    cards.RemoveRange(2, 5);
                    unsortedFlushes.Add((hand, new List<Card>().Concat(_tempList).ToList()));
                    continue;
                }

                _tempList = IsStraight(theValueHand);
                if (_tempList.Count == 5)
                {
                    cards.RemoveRange(2, 5);
                    unsortedStraights.Add((hand, new List<Card>().Concat(_tempList).ToList()));
                    continue;
                }

                _tempList = IsThreeKind(theValueHand);
                if (_tempList.Count == 5)
                {
                    cards.RemoveRange(2, 5);
                    unsortedThreeKinds.Add((hand, new List<Card>().Concat(_tempList).ToList()));
                    continue;
                }

                _tempList = IsTwoPair(theValueHand);
                if (_tempList.Count == 5)
                {
                    cards.RemoveRange(2, 5);
                    unsortedTwoPairs.Add((hand, new List<Card>().Concat(_tempList).ToList()));
                    continue;
                }

                _tempList = IsOnePair(theValueHand);
                if (_tempList.Count == 5)
                {
                    cards.RemoveRange(2, 5);
                    unsortedOnePairs.Add((hand, new List<Card>().Concat(_tempList).ToList()));
                    continue;
                }

                _tempList = HighCard(theValueHand);
                if (_tempList.Count == 5)
                {
                    cards.RemoveRange(2, 5);
                    unsortedHighCards.Add((hand, new List<Card>().Concat(_tempList).ToList()));
                    continue;
                }
            }
            resultHands.AddRange(SortHighCards(unsortedHighCards));
            resultHands.AddRange(SortOnePairs(unsortedOnePairs));
            resultHands.AddRange(SortTwoPairs(unsortedTwoPairs));
            resultHands.AddRange(SortThreeKinds(unsortedThreeKinds));
            resultHands.AddRange(SortStraights(unsortedStraights));
            resultHands.AddRange(SortFlushes(unsortedFlushes));
            resultHands.AddRange(SortFullHouses(unsortedFullHouses));
            resultHands.AddRange(SortFourKinds(unsortedFourKinds));
            resultHands.AddRange(SortStraightFlushes(unsortedStraightFlushes));
            return resultHands;
        }

        private List<(int, Hand)> SortStraightFlushes(List<(Hand, List<Card>)> hList)
        {
            List<(int, Hand)> output = new List<(int, Hand)>();
            var sortedHList = hList?.OrderBy(x => x.Item2[0].Value).ThenBy(x => DisplayCards(x.Item1.Cards())).ToList();
            for (int i = 0; i < sortedHList.Count; i++)
            {
                if (i != sortedHList.Count - 1)
                    if (sortedHList[i].Item2[0].Value == sortedHList[i + 1].Item2[0].Value)
                        output.Add((1, sortedHList[i].Item1));
                    else
                        output.Add((0, sortedHList[i].Item1));
                else
                    output.Add((0, sortedHList[i].Item1));
            }

            return output;
        }

        private List<(int, Hand)> SortFourKinds(List<(Hand, List<Card>)> hList)
        {
            List<(int, Hand)> output = new List<(int, Hand)>();

            var sortedHList = hList?.OrderBy(x => x.Item2[0].Value).ThenBy(x => x.Item2[4].Value).ThenBy(x => DisplayCards(x.Item1.Cards())).ToList();
            for (int i = 0; i < sortedHList.Count; i++)
            {
                if (i != sortedHList.Count - 1)
                    if (sortedHList[i].Item2[0].Value == sortedHList[i + 1].Item2[0].Value && sortedHList[i].Item2[4].Value == sortedHList[i + 1].Item2[4].Value)
                        output.Add((1, sortedHList[i].Item1));
                    else
                        output.Add((0, sortedHList[i].Item1));
                else
                    output.Add((0, sortedHList[i].Item1));
            }

            return output;
        }

        private List<(int, Hand)> SortFullHouses(List<(Hand, List<Card>)> hList)
        {
            List<(int, Hand)> output = new List<(int, Hand)>();

            var sortedHList = hList?.OrderBy(x => x.Item2[0].Value).ThenBy(x => x.Item2[3].Value).ThenBy(x => DisplayCards(x.Item1.Cards())).ToList();
            for (int i = 0; i < sortedHList.Count; i++)
            {
                if (i != sortedHList.Count - 1)
                    if (sortedHList[i].Item2[0].Value == sortedHList[i + 1].Item2[0].Value && sortedHList[i].Item2[3].Value == sortedHList[i + 1].Item2[3].Value)
                        output.Add((1, sortedHList[i].Item1));
                    else
                        output.Add((0, sortedHList[i].Item1));
                else
                    output.Add((0, sortedHList[i].Item1));
            }

            return output;
        }

        private List<(int, Hand)> SortFlushes(List<(Hand, List<Card>)> hList)
        {
            List<(int, Hand)> output = new List<(int, Hand)>();

            var sortedHList = hList?.OrderBy(x => x.Item2[0].Value).
            ThenBy(x => x.Item2[1].Value).
            ThenBy(x => x.Item2[2].Value).
            ThenBy(x => x.Item2[3].Value).
            ThenBy(x => x.Item2[4].Value).ThenBy(x => DisplayCards(x.Item1.Cards())).ToList();
            for (int i = 0; i < sortedHList.Count; i++)
            {
                if (i != sortedHList.Count - 1)
                    if (sortedHList[i].Item2[0].Value == sortedHList[i + 1].Item2[0].Value &&
                    sortedHList[i].Item2[1].Value == sortedHList[i + 1].Item2[1].Value &&
                    sortedHList[i].Item2[2].Value == sortedHList[i + 1].Item2[2].Value &&
                    sortedHList[i].Item2[3].Value == sortedHList[i + 1].Item2[3].Value &&
                    sortedHList[i].Item2[4].Value == sortedHList[i + 1].Item2[4].Value)
                        output.Add((1, sortedHList[i].Item1));
                    else
                        output.Add((0, sortedHList[i].Item1));
                else
                    output.Add((0, sortedHList[i].Item1));
            }

            return output;
        }

        private List<(int, Hand)> SortStraights(List<(Hand, List<Card>)> hList)
        {
            List<(int, Hand)> output = new List<(int, Hand)>();
            var sortedHList = hList?.OrderBy(x => x.Item2[0].Value).ThenBy(x => DisplayCards(x.Item1.Cards())).ToList();
            for (int i = 0; i < sortedHList.Count; i++)
            {
                if (i != sortedHList.Count - 1)
                    if (sortedHList[i].Item2[0].Value == sortedHList[i + 1].Item2[0].Value)
                        output.Add((1, sortedHList[i].Item1));
                    else
                        output.Add((0, sortedHList[i].Item1));
                else
                    output.Add((0, sortedHList[i].Item1));
            }

            return output;
        }

        private List<(int, Hand)> SortThreeKinds(List<(Hand, List<Card>)> hList)
        {
            List<(int, Hand)> output = new List<(int, Hand)>();
            var sortedHList = hList?.OrderBy(x => x.Item2[0].Value).ThenBy(x => x.Item2[3].Value).ThenBy(x => x.Item2[4].Value).
            ThenBy(x => DisplayCards(x.Item1.Cards())).ToList();
            for (int i = 0; i < sortedHList.Count; i++)
            {
                if (i != sortedHList.Count - 1)
                    if (sortedHList[i].Item2[0].Value == sortedHList[i + 1].Item2[0].Value &&
                        sortedHList[i].Item2[3].Value == sortedHList[i + 1].Item2[3].Value &&
                        sortedHList[i].Item2[4].Value == sortedHList[i + 1].Item2[4].Value)
                        output.Add((1, sortedHList[i].Item1));
                    else
                        output.Add((0, sortedHList[i].Item1));
                else
                    output.Add((0, sortedHList[i].Item1));
            }

            return output;
        }

        private List<(int, Hand)> SortTwoPairs(List<(Hand, List<Card>)> hList)
        {
            List<(int, Hand)> output = new List<(int, Hand)>();
            var sortedHList = hList?.OrderBy(x => x.Item2[0].Value).ThenBy(x => x.Item2[2].Value).ThenBy(x => x.Item2[4].Value).
                ThenBy(x => DisplayCards(x.Item1.Cards())).ToList();
            for (int i = 0; i < sortedHList.Count; i++)
            {
                if (i != sortedHList.Count - 1)
                    if (sortedHList[i].Item2[0].Value == sortedHList[i + 1].Item2[0].Value &&
                        sortedHList[i].Item2[2].Value == sortedHList[i + 1].Item2[2].Value &&
                        sortedHList[i].Item2[4].Value == sortedHList[i + 1].Item2[4].Value)
                        output.Add((1, sortedHList[i].Item1));
                    else
                        output.Add((0, sortedHList[i].Item1));
                else
                    output.Add((0, sortedHList[i].Item1));
            }

            return output;
        }

        private List<(int, Hand)> SortOnePairs(List<(Hand, List<Card>)> hList)
        {
            List<(int, Hand)> output = new List<(int, Hand)>();
            var sortedHList = hList?.OrderBy(x => x.Item2[0].Value).ThenBy(x => x.Item2[2].Value).ThenBy(x => x.Item2[3].Value).ThenBy(x => x.Item2[4].Value).
                ThenBy(x => DisplayCards(x.Item1.Cards())).ToList();
            for (int i = 0; i < sortedHList.Count; i++)
            {
                if (i != sortedHList.Count - 1)
                    if (sortedHList[i].Item2[0].Value == sortedHList[i + 1].Item2[0].Value &&
                    sortedHList[i].Item2[2].Value == sortedHList[i + 1].Item2[2].Value &&
                    sortedHList[i].Item2[3].Value == sortedHList[i + 1].Item2[3].Value &&
                    sortedHList[i].Item2[4].Value == sortedHList[i + 1].Item2[4].Value)
                        output.Add((1, sortedHList[i].Item1));
                    else
                        output.Add((0, sortedHList[i].Item1));
                else
                    output.Add((0, sortedHList[i].Item1));
            }

            return output;
        }

        private List<(int, Hand)> SortHighCards(List<(Hand, List<Card>)> hList)
        {
            List<(int, Hand)> output = new List<(int, Hand)>();

            var sortedHList = hList?.OrderBy(x => x.Item2[0].Value).
            ThenBy(x => x.Item2[1].Value).
            ThenBy(x => x.Item2[2].Value).
            ThenBy(x => x.Item2[3].Value).
            ThenBy(x => x.Item2[4].Value).ThenBy(x => DisplayCards(x.Item1.Cards())).ToList();
            for (int i = 0; i < sortedHList.Count; i++)
            {
                if (i != sortedHList.Count - 1)
                    if (sortedHList[i].Item2[0].Value == sortedHList[i + 1].Item2[0].Value &&
                    sortedHList[i].Item2[1].Value == sortedHList[i + 1].Item2[1].Value &&
                    sortedHList[i].Item2[2].Value == sortedHList[i + 1].Item2[2].Value &&
                    sortedHList[i].Item2[3].Value == sortedHList[i + 1].Item2[3].Value &&
                    sortedHList[i].Item2[4].Value == sortedHList[i + 1].Item2[4].Value)
                        output.Add((1, sortedHList[i].Item1));
                    else
                        output.Add((0, sortedHList[i].Item1));
                else
                    output.Add((0, sortedHList[i].Item1));
            }

            return output;
        }

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
