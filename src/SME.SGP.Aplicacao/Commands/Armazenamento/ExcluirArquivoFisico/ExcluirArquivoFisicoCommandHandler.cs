using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirArquivoFisicoCommandHandler : IRequestHandler<ExcluirArquivoFisicoCommand, bool>
    {
        public ExcluirArquivoFisicoCommandHandler()
        {
        }

        public Task<bool> Handle(ExcluirArquivoFisicoCommand request, CancellationToken cancellationToken)
        {
            var caminhoBase = ObterCaminhoArquivos(request.Tipo);
            var extencao = Path.GetExtension(request.Nome);
            var nomeArquivo = $"{request.Codigo}{extencao}";
            var caminhoArquivo = Path.Combine($"{caminhoBase}", nomeArquivo);

            File.Delete(caminhoArquivo);
            return Task.FromResult(true);
        }

        private string ObterCaminhoArquivos(TipoArquivo tipo)
            => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Arquivos", tipo.ToString());
    }
}
