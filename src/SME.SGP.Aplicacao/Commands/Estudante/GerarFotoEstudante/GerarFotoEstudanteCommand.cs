using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class GerarFotoEstudanteCommand : IRequest<long>
    {
        public GerarFotoEstudanteCommand(string alunoCodigo, long arquivoId, long? miniaturaId = null)
        {
            AlunoCodigo = alunoCodigo;
            ArquivoId = arquivoId;
            MiniaturaId = miniaturaId;
        }

        public string AlunoCodigo { get; }
        public long ArquivoId { get; }
        public long? MiniaturaId { get; }
    }
}
