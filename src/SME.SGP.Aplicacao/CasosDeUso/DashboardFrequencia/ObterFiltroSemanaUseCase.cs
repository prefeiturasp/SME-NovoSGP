using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroSemanaUseCase : IObterFiltroSemanaUseCase
    {

        public ObterFiltroSemanaUseCase()
        {
        }

        public async Task<IEnumerable<FiltroSemanaDto>> Executar(int anoLetivo)
        {
            List<FiltroSemanaDto> semanas;

            semanas = ObterSemanas(anoLetivo);

            return await Task.FromResult(semanas);
        }

        private List<FiltroSemanaDto> ObterSemanas(int anoLetivo)
        {
            List<FiltroSemanaDto> semanas = new List<FiltroSemanaDto>();
            var dataReferencia = anoLetivo == DateTime.Now.Year ? DateTime.Now : new DateTime(anoLetivo, 12, 31);
            for (int mes = 1; mes <= dataReferencia.Month; mes++)
            {
                var diasNoMes = DateTime.DaysInMonth(dataReferencia.Year, mes);
                DateTime primeiroDiaMes = new DateTime(dataReferencia.Year, mes, 1);
                for (int dia = 0; dia < diasNoMes && primeiroDiaMes.AddDays(dia) < dataReferencia; dia++)
                {
                    var inicio = primeiroDiaMes.AddDays(dia);

                    if (inicio.DayOfWeek == DayOfWeek.Monday && inicio.Date != dataReferencia.Date)
                    {
                        var dataFim = inicio.AddDays(6);
                        semanas.Add(new FiltroSemanaDto()
                        {
                            Inicio = inicio,
                            Fim = dataFim <= dataReferencia ? dataFim : dataReferencia
                        });
                        dia += 6;
                    }
                }
            }

            return semanas.OrderByDescending(c => c.Inicio).ToList();
        }
    }
}
