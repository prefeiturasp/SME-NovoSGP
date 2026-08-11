using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class PendenciaDevolutivaMap : SimpleMap<PendenciaDevolutiva>
    {
        public PendenciaDevolutivaMap()
        {
            ToTable("pendencia_devolutiva");
            Map(nameof(PendenciaDevolutiva.PedenciaId), "pendencia_id");
            Map(nameof(PendenciaDevolutiva.ComponenteCurricularId), "componente_curricular_id");
            Map(nameof(PendenciaDevolutiva.TurmaId), "turma_id");
        }
    }
}