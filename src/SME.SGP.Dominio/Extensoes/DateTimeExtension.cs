using System;

namespace SME.SGP.Dominio
{
    public static class DateTimeExtension
    {
        public static DateTime Local(this DateTime data)
        {
            return data;
        }
       
        private static readonly TimeZoneInfo BrasiliaTz = ResolveBrasiliaTimeZone();
        public static DateTime HorarioBrasilia()
        {
            var brasilia = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, BrasiliaTz).DateTime;
            return DateTime.SpecifyKind(brasilia, DateTimeKind.Unspecified);
        }

        public static DateTime ObterDomingo(this DateTime data)
        {
            if (data.DayOfWeek == DayOfWeek.Sunday)
                return data;
            var diferenca = (7 + (data.DayOfWeek - DayOfWeek.Sunday)) % 7;
            return data.AddDays(-1 * diferenca).Date;
        }

        public static DateTime ObterSabado(this DateTime data)
        {
            if (data.DayOfWeek == DayOfWeek.Saturday)
                return data;
            var diferenca = (((int)DayOfWeek.Saturday - (int)data.DayOfWeek + 7) % 7);
            return data.AddDays(diferenca);
        }

        public static bool FimDeSemana(this DateTime data)
            => data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday;

        public static bool Domingo(this DateTime data)
            => data.DayOfWeek == DayOfWeek.Sunday;

        public static int Semestre(this DateTime data)
            => data.Month > 6 ? 2 : 1;

       public static DateTime EnsureUnspecified(DateTime dt) =>
            DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

        public static DateTime DiaRetroativo(this DateTime data, int nrDias)
        {
            var contadorDias = nrDias;
            var dataRetorno = data;

            while (contadorDias > 0)
            {
                if (!dataRetorno.FimDeSemana())
                    contadorDias--;

                dataRetorno = dataRetorno.AddDays(-1);
            }

            return dataRetorno;
        }

        public static bool EhAnoAtual(this DateTime data)
        {
            return data.Year == HorarioBrasilia().Year;
        }
        private static TimeZoneInfo ResolveBrasiliaTimeZone()
        {
            // Windows / Linux
            var ids = new[] { "E. South America Standard Time", "America/Sao_Paulo" };

            foreach (var id in ids)
            {
                try { return TimeZoneInfo.FindSystemTimeZoneById(id); }
                catch(Exception ex)
                {
                    if (ex.InnerException != null) throw ex.InnerException;
                }
            }

            // fallback fixo -03:00
            return TimeZoneInfo.CreateCustomTimeZone(
                "Brasilia_Fallback", TimeSpan.FromHours(-3),
                "Brasilia (fallback)", "Brasilia (fallback)");
        }
    }
}