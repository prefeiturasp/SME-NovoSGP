using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class OcorrenciaMap : BaseMap<Ocorrencia>
    {
        public OcorrenciaMap()
        {
            ToTable("ocorrencia");
            Map(c => c.DataOcorrencia).ToColumn("data_ocorrencia");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.HoraOcorrencia).ToColumn("hora_ocorrencia");
            Map(c => c.OcorrenciaTipoId).ToColumn("ocorrencia_tipo_id");
            Map(c => c.Titulo).ToColumn("titulo");
            Map(c => c.TurmaId).ToColumn("turma_id");
        }
    }
}