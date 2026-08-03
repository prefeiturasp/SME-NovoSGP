using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class MotivoAusenciaMap : SimpleMap<MotivoAusencia>
    {
        public MotivoAusenciaMap()
        {
            ToTable("motivo_ausencia");
            Map(nameof(MotivoAusencia.Descricao), "descricao");
        }
    }
}