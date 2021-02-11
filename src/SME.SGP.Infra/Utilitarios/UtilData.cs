using System;
using System.Globalization;

namespace SME.SGP.Infra.Utilitarios
{
    public static class UtilData
    {
        public static int ObterSemanaDoAno(DateTime data)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;

            return dfi.Calendar.GetWeekOfYear(data, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

        public static int ObterDiferencaDeMeses(DateTime dataInicial, DateTime dataFinal) 
        {
            int incremento = 0;
            int meses = 0;
            if ((dataInicial.Month + meses) > dataFinal.Month)
            {
                meses = (dataFinal.Month + 12) - (dataInicial.Month + incremento);
                incremento = 1;
            }
            else
            {
                meses = (dataFinal.Month) - (dataInicial.Month + incremento);
                incremento = 0;
            }
            return meses;
        }
    }
}
