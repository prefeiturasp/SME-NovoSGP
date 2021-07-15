using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoAtribuicaoPorUeUseCase : AbstractUseCase, IObterPeriodoAtribuicaoPorUeUseCase
    {
        public ObterPeriodoAtribuicaoPorUeUseCase(IMediator mediator) : base(mediator) { }

        public async Task<PeriodoAtribuicaoEsporadicaDto> Executar(long ueId, int anoLetivo)
        {
            long tipoCalendarioId;

            var ue = await mediator.Send(new ObterUePorIdQuery(ueId));

            if (ue == null)
                throw new NegocioException("UE não encontrada");

            if (ue.EhUnidadeInfantil())
            {
                tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery(Dominio.ModalidadeTipoCalendario.Infantil, anoLetivo, 0));
                if (tipoCalendarioId <= 0)
                    throw new NegocioException("Tipo do calendario não encontrado");
            }
            else
            {
                tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery(Dominio.ModalidadeTipoCalendario.FundamentalMedio, anoLetivo, 0));
                if (tipoCalendarioId <= 0)
                    throw new NegocioException("Tipo do calendario não encontrado");
            }

            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));

            if (periodosEscolares == null && !periodosEscolares.Any())
                throw new NegocioException("Periodos escolares não encontrados");

            periodosEscolares = periodosEscolares.OrderBy(x => x.Bimestre);

            var dto = new PeriodoAtribuicaoEsporadicaDto
            {
                DataInicio = periodosEscolares.First().PeriodoInicio,
                DataFim = periodosEscolares.Last().PeriodoFim
            };

            return dto;
        }
    }
}
