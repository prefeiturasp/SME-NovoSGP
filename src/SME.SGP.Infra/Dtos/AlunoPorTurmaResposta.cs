using SME.SGP.Dominio;
using System;
using System.Linq;

namespace SME.SGP.Infra
{
    public class AlunoPorTurmaResposta
    {
        public int Ano { get; set; }
        public string CodigoAluno { get; set; }
        public int? CodigoComponenteCurricular { get; set; }
        public SituacaoMatriculaAluno CodigoSituacaoMatricula { get; set; }
        public long CodigoTurma { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataSituacao { get; set; }
        public string EscolaTransferencia { get; set; }
        public string NomeAluno { get; set; }
        public string NomeSocialAluno { get; set; }
        public int NumeroAlunoChamada { get; set; }
        public char? ParecerConclusivo { get; set; }
        public bool PossuiDeficiencia { get; set; }
        public string SituacaoMatricula { get; set; }
        public bool Transferencia_Interna { get; set; }
        public string TurmaEscola { get; set; }
        public string TurmaRemanejamento { get; set; }
        public string TurmaTransferencia { get; set; }

        public bool DeveMostrarNaChamada()
        {
            return !EstaInativo() || (EstaInativo() || NumeroAlunoChamada > 0);
        }

        public bool EstaInativo()
            => !(new[] { SituacaoMatriculaAluno.Ativo,
                        SituacaoMatriculaAluno.Rematriculado,
                        SituacaoMatriculaAluno.PendenteRematricula,
                        SituacaoMatriculaAluno.SemContinuidade
            }).Contains(CodigoSituacaoMatricula);

        public string NomeValido()
        {
            return string.IsNullOrEmpty(NomeSocialAluno) ? NomeAluno : NomeSocialAluno;
        }
    }
}