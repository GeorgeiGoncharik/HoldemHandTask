using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HoldemHand.Model;

namespace HoldemHand
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            string inputString;
            while ((inputString = Console.In.ReadLine()) != null)
            {
                var substrings = inputString?.Split(' ').ToList();

                var table = new Table((substrings ?? throw new InvalidOperationException()).First()); //<5 board cards>

                var hands = new List<Hand>();
                if (!int.TryParse(substrings[1], out var amountOfPlayers))
                    for (var i = 1; i < substrings.Count; i++)// <hand N> if the amount of them(N) is not specified 
                    {
                        var hand = new Hand();
                        Table.DealManyCardsTo(hand, substrings[i]);
                        hands.Add(hand);
                    }
                else if (amountOfPlayers + 2 == substrings.Count)
                    for (var i = 2; i < amountOfPlayers + 2; i++)// <hand N> if it is specified 
                    {
                        var hand = new Hand();
                        Table.DealManyCardsTo(hand, substrings[i]);
                        hands.Add(hand);
                    }
                else
                    throw new InvalidDataException
                        ("Incorrect input. check if the number of hands is the same as the number of hands you have inputted, or input your hands without specifying the number",
                        null);


                var bestHands = table.EvaluateManyHands(hands);
                for (var i = 1; i < bestHands.Count; i++)
                    Console.Out.Write(
                                    table.DisplayCards(bestHands[i - 1].Item2.Cards())
                                    + (bestHands[i].Item1 == bestHands[i - 1].Item1 ? "=" : " ")
                                    + (i == bestHands.Count - 1 ? table.DisplayCards(bestHands[i].Item2.Cards()) : ""));
                Console.Out.WriteLine();
            }
        }
    }
}
