using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RegistroFrequenciaMap : BaseMap<RegistroFrequencia>
    {
        public RegistroFrequenciaMap()
        {
            ToTable("registro_frequencia");
            Map(c => c.AulaId).ToColumn("aula_id");
            Map(c => c.Migrado).ToColumn("migrado");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}