using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPorCpfUseCase : IObterUsuarioPorCpfUseCase
    {
        private readonly IMediator mediator;

        public ObterUsuarioPorCpfUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<UsuarioEscolaAquiDto> Executar(string codigoDre, long codigoUe, string cpf)
        {
            return await mediator.Send(new ObterUsuarioPorCpfQuery(codigoDre, codigoUe, cpf));
        }
    }
}
