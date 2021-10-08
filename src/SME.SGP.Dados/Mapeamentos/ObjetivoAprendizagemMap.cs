using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ObjetivoAprendizagemMap : DommelEntityMap<ObjetivoAprendizagem>
    {
        public ObjetivoAprendizagemMap()
        {
            ToTable("objetivo_aprendizagem");
            Map(c => c.Ano).Ignore();
            Map(c => c.AnoTurma).ToColumn("ano_turma");
            Map(c => c.AtualizadoEm).ToColumn("atualizado_em");
            Map(c => c.CodigoCompleto).ToColumn("codigo_completo");
            Map(c => c.ComponenteCurricularId).ToColumn("componente_curricular_id");
            Map(c => c.CriadoEm).ToColumn("criado_em");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.Id).ToColumn("id");
        }
    }
}