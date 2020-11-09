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

        public async Task<(byte[], string, string)> Executar(Guid codigoArquivo)
        {
            var entidadeArquivo = await mediator.Send(new ObterArquivoPorCodigoQuery(codigoArquivo));

            var arquivoFisico = await mediator.Send(new DownloadArquivoCommand(codigoArquivo, entidadeArquivo.Nome, entidadeArquivo.Tipo));

            return (arquivoFisico, entidadeArquivo.TipoConteudo, entidadeArquivo.Nome);
        }

        public async Task<bool> Executar(VerificarUsuarioDocumentoDto param)
        {
            return await mediator.Send(new VerificaUsuarioPossuiArquivoQuery(param.TipoDocumentoId, param.ClassificacaoId, param.UsuarioId));
        }
    }
}
