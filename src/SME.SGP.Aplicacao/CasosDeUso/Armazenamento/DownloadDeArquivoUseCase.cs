using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class DownloadDeArquivoUseCase : AbstractUseCase, IDownloadDeArquivoUseCase
    {
        public DownloadDeArquivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<(byte[], string, string)> Executar(Guid param)
        {
            var entidadeArquivo = await mediator.Send(new ObterArquivoPorCodigoQuery(param));
            
            var extensao = Path.GetExtension(entidadeArquivo.Nome);

            var nomeArquivoComExtensao = $"{param}{extensao}";

            var arquivoFisico = await mediator.Send(new DownloadArquivoCommand(param, nomeArquivoComExtensao, entidadeArquivo.Tipo));

            return (arquivoFisico, entidadeArquivo.TipoConteudo, entidadeArquivo.Nome);
        }
    }
}
