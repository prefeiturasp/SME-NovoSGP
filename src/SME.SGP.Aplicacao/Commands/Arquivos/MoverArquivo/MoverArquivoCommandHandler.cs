using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Aplicacao
{
    public class MoverArquivoCommandHandler : IRequestHandler<MoverArquivoCommand, string>
    {
        private readonly IRepositorioArquivo repositorioArquivo;
        private readonly IServicoArmazenamento servicoArmazenamento;

        public MoverArquivoCommandHandler(IRepositorioArquivo repositorioArquivo, IServicoArmazenamento servicoArmazenamento)
        {
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
        }

        public async Task<string> Handle(MoverArquivoCommand request, CancellationToken cancellationToken)
        {
            var retorno = await servicoArmazenamento.Mover(request.Nome);
            
            await AlterarTipoArquivo(request.Tipo, request.Nome);

            return retorno;
        }

        private async Task AlterarTipoArquivo(TipoArquivo tipo, string nomeArquivo)
        {
            var arquivo = await repositorioArquivo.ObterPorCodigo(new Guid(nomeArquivo.Split('.').FirstOrDefault()));
            if (arquivo.NaoEhNulo())
            {
                arquivo.Tipo = tipo;
                await repositorioArquivo.SalvarAsync(arquivo);
            }
        }

        private void MoverAquivo(string caminhoArquivoTemp, string caminhoArquivoFuncionalidade, string nomeArquivo)
        {
            if (!Directory.Exists(caminhoArquivoFuncionalidade))
                Directory.CreateDirectory(caminhoArquivoFuncionalidade);

            var nomeArquivoCompleto = Path.Combine(caminhoArquivoTemp, nomeArquivo);
            if (File.Exists(nomeArquivoCompleto))
                File.Move(nomeArquivoCompleto, Path.Combine(caminhoArquivoFuncionalidade, nomeArquivo));
        }
    }
}