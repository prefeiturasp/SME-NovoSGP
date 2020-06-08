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

        private SituacaoMatriculaAluno[] SituacoesAtiva => new[] { SituacaoMatriculaAluno.Ativo,
                        SituacaoMatriculaAluno.Rematriculado,
                        SituacaoMatriculaAluno.PendenteRematricula,
                        SituacaoMatriculaAluno.SemContinuidade
    };

        public bool DeveMostrarNaChamada(DateTime dataAula)
        {
            return EstaAtivo(dataAula) || NumeroAlunoChamada > 0;
        }

        /// <summary>
        /// Verifica se o aluno está ativo
        /// </summary>
        /// <param name="dataBase">Data a se considerar para verificar a situação do aluno, Ex: Data da aula</param>
        /// <returns></returns>
        public bool EstaAtivo(DateTime dataBase) => SituacoesAtiva.Contains(CodigoSituacaoMatricula) || dataBase.Date <= DataSituacao.Date;        

        /// <summary>
        /// Verifica se o aluno está inativo
        /// </summary>
        /// <param name="dataBase">Data a se considerar para verificar a situação do aluno, Ex: Data da aula</param>
        /// <returns></returns>
        public bool EstaInativo(DateTime dataBase) => !EstaAtivo(dataBase);

        public string NomeValido()
        {
            return string.IsNullOrEmpty(NomeSocialAluno) ? NomeAluno : NomeSocialAluno;
        }

        public bool PodeEditarNotaConceito()
        {
            if (CodigoSituacaoMatricula != SituacaoMatriculaAluno.Ativo &&
                CodigoSituacaoMatricula != SituacaoMatriculaAluno.PendenteRematricula &&
                CodigoSituacaoMatricula != SituacaoMatriculaAluno.Rematriculado &&
                CodigoSituacaoMatricula != SituacaoMatriculaAluno.SemContinuidade &&
                CodigoSituacaoMatricula != SituacaoMatriculaAluno.Concluido)
                return false;

            return true;
        }
        public bool PodeEditarNotaConceitoNoPeriodo(PeriodoEscolar periodoEscolar)
        {
            if (!PodeEditarNotaConceito())
            {
                return DataSituacao >= periodoEscolar.PeriodoFim;
            }
            return true;
        }
    }
}