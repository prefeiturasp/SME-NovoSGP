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
        private readonly IMediator mediator;

        public ObterFiltroSemanaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<FiltroSemanaDto>> Executar(int anoLetivo, int modalidade, int semestre)
        {
            List<FiltroSemanaDto> semanas;

            semanas = await ObterSemanas(anoLetivo, modalidade, semestre);

            return await Task.FromResult(semanas);
        }

        private async Task<List<FiltroSemanaDto>> ObterSemanas(int anoLetivo, int modalidade, int semestre)
        {
            List<FiltroSemanaDto> semanas = new List<FiltroSemanaDto>();
            var periodosEscolaresReferentes = await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery((Modalidade) modalidade, anoLetivo, semestre));
            var dataReferencia = anoLetivo == DateTime.Now.Year ? DateTime.Now : new DateTime(anoLetivo, 12, 31);

            DateTime inicioPeriodoEscolar = periodosEscolaresReferentes.Any() && periodosEscolaresReferentes != null
             ? periodosEscolaresReferentes.FirstOrDefault(p => p.Bimestre == 1).PeriodoInicio
             : new DateTime(dataReferencia.Year, 2, 1);

            for (int mes = inicioPeriodoEscolar.Month; mes <= dataReferencia.Month; mes++)
            {
                var diasNoMes = DateTime.DaysInMonth(dataReferencia.Year, mes);
                DateTime primeiroDiaMes = mes == inicioPeriodoEscolar.Month ? inicioPeriodoEscolar.Date : new DateTime(dataReferencia.Year, mes, 1);

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
