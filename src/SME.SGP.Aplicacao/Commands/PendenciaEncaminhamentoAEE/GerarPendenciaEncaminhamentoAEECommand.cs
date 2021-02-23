using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaEncaminhamentoAEECommand : IRequest<bool>
    {
        public GerarPendenciaEncaminhamentoAEECommand(TipoPendencia tipoPendencia, string titulo, string texto)
        {
            TipoPendencia = tipoPendencia;
            Titulo = titulo;
            Texto = texto;
        }

        public TipoPendencia TipoPendencia { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
    }
}
