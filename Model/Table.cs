using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HoldemHand.Helpers;

namespace HoldemHand.Model
{
    internal partial class Table
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
        
        public List<(int,Hand)> EvaluateManyHandsOmaha(IEnumerable<Hand> hands)
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
                //логика такая: создаю из каждой сложной руки (4!/(2!(4-2)!)) 6 простых. Выполняю для них обычную оценку и потом беру лучшую комбинацию для дальнейших сравнений
                List<Hand> combinations = new List<Hand>();
                foreach (var pair in Permutations.GetPermutations(hand.Cards(),2))
                {
                    Hand combHand = new Hand();
                    DealManyCardsTo(combHand, DisplayCards(pair));
                    combinations.Add(combHand);
                }
                var bestCombinations = EvaluateManyHands(combinations);

                var cards = bestCombinations.Last().Item2.Cards();
                cards.AddRange(_tableCards);
                var theValueHand = SortByValue(cards);
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
    }
}
