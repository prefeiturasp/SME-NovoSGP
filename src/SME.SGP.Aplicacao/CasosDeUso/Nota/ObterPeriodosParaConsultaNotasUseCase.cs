using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosParaConsultaNotasUseCase : IObterPeriodosParaConsultaNotasUseCase
    {
        private readonly IMediator mediator;

        public ObterPeriodosParaConsultaNotasUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<PeriodosParaConsultaNotasDto>> Executar(ObterPeriodosParaConsultaNotasFiltroDto filtro)
        {
            var periodos = await mediator.Send(new ObterPeriodoCalendarioBimestrePorAnoLetivoModalidadeQuery(filtro.AnoLetivo, ObterModalidadeCalendario(filtro.Modalidade), filtro.Semestre));
            if (periodos == null || !periodos.Any())
                throw new NegocioException("Não foram encontrados períodos escolares para esta turma.");

            var listaPeriodos = new List<PeriodosParaConsultaNotasDto>();

            var bimestreAtual = ObterBimestreAtual(periodos);

            foreach (var periodo in periodos)
            {
                listaPeriodos.Add(new PeriodosParaConsultaNotasDto(periodo.PeriodoInicio.Ticks, periodo.PeriodoFim.Ticks, periodo.PeriodoEscolarId, periodo.Bimestre, periodo.Bimestre == bimestreAtual));
            }
            return listaPeriodos;
        }
        private int ObterBimestreAtual(IEnumerable<PeriodoCalendarioBimestrePorAnoLetivoModalidadeDto> periodosEscolares)
        {
            var dataPesquisa = DateTimeExtension.HorarioBrasilia();

            var periodoEscolar = periodosEscolares.FirstOrDefault(x => x.PeriodoInicio.Date <= dataPesquisa.Date && x.PeriodoFim.Date >= dataPesquisa.Date);

            if (periodoEscolar == null)
                return 1;
            else return periodoEscolar.Bimestre;
        }
        private static ModalidadeTipoCalendario ObterModalidadeCalendario(Modalidade modalidade)
        {
            return modalidade == Modalidade.EJA ? ModalidadeTipoCalendario.EJA : ModalidadeTipoCalendario.FundamentalMedio;
        }
    }   
}
