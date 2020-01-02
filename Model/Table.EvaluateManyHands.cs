using System.Collections.Generic;
using System.Linq;

namespace HoldemHand.Model
{
    internal partial class Table
    {
                private List<(int, Hand)> SortStraightFlushes(List<(Hand, List<Card>)> hList)
        {
            List<(int, Hand)> output = new List<(int, Hand)>();
            var sortedHList = hList?.OrderBy(x => x.Item2[0].Value).ThenBy(x => DisplayCards(x.Item1.Cards())).ToList();
            if (sortedHList != null)
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
            if (sortedHList != null)
                for (int i = 0; i < sortedHList.Count; i++)
                {
                    if (i != sortedHList.Count - 1)
                        if (sortedHList[i].Item2[0].Value == sortedHList[i + 1].Item2[0].Value &&
                            sortedHList[i].Item2[4].Value == sortedHList[i + 1].Item2[4].Value)
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
            if (sortedHList != null)
                for (int i = 0; i < sortedHList.Count; i++)
                {
                    if (i != sortedHList.Count - 1)
                        if (sortedHList[i].Item2[0].Value == sortedHList[i + 1].Item2[0].Value &&
                            sortedHList[i].Item2[3].Value == sortedHList[i + 1].Item2[3].Value)
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
            if (sortedHList != null)
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
            if (sortedHList != null)
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
            if (sortedHList != null)
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
            if (sortedHList != null)
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
            if (sortedHList != null)
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
            if (sortedHList != null)
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
    }
}