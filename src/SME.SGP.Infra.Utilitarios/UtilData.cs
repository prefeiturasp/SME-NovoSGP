using System;
using System.Globalization;

namespace SME.SGP.Infra.Utilitarios
{
    public static class UtilData
    {
        public static int ObterSemanaDoAno(DateTime data)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            return dfi.Calendar.GetWeekOfYear(data, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
        }

        public static int ObterSemanaDoAnoISO(DateTime data)
        {
            return ISOWeek.GetWeekOfYear(data);
        }

        public static int ObterDiferencaDeMesesEntreDatas(DateTime dataInicial, DateTime dataFinal) 
        {
            int meses = 0;
            if ((dataInicial.Month + meses) > dataFinal.Month)
            {
                meses = (dataFinal.Month + 12) - (dataInicial.Month);
            }
            else
            {
                meses = (dataFinal.Month) - (dataInicial.Month);
            }
            return meses;
        }
    }
}
