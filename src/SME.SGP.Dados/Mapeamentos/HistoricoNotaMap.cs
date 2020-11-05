using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class HistoricoNotaMap : BaseMap<HistoricoNota>
    {
        public HistoricoNotaMap()
        {
            ToTable("historico_nota");
            Map(e => e.ConceitoAnteriorId).ToColumn("conceito_anterior_id");
            Map(e => e.ConceitoNovoId).ToColumn("conceito_novo_id");
            Map(e => e.NotaAnterior).ToColumn("nota_anterior");
            Map(e => e.NotaNova).ToColumn("nota_nova");
        }
    }
}
