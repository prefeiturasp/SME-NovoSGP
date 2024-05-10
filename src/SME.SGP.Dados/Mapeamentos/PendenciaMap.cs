using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PendenciaMap : BaseMap<Pendencia>
    {
        public PendenciaMap()
        {
            ToTable("pendencia");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.Situacao).ToColumn("situacao");
            Map(c => c.Tipo).ToColumn("tipo");
            Map(c => c.Titulo).ToColumn("titulo");
            Map(c => c.Instrucao).ToColumn("instrucao");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.DescricaoHtml).ToColumn("descricao_html");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.QuantidadeAulas).ToColumn("qtde_aulas");
            Map(c => c.QuantidadeDias).ToColumn("qtde_dias");
            Map(c => c.PendenciaAssunto).Ignore();
        }
    }
}
