using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDocumentoUseCase : AbstractUseCase, IObterDocumentoUseCase
    {
        public ObterDocumentoUseCase(IMediator mediator) : base(mediator)
        { }

        public async Task<ObterDocumentoResumidoDto> Executar(long documentoId)
        {
            return await mediator.Send(new ObterDocumentoPorIdCompletoQuery(documentoId));
        }
    }
}
