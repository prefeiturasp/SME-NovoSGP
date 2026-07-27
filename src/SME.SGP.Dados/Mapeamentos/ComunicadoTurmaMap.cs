using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ComunicadoTurmaMap : BaseEntityMap<ComunicadoTurma>
    {
        public ComunicadoTurmaMap()
        {
            ToTable("comunicado_turma");
            Map(nameof(ComunicadoTurma.CodigoTurma), "turma_codigo");
            Map(nameof(ComunicadoTurma.ComunicadoId), "comunicado_id");
            Map(nameof(ComunicadoTurma.Excluido), "excluido");
        }
    }
}