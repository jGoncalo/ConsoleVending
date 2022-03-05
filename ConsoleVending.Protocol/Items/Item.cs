using System.Diagnostics.CodeAnalysis;

namespace ConsoleVending.Protocol.Items
{
    public struct Item
    {
        public readonly string Name;
        public readonly uint Code;
        public readonly uint Cost; //Stored in cents/pennies

        public Item(string name, uint code, uint cost)
        {
            Name = name;
            Code = code;
            Cost = cost;
        }

        public static bool operator==(Item left, Item right)
        {
            return left.Equals(right);
        }
        public static bool operator!=(Item left, Item right)
        {
            return !left.Equals(right);
        }

        public string CostString => $"{(Cost / 100.0f):N2}£";

        public override string ToString()
        {
            return $"[{Code}] - {Name} : {CostString}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is Item itm)
            {
                return itm.Name == Name
                       && itm.Cost == Cost
                       && itm.Code == Code;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode()
                   + Name.GetHashCode()
                   + Cost.GetHashCode()
                   + Code.GetHashCode();
        }
    }
}