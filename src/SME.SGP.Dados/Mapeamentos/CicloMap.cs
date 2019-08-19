using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class CicloMap : BaseMap<Ciclo>
    {
        public CicloMap()
        {
            ToTable("tipo_ciclo");
        }
    }
}