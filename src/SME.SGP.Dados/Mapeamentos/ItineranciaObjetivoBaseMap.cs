using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ItineranciaObjetivoBaseMap : SimpleMap<ItineranciaObjetivoBase>
    {
        public ItineranciaObjetivoBaseMap()
        {
            ToTable("itinerancia_objetivo_base");
            Map(nameof(ItineranciaObjetivoBase.Nome), "nome");
            Map(nameof(ItineranciaObjetivoBase.TemDescricao), "tem_descricao");
            Map(nameof(ItineranciaObjetivoBase.Ordem), "ordem");
            Map(nameof(ItineranciaObjetivoBase.Excluido), "excluido");
        }
    }
}