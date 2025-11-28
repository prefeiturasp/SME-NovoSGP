using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.Aluno;
using System;

namespace SME.SGP.Infra.Dtos
{
    public class AlunoReduzidoDto
    {
        public string CodigoAluno { get; set; }
        public string Nome { get; set; }        
        public int NumeroAlunoChamada { get; set; }
        public DateTime DataNascimento { get; set; }
        public int Idade { get; set; }
        public string DocumentoCpf { get; set; } = null;
        public DateTime DataSituacao { get; set; }
        public SituacaoMatriculaAluno CodigoSituacaoMatricula { get; set; }
        public string Situacao { get; set; }
        public string TurmaEscola { get; set; }
        public string CodigoTurma { get; set; }
        public string NomeResponsavel { get; set; }
        public string TipoResponsavel { get; set; }
        public string CelularResponsavel { get; set; }
        public DateTime? DataAtualizacaoContato { get; set; }
        public bool EhAtendidoAEE { get; set; }
        public bool EhMatriculadoTurmaPAP { get; set; }
        public DadosResponsavelFiliacaoAlunoDto DadosResponsavelFiliacao { get; set; }
    }
}
