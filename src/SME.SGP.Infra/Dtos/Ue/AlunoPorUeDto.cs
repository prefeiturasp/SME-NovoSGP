using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class AlunoPorUeDto
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
    }
}
