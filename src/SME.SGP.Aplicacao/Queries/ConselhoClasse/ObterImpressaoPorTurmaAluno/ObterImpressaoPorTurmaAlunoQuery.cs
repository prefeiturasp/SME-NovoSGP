using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao.Queries.ConselhoClasse.ObterImpressaoPorTurmaAluno
{
    public class ObterImpressaoPorTurmaAlunoQuery : IRequest
    {
        public long TurmaId { get; set; }

        public string AlunoCodigo { get; set; }
    }
}
