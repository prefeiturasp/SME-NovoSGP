using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CopiarArquivoCommandHandler : IRequestHandler<CopiarArquivoCommand, string>
    {
        private readonly IRepositorioArquivo repositorioArquivo;

        public CopiarArquivoCommandHandler(IRepositorioArquivo repositorioArquivo)
        {
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
        }

        public async Task<string> Handle(CopiarArquivoCommand request, CancellationToken cancellationToken)
        {
            var caminhoBase = UtilArquivo.ObterDiretorioBase();
            var nomeArquivo = Path.GetFileName(request.Nome);

            var caminhoArquivoOriginal = Path.Combine(caminhoBase, request.TipoArquivoOriginal.Name(), DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString().PadLeft(2, '0'));
            var caminhoArquivoDestino = Path.Combine(caminhoBase, request.TipoArquivoDestino.Name(), DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString().PadLeft(2, '0'));
            CopiarArquivo(caminhoArquivoOriginal, caminhoArquivoDestino, nomeArquivo);
            await SalvarCopiaArquivo(request.TipoArquivoDestino, request.Nome);
            return $@"/{request.TipoArquivoDestino.Name()}/{DateTime.Now.Year}/{DateTime.Now.Month:00}/";
        }
        private async Task SalvarCopiaArquivo(TipoArquivo tipo, string nomeArquivo)
        {
            var arquivo = await repositorioArquivo.ObterPorCodigo(new Guid(nomeArquivo.Split('.').FirstOrDefault()));
            if (arquivo != null)
            {
                arquivo.Tipo = tipo;
                await repositorioArquivo.SalvarAsync(MapearParaEntidade(tipo, arquivo));
            }
        }
        private Arquivo MapearParaEntidade(TipoArquivo tipo, Arquivo arquivo)
        {
            return new Arquivo
            {
                Nome = arquivo.Nome,
                Codigo = arquivo.Codigo,
                Tipo = tipo,
                TipoConteudo = arquivo.TipoConteudo
            };
        }

        private void CopiarArquivo(string caminhoArquivoOrigem, string caminhoArquivoDestino, string nomeArquivo)
        {
            if (!Directory.Exists(caminhoArquivoDestino))
                Directory.CreateDirectory(caminhoArquivoDestino);

            var pathArquivoDestino = Path.Combine(caminhoArquivoDestino, nomeArquivo);
            var pathArquivoOrigem = Path.Combine(caminhoArquivoOrigem, nomeArquivo);
            if (File.Exists(pathArquivoOrigem) && !File.Exists(pathArquivoDestino))
                File.Copy(pathArquivoOrigem, pathArquivoDestino);
        }
    }
}
