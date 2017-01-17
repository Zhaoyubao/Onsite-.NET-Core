using System;

namespace DeckOfCards
{
    public class Card
    {
        public int val { get; set; }
        public string suit { get; set; }
        public string StringValue { 
            get {
                switch (val)
                {
                    case 1:
                        return "A";
                    case 11:
                        return "J";
                    case 12:
                        return "Q";
                    case 13:
                        return "K";
                    default:
                        return val.ToString();
                }
            } 
        }

        public Card(int _val, string _suit)
        {
            val = _val;
            suit = _suit;
        }
    }

}