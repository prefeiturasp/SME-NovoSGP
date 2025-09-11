using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class DownloadArquivoCommand : IRequest<byte[]>
    {
        public DownloadArquivoCommand(Guid codigoArquivo, string nome, TipoArquivo tipo)
        {
            Codigo = codigoArquivo;
            Nome = nome;
            Tipo = tipo;
        }

        public Guid Codigo { get; set; }

        public string Nome { get; set; }
        public TipoArquivo Tipo { get; set; }
    }
}
