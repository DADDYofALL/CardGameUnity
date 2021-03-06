using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Card1 : IComparable
{
    public Value value;
    public Suite suite;

    public Card1(Value value, Suite suite)
    {
        this.value = value;
        this.suite = suite;
    }
	public Card1()
	{
	}

    // implement IComparable interface
    public int CompareTo(object obj)
    {
        if (obj is Card1)
        {
            return this.value.CompareTo((obj as Card1).value);  // compare user names
        }
        throw new ArgumentException("Object is not a Card");
    }

    public enum Value { TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING, ACE };
    public enum Suite { HEARTS, DIAMONDS, CLUBS, SPADES };
}

