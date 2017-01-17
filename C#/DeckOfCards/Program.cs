using System;
using System.Collections.Generic;

namespace DeckOfCards
{
    class Program
    {
        static void Main()
        {
            // Console.ForegroundColor = ConsoleColor.Blue;
            // Console.BackgroundColor = ConsoleColor.Black;
            // Deck myDeck = new Deck();
            // Console.WriteLine(myDeck);
            // myDeck.Shuffle();
            // Console.WriteLine(myDeck);
            List<int> nums = new List<int> {1,2,3,4};
            int[] arr = new int[4];
            foreach (int num in nums)
                System.Console.WriteLine(num);
            // for (int i = 0; i < arr.Length; i++)
            // {
            //     arr[i] = i;
            //     System.Console.WriteLine(arr[i]);
            // }
            // Program pro = new Program();
            // pro.param(1,2,3,4);
            // Console.ReadKey(true);
        }
        public void param(params int[] arr)
        {
            foreach (var item in arr)
            {
                Console.WriteLine(item);
            }
        }
    }
}
