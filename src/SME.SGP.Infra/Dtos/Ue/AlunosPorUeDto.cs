using SME.SGP.Dominio;
using System;
using System.Linq;

namespace SME.SGP.Infra
{
    public class AlunosPorUeDto
    {
        private string _nomeAluno;

        public string CodigoAluno { get; set; }

        public string NomeAluno {
            get => !string.IsNullOrWhiteSpace(NomeSocialAluno) ? NomeSocialAluno : _nomeAluno;
            set => _nomeAluno = value;
        }

        public string NomeSocialAluno { get; set; }
        public SituacaoMatriculaAluno CodigoSituacaoMatricula { get; set; }
        public string SituacaoMatricula { get; set; }
        public DateTime DataSituacao { get; set; }
        public long CodigoTurma { get; set; }
        public long CodigoMatricula { get; set; }
        public int AnoLetivo { get; set; }

        private readonly SituacaoMatriculaAluno[] SituacoesAtivas = new[]
        {
            SituacaoMatriculaAluno.Ativo,
            SituacaoMatriculaAluno.PendenteRematricula,
            SituacaoMatriculaAluno.Rematriculado,
            SituacaoMatriculaAluno.SemContinuidade
        };

        public bool EstaAtivo(DateTime dataBase) => (SituacoesAtivas.Contains(CodigoSituacaoMatricula) && DataSituacao.Date <= dataBase) ||
            CodigoSituacaoMatricula.Equals(SituacaoMatriculaAluno.Concluido);
    }
}
