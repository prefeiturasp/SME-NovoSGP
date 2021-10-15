using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class MoverArquivoCommandHandler : IRequestHandler<MoverArquivoCommand, string>
    {
        private readonly IRepositorioArquivo repositorioArquivo;

        public MoverArquivoCommandHandler(IRepositorioArquivo repositorioArquivo)
        {
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
        }

        public async Task<string> Handle(MoverArquivoCommand request, CancellationToken cancellationToken)
        {
            var caminhoBase = UtilArquivo.ObterDiretorioBase();
            var nomeArquivo = Path.GetFileName(request.Nome);
            var caminhoArquivoTemp = Path.Combine(caminhoBase, TipoArquivo.Editor.Name());
            var caminhoArquivoFuncionalidade = Path.Combine(caminhoBase, request.Tipo.Name(), DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            MoverAquivo(caminhoArquivoTemp, caminhoArquivoFuncionalidade, nomeArquivo);
            await AlterarTipoArquivo(request.Tipo,request.Nome);

            return  await Task.FromResult($"/{Path.Combine(request.Tipo.Name(), DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString())}/");
        }

        private async Task AlterarTipoArquivo(TipoArquivo tipo, string nomeArquivo)
        {
            try
            {
                var arquivo = await repositorioArquivo.ObterPorCodigo(new Guid(nomeArquivo.Split('.').FirstOrDefault()));
                if (arquivo != null)
                {
                    arquivo.Tipo = tipo;
                    await repositorioArquivo.SalvarAsync(arquivo);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void MoverAquivo(string caminhoArquivoTemp, string caminhoArquivoFuncionalidade,string nomeArquivo)
        {
            if (!Directory.Exists(caminhoArquivoFuncionalidade))
                Directory.CreateDirectory(caminhoArquivoFuncionalidade);

            var nomeArquivoCompleto = Path.Combine(caminhoArquivoTemp, nomeArquivo);
            if (File.Exists(nomeArquivoCompleto))
                File.Move(nomeArquivoCompleto, Path.Combine(caminhoArquivoFuncionalidade, nomeArquivo));
        }
    }
}