using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
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
            var auditoria = await mediator.Send(new AlterarDocumentoCommand(param.DocumentoId, param.CodigoArquivo));

            return auditoria;
        }
    }
}
