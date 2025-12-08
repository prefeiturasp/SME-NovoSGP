using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class AlunoTurmaReduzidoDto
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
        public string Frequencia { get; set; }
        public int NumeroAulasDadas { get; set; }
        public int NumeroAulasFrequentadas { get; set; }
        public string CicloEnsino { get; set; }
        public bool EhBuscaAtiva { get; set; }
        public int NumeroVisitas { get; set; }
        public int NumeroLigacoes { get; set; }
        public int ProgramaAluno { get; set; }
    }
}
