using System;

namespace Kitchen
{
    public static class GlobalCounter 
    {
        public static int[] attendedClients = new int[Enum.GetValues((typeof(IngredientName))).Length];
        public static int streaksAmount = 0;
        public static int frightenedMonkeys=0;
    }
}
