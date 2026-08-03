using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RegistroFrequenciaMap : BaseMap<RegistroFrequencia>
    {
        public RegistroFrequenciaMap()
        {
            ToTable("registro_frequencia");
            Map(nameof(RegistroFrequencia.AulaId), "aula_id");
            Map(nameof(RegistroFrequencia.Migrado), "migrado");
            Map(nameof(RegistroFrequencia.Excluido), "excluido");
        }
    }
}