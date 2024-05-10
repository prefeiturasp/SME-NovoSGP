using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarDocumentoUseCase : AbstractUseCase, IAlterarDocumentoUseCase
    {
        public AlterarDocumentoUseCase(IMediator mediator): base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(AlterarDocumentoDto param)
        {
            var auditoria = await mediator.Send(new AlterarDocumentoCommand(param.DocumentoId, param.ArquivosCodigos));

            return auditoria;
        }
    }
}
