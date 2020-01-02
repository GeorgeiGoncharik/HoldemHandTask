using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using HoldemHand.Model;

namespace HoldemHand
{
    internal static class Program
    {
        private static bool _isOmaha = false;
        private static void Main(string[] args)
        {
            if (args.Length != 0 && args[0] == "--omaha")
                _isOmaha = true;
            
            string inputString;
            while ((inputString = Console.In.ReadLine()) != null)
            {
                try
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
                        if(amountOfPlayers >= 1 && amountOfPlayers <= 256)
                            for (var i = 2; i < amountOfPlayers + 2; i++)// <hand N> if it is specified 
                            {
                                var hand = new Hand();
                                Table.DealManyCardsTo(hand, substrings[i]);
                                hands.Add(hand);
                            }
                        else
                            throw new ArgumentOutOfRangeException
                                (nameof(amountOfPlayers), $"{nameof(amountOfPlayers)} is an integer from 1 to 256 which represents how many hands are to be compared");
                    else
                        throw new ArgumentOutOfRangeException
                            (nameof(substrings), "Incorrect input. check if the number of hands is the same as the number of hands you have inputted, or input your hands without specifying the number");

                    foreach (var hand in hands)
                    {
                        if(_isOmaha)
                            if(hand.Cards().Count != 4)
                                throw new ArgumentOutOfRangeException
                                    (nameof(hand), $"Each player is dealt four cards in Omaha");
                        if(!_isOmaha)
                            if(hand.Cards().Count != 2)
                                throw new ArgumentOutOfRangeException
                                    (nameof(hand), $"Each player is dealt two cards in Texas Holdem");
                    }
                    var bestHands = _isOmaha ? table.EvaluateManyHandsOmaha(hands) : table.EvaluateManyHands(hands);

                    for(var i = 0; i < bestHands.Count; i++)
                        Console.Write(
                        table.DisplayCards(bestHands[i].Item2.Cards())
                        + (bestHands[i].Item1 == 1 ? "=" : (i != bestHands.Count - 1 ? " " : "")));

                    Console.Out.WriteLine();
                }
                catch (ArgumentOutOfRangeException e) { Console.Out.WriteLine(e.Message); }
                catch (ArgumentException e) { Console.Out.WriteLine(e.Message); }

            }
        }
    }
}
