using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionApp
{
    static class RomanNumerals
    {
        public static string ToRomanNumeral(int num)
        {
            if (num <= 0 || num > 3999)
                throw new ArgumentOutOfRangeException($"{num} is not between the valid range of 1 - 3999");
            var intToRomanDictionary = new Dictionary<int, string>
            {
                {1000,"M" },
                {900,"CM" },
                {500,"D" },
                {400,"CD" },
                {100,"C" },
                {90,"XC" },
                {50,"L" },
                {40,"XL" },
                {10,"X" },
                {9,"IX" },
                {5,"V" },
                {4,"IV" },
                {1,"I" }
            };

            var romanNumerals = new StringBuilder();

            foreach (var item in intToRomanDictionary.Where(x => num >= x.Key))
            {
                if (char.TryParse(item.Value, out char roman))
                {
                    int quo = num / item.Key;
                    romanNumerals.Append(new string(roman, quo));
                    num -= item.Key * quo;
                }
                else
                {
                    romanNumerals.Append(item.Value);
                    num -= item.Key;
                }

                if (num == 0) break;
            }
            return romanNumerals.ToString();
        }

        public static int FromRomanNumeral(string roman)
        {
            var romanToIntDictionary = new Dictionary<char, int>
            {
                { 'I', 1 },
                { 'V', 5 },
                { 'X', 10 },
                { 'L', 50 },
                { 'C', 100 },
                { 'D', 500 },
                { 'M', 1000 },
             };


            int total = 0;
            char previousRoman = '\0';

            for (int i = 0; i < roman.Length; i++)
            {
                var currentRoman = roman[i];

                int previousInt = previousRoman != '\0' ? romanToIntDictionary[previousRoman] : '\0';
                var currentInt = romanToIntDictionary[currentRoman];

                total = currentInt > previousInt ? total - (2 * previousInt) + currentInt : total + currentInt;

                previousRoman = currentRoman;
            }

            return total;
        }

        
    }
}
