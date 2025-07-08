using MediatR;
using SME.SGP.Aplicacao.Queries.Armazenamento.ObterComprimir;
using SME.SGP.Dominio;
using SME.SGP.Infra.Interface;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class DownloadDeArquivoUseCase : AbstractUseCase, IDownloadDeArquivoUseCase
    {
        private readonly IServicoArmazenamento servicoArmazenamento;
        public DownloadDeArquivoUseCase(IMediator mediator, IServicoArmazenamento servicoArmazenamento) : base(mediator)
        {
            this.servicoArmazenamento = servicoArmazenamento;
        }

        public async virtual Task<(byte[], string, string)> Executar(Guid codigoArquivo)
        {
            var entidadeArquivo = await ObterArquivoPorCodigo(codigoArquivo);


            var arquivos = await mediator.Send(new ObterComprimirQuery(Convert.ToDateTime("2025-07-03"), Convert.ToDateTime("2025-07-04")));

            foreach (var arquivo in arquivos)
            {
                var partes = arquivo.TipoConteudo.Split('/');
                await servicoArmazenamento.OtimizarArquivos($"{arquivo.Codigo}.{partes[1]}");
            }

            var extensao = Path.GetExtension(entidadeArquivo.Nome);

            var nomeArquivoComExtensao = $"{codigoArquivo}{extensao}";

            var arquivoFisico = await mediator.Send(new DownloadArquivoCommand(codigoArquivo, nomeArquivoComExtensao, entidadeArquivo.Tipo));

            return (arquivoFisico, entidadeArquivo.TipoConteudo, entidadeArquivo.Nome);
        }

        protected virtual Task<Arquivo> ObterArquivoPorCodigo(Guid codigoArquivo)
            => mediator.Send(new ObterArquivoPorCodigoQuery(codigoArquivo));
    }
}
