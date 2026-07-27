using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PendenciaMap : BaseEntityMap<Pendencia>
    {
        public PendenciaMap()
        {
            ToTable("pendencia");
            Map(nameof(Pendencia.Descricao), "descricao");
            Map(nameof(Pendencia.Situacao), "situacao");
            Map(nameof(Pendencia.Tipo), "tipo");
            Map(nameof(Pendencia.Titulo), "titulo");
            Map(nameof(Pendencia.Instrucao), "instrucao");
            Map(nameof(Pendencia.Excluido), "excluido");
            Map(nameof(Pendencia.DescricaoHtml), "descricao_html");
            Map(nameof(Pendencia.UeId), "ue_id");
            Map(nameof(Pendencia.TurmaId), "turma_id");
            Map(nameof(Pendencia.QuantidadeAulas), "qtde_aulas");
            Map(nameof(Pendencia.QuantidadeDias), "qtde_dias");
        }
    }
}