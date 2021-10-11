using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class MoverArquivoCommandHandler : IRequestHandler<MoverArquivoCommand, bool>
    {
        public MoverArquivoCommandHandler()
        {
        }

        public Task<bool> Handle(MoverArquivoCommand request, CancellationToken cancellationToken)
        {
            var caminhoBase = UtilArquivo.ObterDiretorioBase();
            var nomeArquivo = Path.GetFileName(request.Nome);
            var caminhoArquivoTemp = Path.Combine(caminhoBase,TipoArquivo.Temp.Name());
            var caminhoArquivoFuncionalidade = Path.Combine(caminhoBase,request.Tipo.Name());
            MoverAquivo(caminhoArquivoTemp, caminhoArquivoFuncionalidade, nomeArquivo);

            return Task.FromResult(true);
        }
        private void MoverAquivo(string caminhoArquivoTemp, string caminhoArquivoFuncionalidade,string nomeArquivo)
        {
            if (!Directory.Exists(caminhoArquivoFuncionalidade))
                Directory.CreateDirectory(caminhoArquivoFuncionalidade);
            File.Move(Path.Combine(caminhoArquivoTemp,nomeArquivo), Path.Combine(caminhoArquivoFuncionalidade, nomeArquivo));
        }
    }
}