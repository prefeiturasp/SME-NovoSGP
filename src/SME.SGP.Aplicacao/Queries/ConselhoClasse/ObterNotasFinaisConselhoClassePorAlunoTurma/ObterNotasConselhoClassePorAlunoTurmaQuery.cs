using System.Collections.Generic;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasConselhoClassePorAlunoTurmaQuery : IRequest<IEnumerable<NotaConceitoFechamentoConselhoFinalDto>>
    {
        public ObterNotasConselhoClassePorAlunoTurmaQuery(string[] turmasCodigos, string alunoCodigo)
        {
            TurmasCodigos = turmasCodigos;
            AlunoCodigo = alunoCodigo;
        }

        public string[] TurmasCodigos { get; set; }
        public string AlunoCodigo { get; set; }
    }
}