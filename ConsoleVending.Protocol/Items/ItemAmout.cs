using System;

namespace ConsoleVending.Protocol.Items
{

    public struct ItemAmount
    {
        public readonly Item Item;
        public readonly int Amount;

        public ItemAmount(Item item, int amout)
        {
            Item = item;
            Amount = amout;
        }

        public static ItemAmount operator +(ItemAmount left, int amount)
        {
            return new ItemAmount(left.Item, Math.Clamp(left.Amount + amount, 0, int.MaxValue));
        }

        public static ItemAmount operator -(ItemAmount left, int amount)
        {
            return new ItemAmount(left.Item, Math.Clamp(left.Amount - amount, 0, int.MaxValue));
        }

        public override bool Equals(object? obj)
        {
            if (obj is ItemAmount itm)
            {
                return itm.Amount == Amount
                       && itm.Item == Item;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode()
                   + Item.GetHashCode()
                   + Amount.GetHashCode();
        }
    }
}