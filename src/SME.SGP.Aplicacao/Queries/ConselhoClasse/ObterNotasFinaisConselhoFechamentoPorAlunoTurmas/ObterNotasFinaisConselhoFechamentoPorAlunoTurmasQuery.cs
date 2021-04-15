using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasFinaisConselhoFechamentoPorAlunoTurmasQuery : IRequest<IEnumerable<NotaConceitoFechamentoConselhoFinalDto>>
    {
        public ObterNotasFinaisConselhoFechamentoPorAlunoTurmasQuery(string[] turmasCodigos, string alunoCodigo)
        {
            TurmasCodigos = turmasCodigos;
            AlunoCodigo = alunoCodigo;
        }

        public string[] TurmasCodigos { get; set; }
        public string AlunoCodigo { get; set; }
    }
}
