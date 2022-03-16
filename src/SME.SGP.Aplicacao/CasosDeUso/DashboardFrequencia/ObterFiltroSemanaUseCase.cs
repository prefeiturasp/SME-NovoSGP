using MediatR;
using SME.SGP.Dominio;
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
                for (int dia = 0; dia < diasNoMes && primeiroDiaMes.AddDays(dia) <= dataReferencia; dia++)
                {
                    if (primeiroDiaMes.AddDays(dia).DayOfWeek == DayOfWeek.Monday)
                    {
                        var inicio = primeiroDiaMes.AddDays(dia);
                        semanas.Add(new FiltroSemanaDto()
                        {
                            Inicio = inicio,
                            Fim = inicio.AddDays(6) <= dataReferencia ? inicio.AddDays(6) : dataReferencia
                        });
                    }
                }
            }

            return semanas.OrderByDescending(c => c.Inicio).ToList();
        }
    }
}
