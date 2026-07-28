using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class TurmaMap : SimpleEntityMap<Turma>
    {
        public TurmaMap()
        {
            ToTable("turma");
            Map(nameof(Turma.Ano), "ano");
            Map(nameof(Turma.AnoLetivo), "ano_letivo");
            Map(nameof(Turma.CodigoTurma), "turma_id");
            Map(nameof(Turma.TipoTurma), "tipo_turma");
            Map(nameof(Turma.DataAtualizacao), "data_atualizacao");
            Map(nameof(Turma.ModalidadeCodigo), "modalidade_codigo");
            Map(nameof(Turma.Nome), "nome");
            Map(nameof(Turma.QuantidadeDuracaoAula), "qt_duracao_aula");
            Map(nameof(Turma.Semestre), "semestre");
            Map(nameof(Turma.TipoTurno), "tipo_turno");
            Map(nameof(Turma.SerieEnsino), "serie_ensino");
            Map(nameof(Turma.UeId), "ue_id");
            Map(nameof(Turma.NomeFiltro), "nome_filtro");
            Map(nameof(Turma.Historica), "historica");
            Map(nameof(Turma.EnsinoEspecial), "ensino_especial");
            Map(nameof(Turma.DataInicio), "data_inicio");
            Map(nameof(Turma.DataFim), "dt_fim_eol");
            Map(nameof(Turma.EtapaEJA), "etapa_eja");
        }
    }
}