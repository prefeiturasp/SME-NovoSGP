using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Infra
{
    public class PlanoAEEAlunoTurmaDto
    {
        public long Id { get; set; }
        public string AlunoNumero { get; set; }
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public string TurmaCodigo { get; set; }
        public string TurmaNome { get; set; }
        public int TurmaAno { get; set; }
        public bool PossuiEncaminhamentoAEE { get; set; }
        public Modalidade TurmaModalidade { get; set; }
        public SituacaoPlanoAEE Situacao { get; set; }
        public DateTime CriadoEm { get; set; }
        public int Versao { get; set; }
        public DateTime DataVersao { get; set; }
    }
}
