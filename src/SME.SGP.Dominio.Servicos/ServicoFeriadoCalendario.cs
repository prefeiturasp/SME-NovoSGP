using SME.SGP.Dominio.Interfaces;
using System;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFeriadoCalendario : IServicoFeriadoCalendario
    {
        private readonly IRepositorioFeriadoCalendario repositorioFeriadoCalendario;

        public ServicoFeriadoCalendario(IRepositorioFeriadoCalendario repositorioFeriadoCalendario)
        {
            this.repositorioFeriadoCalendario = repositorioFeriadoCalendario ?? throw new ArgumentNullException(nameof(repositorioFeriadoCalendario));
        }

        public DateTime CalcularFeriado(int ano, FeriadoEnum feriado)
        {
            DateTime data = CalcularPascoa(ano);

            switch (feriado)
            {
                case FeriadoEnum.Carnaval:
                    return data.AddDays(-47);

                case FeriadoEnum.QuartaCinzas:
                    return data.AddDays(-46);

                case FeriadoEnum.SextaSanta:
                    return data.AddDays(-2);

                case FeriadoEnum.CorpusChristi:
                    return data.AddDays(60);
            }

            return data;
        }

        public void VerficaSeExisteFeriadosMoveis(int ano)
        {
            var feriadosMoveis = repositorioFeriadoCalendario.ObterFeriadosCalendario()
        }

        private static DateTime CalcularPascoa(int ano)
        {
            int r1 = ano % 19;
            int r2 = ano % 4;
            int r3 = ano % 7;
            int r4 = (19 * r1 + 24) % 30;
            int r5 = (6 * r4 + 4 * r3 + 2 * r2 + 5) % 7;
            DateTime dataPascoa = new DateTime(ano, 3, 22).AddDays(r4 + r5);
            int dia = dataPascoa.Day;
            switch (dia)
            {
                case 26:
                    dataPascoa = new DateTime(ano, 4, 19);
                    break;

                case 25:
                    if (r1 > 10)
                        dataPascoa = new DateTime(ano, 4, 18);
                    break;
            }
            return dataPascoa.Date;
        }
    }
}