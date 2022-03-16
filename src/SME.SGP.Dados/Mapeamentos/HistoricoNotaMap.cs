using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class HistoricoNotaMap : BaseMap<HistoricoNota>
    {
        public HistoricoNotaMap()
        {
            ToTable("historico_nota");
            Map(c => c.NotaAnterior).ToColumn("nota_anterior");
            Map(c => c.NotaNova).ToColumn("nota_nova");
            Map(c => c.ConceitoAnteriorId).ToColumn("conceito_anterior_id");
            Map(c => c.ConceitoNovoId).ToColumn("conceito_novo_id");
        }
    }
}
