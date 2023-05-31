using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Infra
{
    public class PlanoAEEAlunoTurmaDto
    {
        public long Id { get; set; }
        public int AlunoNumero { get; set; }
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
        public string RfReponsavel { get; set; }
        public string NomeReponsavel { get; set; }
        public string RfPaaiReponsavel { get; set; }
        public string NomePaaiReponsavel { get; set; }
        public long PlanoAeeVersaoId { get; set; }
        public string UeNome { get; set; }
        public TipoEscola TipoEscola { get; set; }

        public string ObterVersaoPlano()
            => $"v{Versao} ({DataVersao:dd/MM/yyyy})";

        public bool EhAtendidoAEE()
            => (Situacao != SituacaoPlanoAEE.Encerrado && Situacao != SituacaoPlanoAEE.EncerradoAutomaticamente);

        public string NomeTurmaFormatado()
            => $"{TurmaModalidade.ShortName()} - {TurmaNome}";

        public string SituacaoPlano()
            => Situacao != 0 ? Situacao.Name() : "";
    }
}
