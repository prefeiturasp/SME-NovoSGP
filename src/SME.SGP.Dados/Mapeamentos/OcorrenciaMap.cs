using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class OcorrenciaMap : BaseMap<Ocorrencia>
    {
        public OcorrenciaMap()
        {
            ToTable("ocorrencia");
            Map(x => x.DataOcorrencia).ToColumn("data_ocorrencia");
            Map(x => x.HoraOcorrencia).ToColumn("hora_ocorrencia");
            Map(x => x.OcorrenciaTipoId).ToColumn("ocorrencia_tipo_id");
            Map(x => x.TurmaId).ToColumn("turma_id");
        }
    }
}