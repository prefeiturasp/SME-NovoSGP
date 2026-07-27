using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ComunicadoMap : BaseEntityMap<Comunicado>
    {
        public ComunicadoMap()
        {
            ToTable("comunicado");
            Map(nameof(Comunicado.AnoLetivo), "ano_letivo");
            Map(nameof(Comunicado.CodigoDre), "codigo_dre");
            Map(nameof(Comunicado.CodigoUe), "codigo_ue");
            Map(nameof(Comunicado.Turmas), "turma");
            Map(nameof(Comunicado.AlunoEspecificado), "alunos_especificados");
            Map(nameof(Comunicado.DataEnvio), "data_envio");
            Map(nameof(Comunicado.DataExpiracao), "data_expiracao");
            Map(nameof(Comunicado.Descricao), "descricao");
            Map(nameof(Comunicado.Excluido), "excluido");
            Map(nameof(Comunicado.Modalidades), "modalidades");
            Map(nameof(Comunicado.TiposEscolas), "tipos_escolas");
            Map(nameof(Comunicado.AnosEscolares), "anos_escolares");
            Map(nameof(Comunicado.Semestre), "semestre");
            Map(nameof(Comunicado.TipoComunicado), "tipo_comunicado");
            Map(nameof(Comunicado.Titulo), "titulo");
            Map(nameof(Comunicado.SeriesResumidas), "series_resumidas");
            Map(nameof(Comunicado.TipoCalendarioId), "tipo_calendario_id");
            Map(nameof(Comunicado.EventoId), "evento_id");
        }
    }
}