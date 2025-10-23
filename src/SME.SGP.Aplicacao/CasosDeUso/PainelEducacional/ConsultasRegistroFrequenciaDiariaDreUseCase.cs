using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaDiaria;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaDiaria;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasRegistroFrequenciaDiariaDreUseCase : IConsultasRegistroFrequenciaDiariaDreUseCase
    {
        private readonly IMediator mediator;

        public ConsultasRegistroFrequenciaDiariaDreUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public Task<FrequenciaDiariaDreDto> ObterFrequenciaDiariaPorDre(FiltroFrequenciaDiariaDreDto filtro)
        {
            return mediator.Send(new PainelEducacionalRegistroFrequenciaDiariaDreQuery(filtro));
        }
    }
}
