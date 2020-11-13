using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificarUsuarioDocumentoUseCase : AbstractUseCase, IVerificarUsuarioDocumentoUseCase
    {
        public VerificarUsuarioDocumentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(VerificarUsuarioDocumentoDto param)
        {
            return await mediator.Send(new VerificaUsuarioPossuiArquivoQuery(param.TipoDocumentoId, param.ClassificacaoId, param.UsuarioId, param.UeId, param.DocumentoId));
        }
    }
}
