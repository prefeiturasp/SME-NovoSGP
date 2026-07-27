using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class OcorrenciaMap : BaseEntityMap<Ocorrencia>
    {
        public OcorrenciaMap()
        {
            ToTable("ocorrencia");
            Map(nameof(Ocorrencia.DataOcorrencia), "data_ocorrencia");
            Map(nameof(Ocorrencia.Descricao), "descricao");
            Map(nameof(Ocorrencia.Excluido), "excluido");
            Map(nameof(Ocorrencia.HoraOcorrencia), "hora_ocorrencia");
            Map(nameof(Ocorrencia.OcorrenciaTipoId), "ocorrencia_tipo_id");
            Map(nameof(Ocorrencia.Titulo), "titulo");
            Map(nameof(Ocorrencia.TurmaId), "turma_id");
            Map(nameof(Ocorrencia.UeId), "ue_id");
        }
    }
}