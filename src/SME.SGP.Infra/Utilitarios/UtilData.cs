using System;
using System.Globalization;

namespace SME.SGP.Infra.Utilitarios
{
    public static class UtilData
    {
        public static int ObterSemanaDoAno(DateTime data)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;            
            return dfi.Calendar.GetWeekOfYear(data, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }
    }
}
