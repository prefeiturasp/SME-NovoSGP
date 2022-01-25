﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.IO;
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
            var caminhoArquivo = $"{caminhoBase}//{nomeArquivo}".Replace(@"\", @"//");

            if (File.Exists(caminhoArquivo))
                File.Delete(caminhoArquivo);
            else
            {
                var mensagem = $"Arquivo Informado para exclusão não existe no caminho {caminhoArquivo} ";
            }
            return Task.FromResult(true);
        }

        private string ObterCaminhoArquivos(TipoArquivo tipo)
            => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Arquivos", tipo.Name());
    }
}
