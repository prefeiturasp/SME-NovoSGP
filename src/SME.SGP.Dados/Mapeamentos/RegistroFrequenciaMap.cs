using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RegistroFrequenciaMap : BaseMap<RegistroFrequencia>
    {
        public RegistroFrequenciaMap()
        {
            ToTable("registro_frequencia");
            Map(e => e.AulaId).ToColumn("aula_id");
        }
    }
}