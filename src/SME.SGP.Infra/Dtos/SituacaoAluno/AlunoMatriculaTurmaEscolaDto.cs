using System;

namespace SME.Pedagogico.Interface.DTO.Turma
{
    public class AlunoMatriculaTurmaEscolaDto 
    {
        public long CodigoAluno { get; set; }
        public DateTime DataNascimentoAluno { get; set; }
        public int CodigoSituacaoMatricula { get; set; }
        public string SituacaoMatricula { get; set; }
        public string CodigoTurma { get; set; }
    }
}
