using System;
using System.Globalization;

namespace SME.SGP.Infra.Utilitarios
{
    public static class UtilData
    {
        public static int ObterSemanaDoAno(DateTime data)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;

            DayOfWeek day = dfi.Calendar.GetDayOfWeek(data);
            if (day >= DayOfWeek.Sunday && day <= DayOfWeek.Thursday)
            {
                data = data.AddDays(3);
            }

            // Return the week of our adjusted day
            return dfi.Calendar.GetWeekOfYear(data, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
        }
    }
}
