using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

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

        public Guid Codigo;
        public string Nome;
        public TipoArquivo Tipo;
    }
}
