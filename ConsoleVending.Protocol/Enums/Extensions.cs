namespace ConsoleVending.Protocol.Enums{
    public static class EnumExtensions {
        public static string ToHuman(this Denomination denomination) {
            switch(denomination){
                case Denomination.OnePenny: return "1p";
                case Denomination.TwoPenny: return "2p";
                case Denomination.FivePenny: return "5p";
                case Denomination.TenPenny: return "10p";
                case Denomination.TwentyPenny: return "20p";
                case Denomination.FiftyPenny: return "50p";
                case Denomination.OnePound: return "1£";
                case Denomination.TwoPound: return "2£";
                default: throw new ArgumentOutOfRangeException(nameof(denomination));
            }
        }
    }
}