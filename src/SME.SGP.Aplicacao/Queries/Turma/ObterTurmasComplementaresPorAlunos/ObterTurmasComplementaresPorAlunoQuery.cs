using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComplementaresPorAlunoQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterTurmasComplementaresPorAlunoQuery(string[] alunosCodigos)
        {
            AlunosCodigos = alunosCodigos;
        }
        public string[] AlunosCodigos { get; set; }
    }
}
