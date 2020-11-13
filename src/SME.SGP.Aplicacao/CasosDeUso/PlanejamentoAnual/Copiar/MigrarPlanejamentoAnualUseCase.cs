using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class MigrarPlanejamentoAnualUseCase : AbstractUseCase, IMigrarPlanejamentoAnualUseCase
    {     
        public MigrarPlanejamentoAnualUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MigrarPlanejamentoAnualDto dto)
        {
            return await mediator.Send(new MigrarPlanejamentoAnualCommand(dto));          
        }
    }
}
