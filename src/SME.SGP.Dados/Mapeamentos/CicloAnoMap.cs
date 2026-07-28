using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class CicloAnoMap : SimpleEntityMap<CicloAno>
    {
        public CicloAnoMap()
        {
            ToTable("tipo_ciclo_ano");
            Map(nameof(CicloAno.CicloId), "tipo_ciclo_id");
            Map(nameof(CicloAno.Modalidade), "modalidade");
            Map(nameof(CicloAno.Ano), "ano");
        }
    }
}