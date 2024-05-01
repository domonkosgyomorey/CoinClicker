namespace CoinClicker
{
    public static class DoubleFormatter
    {

        private static string[] unitNames =  {
            "",
            "Thousand (K)",
            "Million (M)",
            "Billion (B)",
            "Trillion (T)",
            "Quadrillion (Q)",
            "Quintillion (Qu)",
            "Sextillion (S)",
            "Septillion (Se)",
            "Octillion (O)",
            "Nonillion (N)",
            "Decillion (D)",
            "Undecillion (U)",
            "Duodecillion (Dd)",
            "Tredecillion (Td)",
            "Quattuordecillion (Qd)",
            "Quindecillion (QuD)",
            "Sexdecillion (Sd)",
            "Septendecillion (SeD)",
            "Octodecillion (Od)",
            "Novemdecillion (Nd)",
            "Vigintillion (V)",
            "Unvigintillion (Uv)",
            "Duovigintillion (Dv)",
            "Tresvigintillion (Tv)",
            "Quattuor­vigint­illion (Qv)",
            "Quinvigintillion (QuV)",
            "Sexvigintillion (Sv)",
            "Septemvigintillion (SeV)",
            "Octovigintillion (Ov)",
            "Novemvigintillion (Nv)",
            "Trigintillion (Tg)",
            "Untrigintillion (UTg)",
            "Duotrigintillion (DTg)",
            "Trestrigintillion (TTg)",
            "Quattuor­trigint­illion (QTg)",
            "Quintrigintillion (QuTg)",
            "Sextrigintillion (STg)",
            "Septentrigintillion (SeTg)",
            "Octotrigintillion (OTg)",
            "Noventrigintillion (NTg)",
            "Quadragintillion (Qg)",
        };

        private static string[] unitNamesShort = {
            "",
            "K",
            "M",
            "B",
            "T",
            "Q",
            "Qu",
            "S",
            "Se",
            "O",
            "N",
            "D",
            "U",
            "Dd",
            "Td",
            "Qd",
            "QuD",
            "Sd",
            "SeD",
            "Od",
            "Nd",
            "V",
            "Uv",
            "Dv",
            "Tv",
            "­Qv",
            "QuV",
            "Sv",
            "SeV",
            "Ov",
            "Nv",
            "Tg",
            "UTg",
            "DTg",
            "TTg",
            "­QTg",
            "QuTg",
            "STg",
            "SeTg",
            "OTg",
            "NTg",
            "Qg",
        };

        public static string Format(double number)
        {
            if (double.IsInfinity(number) || double.IsNaN(number))
            {
                return number.ToString();
            }

            int suffixIndex = 0;
            while (Math.Abs(number) >= 1000 && suffixIndex < unitNames.Length - 1)
            {
                number /= 1000;
                suffixIndex++;
            }

            string formattedNumber = number.ToString("0.##");

            formattedNumber += " " + unitNames[suffixIndex];

            return formattedNumber;
        }

        public static string FormatShort(double number)
        {
            if (double.IsInfinity(number) || double.IsNaN(number))
            {
                return number.ToString();
            }

            int suffixIndex = 0;
            while (Math.Abs(number) >= 1000 && suffixIndex < unitNamesShort.Length - 1)
            {
                number /= 1000;
                suffixIndex++;
            }

            string formattedNumber = number.ToString("0.##");

            formattedNumber += " " + unitNamesShort[suffixIndex];

            return formattedNumber;
        }
    }
}