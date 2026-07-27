using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class HistoricoNotaMap : BaseEntityMap<HistoricoNota>
    {
        public HistoricoNotaMap()
        {
            ToTable("historico_nota");
            Map(nameof(HistoricoNota.NotaAnterior), "nota_anterior");
            Map(nameof(HistoricoNota.NotaNova), "nota_nova");
            Map(nameof(HistoricoNota.ConceitoAnteriorId), "conceito_anterior_id");
            Map(nameof(HistoricoNota.ConceitoNovoId), "conceito_novo_id");
        }
    }
}