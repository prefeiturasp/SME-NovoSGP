using MediatR;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtribuirResponsavelGeralDoPlanoUseCase : AbstractUseCase, IAtribuirResponsavelGeralDoPlanoUseCase
    {
        public AtribuirResponsavelGeralDoPlanoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long planoAEEId, string responsavelRF, string responsavelNome)
        {
            return await mediator.Send(new AtribuirResponsavelGeralDoPlanoCommand(planoAEEId, responsavelRF, responsavelNome));
        }
    }
}
