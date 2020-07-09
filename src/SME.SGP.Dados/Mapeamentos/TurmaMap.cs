using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class TurmaMap : DommelEntityMap<Turma>
    {
        public TurmaMap()
        {
            ToTable("turma");
            Map(c => c.Ano).ToColumn("ano");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.CodigoTurma).ToColumn("turma_id");
            Map(c => c.DataAtualizacao).ToColumn("data_atualizacao");
            Map(c => c.Id).ToColumn("id");
            Map(c => c.ModalidadeCodigo).ToColumn("modalidade_codigo");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.QuantidadeDuracaoAula).ToColumn("qt_duracao_aula");
            Map(c => c.Semestre).ToColumn("semestre");
            Map(c => c.TipoTurno).ToColumn("tipo_turno");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.ModalidadeTipoCalendario).Ignore();
            Map(c => c.EnsinoEspecial).ToColumn("ensino_especial");
        }
    }
}