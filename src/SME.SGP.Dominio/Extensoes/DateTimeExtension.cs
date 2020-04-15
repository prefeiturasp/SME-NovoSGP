using System;

namespace SME.SGP.Dominio
{
    public static class DateTimeExtension
    {
        private static readonly TimeZoneInfo fusoHorarioBrasil = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

        public static DateTime Local(this DateTime data)
        {
            // TODO Resolver o problema de regionalização no servidor
            //if (data.TimeOfDay.TotalSeconds > 0)
            //    return TimeZoneInfo.ConvertTimeFromUtc(data, fusoHorarioBrasil);
            //else
            return data;
        }

        public static DateTime ObterDomingo(this DateTime data)
        {
            if (data.DayOfWeek == DayOfWeek.Sunday)
                return data;
            int diferenca = (7 + (data.DayOfWeek - DayOfWeek.Sunday)) % 7;
            return data.AddDays(-1 * diferenca).Date;
        }

        public static DateTime ObterSabado(this DateTime data)
        {
            if (data.DayOfWeek == DayOfWeek.Saturday)
                return data;
            int diferenca = (((int)DayOfWeek.Saturday - (int)data.DayOfWeek + 7) % 7);
            return data.AddDays(diferenca);
        }

        public static bool FimDeSemana(this DateTime data)
            => data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday;

        public static int Semestre(this DateTime data)
            => data.Month > 6 ? 2 : 1;

        public static DateTime DiaRetroativo(this DateTime data, int nrDias)
        {
            int contadorDias = nrDias;
            DateTime dataRetorno = data;

            while (contadorDias > 0)
            {
                if (!dataRetorno.FimDeSemana())
                    contadorDias--;

                dataRetorno = dataRetorno.AddDays(-1);
            }

            return dataRetorno;
        }
    }
}