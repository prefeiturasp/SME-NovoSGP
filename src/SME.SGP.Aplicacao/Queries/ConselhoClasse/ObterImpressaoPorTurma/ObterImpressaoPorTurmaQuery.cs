using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao.Queries.ConselhoClasse.ObterImpressaoPorTurma
{
    public class ObterImpressaoPorTurmaQuery : IRequest
    {
        public long TurmaId { get; set; }
    }
}
