using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Infra
{
    public class TurmasDoAlunoDto
    {
        public int CodigoAluno { get; set; }
        public int AnoLetivo { get; set; }
        public string NomeAluno { get; set; }
        public string NomeSocialAluno { get; set; }
        public int CodigoSituacaoMatricula { get; set; }
        public string SituacaoMatricula { get; set; }
        public DateTime DataSituacao { get; set; }
        public string DataNascimento { get; set; }
        public int NumeroAlunoChamada { get; set; }
        public int CodigoTurma { get; set; }
        public int CodigoTipoTurma { get; set; }
        public string ObterNomeFinalAluno()
        {
            if (!string.IsNullOrEmpty(NomeSocialAluno))
                return NomeSocialAluno;
            else return NomeAluno;
        }
        public string ObterNomeComNumeroChamada()
        {
            return $"{NumeroAlunoChamada} - {ObterNomeFinalAluno()}";
        }

        public bool Inativo => !new int[] { (int)SituacaoMatriculaAluno.Ativo,
                             (int)SituacaoMatriculaAluno.PendenteRematricula,
                             (int)SituacaoMatriculaAluno.Rematriculado,
                             (int)SituacaoMatriculaAluno.SemContinuidade,
                             (int)SituacaoMatriculaAluno.Concluido }.Contains(this.CodigoSituacaoMatricula);
    }
}
