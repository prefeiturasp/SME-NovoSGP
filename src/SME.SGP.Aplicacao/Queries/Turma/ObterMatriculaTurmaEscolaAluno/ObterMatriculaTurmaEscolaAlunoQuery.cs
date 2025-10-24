using MediatR;
using SME.Pedagogico.Interface.DTO.Turma;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterMatriculaTurmaEscolaAlunoQuery : IRequest<List<AlunoMatriculaTurmaEscolaDto>>
    {
        public ObterMatriculaTurmaEscolaAlunoQuery(int anoLetivo, string codigoDre, int? situacaoMatricula, string[] turmas)
        {
            AnoLetivo = anoLetivo;
            CodigoDre = codigoDre;
            SituacaoMatricula = situacaoMatricula;
            Turmas = turmas;
        }

        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public int? SituacaoMatricula { get; set; }
        public string[] Turmas { get; set; }
    }
}
