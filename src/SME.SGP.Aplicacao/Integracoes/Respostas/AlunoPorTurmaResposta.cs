using SME.SGP.Dominio;
using System;
using System.Linq;

namespace SME.SGP.Aplicacao.Integracoes.Respostas
{
    public class AlunoPorTurmaResposta
    {
        public string CodigoAluno { get; set; }
        public SituacaoMatriculaAluno CodigoSituacaoMatricula { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataSituacao { get; set; }
        public string NomeAluno { get; set; }
        public string NomeSocialAluno { get; set; }
        public int NumeroAlunoChamada { get; set; }
        public bool PossuiDeficiencia { get; set; }
        public string SituacaoMatricula { get; set; }
        public string EscolaTransferencia { get; set; }
        public string TurmaTransferencia { get; set; }
        public string TurmaRemanejamento { get; set; }
        public bool Transferencia_Interna { get; set; }

        public bool EstaInativo()
            => !(new[] { SituacaoMatriculaAluno.Ativo,  
                        SituacaoMatriculaAluno.Rematriculado,
                        SituacaoMatriculaAluno.PendenteRematricula,
                        SituacaoMatriculaAluno.SemContinuidade
            }).Contains(CodigoSituacaoMatricula);
    }
}