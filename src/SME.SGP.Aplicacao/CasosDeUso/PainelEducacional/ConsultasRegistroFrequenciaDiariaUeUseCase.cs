using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaDiaria;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaDiaria;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasRegistroFrequenciaDiariaUeUseCase : IConsultasRegistroFrequenciaDiariaUeUseCase
    {
        private readonly IMediator mediator;

        public ConsultasRegistroFrequenciaDiariaUeUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public Task<FrequenciaDiariaUeDto> ObterFrequenciaDiariaPorUe(FiltroFrequenciaDiariaUeDto filtro)
        {
            return mediator.Send(new PainelEducacionalRegistroFrequenciaDiariaUeQuery(filtro));
        }
    }
}
