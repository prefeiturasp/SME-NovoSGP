using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
            DateTime pascoa = CalcularPascoa(ano);

            switch (feriado)
            {
                case FeriadoEnum.Carnaval:
                    return pascoa.AddDays(-47);

                case FeriadoEnum.SextaSanta:
                    return pascoa.AddDays(-2);

                case FeriadoEnum.CorpusChristi:
                    return pascoa.AddDays(60);

                case FeriadoEnum.Pascoa:
                    return pascoa;
            }

            return pascoa;
        }

        public void VerficaSeExisteFeriadosMoveisEInclui(int ano)
        {
            var feriadosMoveis = repositorioFeriadoCalendario.ObterFeriadosCalendario(new FiltroFeriadoCalendarioDto()
            {
                Tipo = TipoFeriadoCalendario.Movel,
                Ano = ano
            }).Result;

            if (feriadosMoveis == null || !feriadosMoveis.Any())
            {
                IncluirFeriadosMoveis(ano);
            }
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

        private void IncluiFeriadoMovel(DateTime dataFeriado, FeriadoEnum feriado)
        {
            var feriadoMovel = new FeriadoCalendario()
            {
                Abrangencia = AbrangenciaFeriadoCalendario.Nacional,
                DataFeriado = dataFeriado,
                Nome = feriado.ObterAtributo<DisplayAttribute>().Name,
                Tipo = TipoFeriadoCalendario.Movel
            };

            repositorioFeriadoCalendario.Salvar(feriadoMovel);
        }

        private void IncluirFeriadosMoveis(int ano)
        {
            IncluiFeriadoMovel(CalcularFeriado(ano, FeriadoEnum.Carnaval), FeriadoEnum.Carnaval);
            IncluiFeriadoMovel(CalcularFeriado(ano, FeriadoEnum.SextaSanta), FeriadoEnum.SextaSanta);
            IncluiFeriadoMovel(CalcularFeriado(ano, FeriadoEnum.CorpusChristi), FeriadoEnum.CorpusChristi);
            IncluiFeriadoMovel(CalcularFeriado(ano, FeriadoEnum.Pascoa), FeriadoEnum.Pascoa);
        }
    }
}