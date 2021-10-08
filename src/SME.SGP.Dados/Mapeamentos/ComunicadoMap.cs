using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ComunicadoMap : BaseMap<Comunicado>
    {
        public ComunicadoMap()
        {
            ToTable("comunicado");
            Map(c => c.Id).ToColumn("id");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.CodigoDre).ToColumn("codigo_dre");
            Map(c => c.CodigoUe).ToColumn("codigo_ue");
            Map(c => c.Turmas).ToColumn("turma");
            Map(c => c.AlunoEspecificado).ToColumn("alunos_especificados");
            Map(c => c.DataEnvio).ToColumn("data_envio");
            Map(c => c.DataExpiracao).ToColumn("data_expiracao");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.Modalidades).ToColumn("modalidades");
            Map(c => c.TiposEscolas).ToColumn("tipos_escolas");
            Map(c => c.AnosEscolares).ToColumn("anos_escolares");
            Map(c => c.Semestre).ToColumn("semestre");
            Map(c => c.TipoComunicado).ToColumn("tipo_comunicado");
            Map(c => c.Titulo).ToColumn("titulo");
            Map(c => c.SeriesResumidas).ToColumn("series_resumidas");
            Map(c => c.TipoCalendarioId).ToColumn("tipo_calendario_id");
            Map(c => c.EventoId).ToColumn("evento_id");
        }
    }
}