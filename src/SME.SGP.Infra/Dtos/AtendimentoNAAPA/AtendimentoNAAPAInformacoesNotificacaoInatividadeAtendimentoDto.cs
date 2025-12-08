using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class AtendimentoNAAPAInformacoesNotificacaoInatividadeAtendimentoDto
    {
        public long EncaminhamentoId { get; set; }
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public long TurmaId { get; set; }
        public string TurmaNome { get; set; }
        public string UeNome { get; set; }
        public string UeCodigo { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public string DreNome { get; set; }
        public string DreCodigo { get; set; }
    }
}
