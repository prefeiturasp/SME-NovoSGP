using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class BuscaCepUseCase : AbstractUseCase, IBuscaCepUseCase
    {
        public BuscaCepUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<CepDto> Executar(string cep)
        {
            return await mediator.Send(new ObterCepQuery(cep));
        }
    }
}