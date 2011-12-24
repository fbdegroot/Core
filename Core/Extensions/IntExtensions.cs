namespace Core.Extensions
{
    public static class IntExtensions
    {
        public static string ToStringWithOrdinal(this int number) {
            switch (number % 100) {
                case 11:
                case 12:
                case 13:
                    return number + "th";
            }

            switch (number % 10) {
                case 1:
                    return number + "st";
                case 2:
                    return number + "nd";
                case 3:
                    return number + "rd";
                default:
                    return number + "th";
            }
        }

        public static string ToStringPadLeft(this int number) {
            return number.ToString().PadLeft(2, '0');
        }

        public static string ToStringDayName(this int number) {
            switch (number) {
                case 1:
                    return "Monday";
                case 2:
                    return "Tuesday";
                case 3:
                    return "Wednesday";
                case 4:
                    return "Thursday";
                case 5:
                    return "Friday";
                case 6:
                    return "Saturday";
                case 7:
                    return "Sunday";
                default:
                    return null;
            }
        }
    }
}