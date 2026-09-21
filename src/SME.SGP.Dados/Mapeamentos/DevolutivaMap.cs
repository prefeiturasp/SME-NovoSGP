using SME.SGP.Dados.Mapeamentos;
using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class DevolutivaMap : BaseMap<Devolutiva>
    {
        public DevolutivaMap()
        {
            ToTable("devolutiva");
            Map(nameof(Devolutiva.Descricao), "descricao");
            Map(nameof(Devolutiva.CodigoComponenteCurricular), "componente_curricular_codigo");
            Map(nameof(Devolutiva.PeriodoInicio), "periodo_inicio");
            Map(nameof(Devolutiva.PeriodoFim), "periodo_fim");
            Map(nameof(Devolutiva.Excluido), "excluido");
        }
    }
}