using System;
using System.Collections.Generic;
using DeckOfCards;

namespace User
{
    public class Player
    {
        string Name { get; set; }
        List<Card> hand = new List<Card>();
        public Player(string name)
        {
            Name = name;
        }

        public Card Draw(Deck curDeck)
        {
            Card curCard = curDeck.Deal();
            hand.Add(curCard);
            return curCard;
        }

        public bool Discard(Card toDiscard)
        {
            return hand.Remove(toDiscard);
        }
    }

}