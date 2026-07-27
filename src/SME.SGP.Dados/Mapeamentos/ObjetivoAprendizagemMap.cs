using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ObjetivoAprendizagemMap : SimpleEntityMap<ObjetivoAprendizagem>
    {
        public ObjetivoAprendizagemMap()
        {
            ToTable("objetivo_aprendizagem");
            Map(nameof(ObjetivoAprendizagem.AnoTurma), "ano_turma");
            Map(nameof(ObjetivoAprendizagem.AtualizadoEm), "atualizado_em");
            Map(nameof(ObjetivoAprendizagem.CodigoCompleto), "codigo");
            Map(nameof(ObjetivoAprendizagem.ComponenteCurricularId), "componente_curricular_id");
            Map(nameof(ObjetivoAprendizagem.CriadoEm), "criado_em");
            Map(nameof(ObjetivoAprendizagem.Descricao), "descricao");
            Map(nameof(ObjetivoAprendizagem.Excluido), "excluido");
        }
    }
}