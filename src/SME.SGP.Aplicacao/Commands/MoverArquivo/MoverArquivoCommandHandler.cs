using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class MoverArquivoCommandHandler : IRequestHandler<MoverArquivoCommand, string>
    {
        public MoverArquivoCommandHandler()
        {
        }

        public Task<string> Handle(MoverArquivoCommand request, CancellationToken cancellationToken)
        {
            var caminhoBase = UtilArquivo.ObterDiretorioBase();
            var nomeArquivo = Path.GetFileName(request.Nome);
            var caminhoArquivoTemp = Path.Combine(caminhoBase,TipoArquivo.Editor.Name());
            var caminhoArquivoFuncionalidade = Path.Combine(caminhoBase, request.Tipo.Name(), DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            MoverAquivo(caminhoArquivoTemp, caminhoArquivoFuncionalidade, nomeArquivo);

            return Task.FromResult($"/{Path.Combine(request.Tipo.Name(), DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString())}/");
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