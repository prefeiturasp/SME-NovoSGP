using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadeAnoItineranciaProgramaUseCase : IObterModalidadeAnoItineranciaProgramaUseCase
    {
        private readonly IMediator mediator;

        public ObterModalidadeAnoItineranciaProgramaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<RetornoModalidadesPorAnoItineranciaProgramaDto>> Executar(int anoLetivo, long dreId, long ueId, int modalidade, int semestre)
        {
            var modalidadesPorAnoRetornoDto = new List<RetornoModalidadesPorAnoItineranciaProgramaDto>();
            var modalidades = await mediator.Send(new ObterModalidadesAnosItineranciaProgramaQuery(anoLetivo, dreId, ueId, modalidade, semestre));
            foreach (var item in modalidades)
            {
                var prefixoModalidade = (item.Ano == AnoItinerarioPrograma.EducacaoFisica || 
                    item.Ano == AnoItinerarioPrograma.Itinerario || 
                    item.Ano == AnoItinerarioPrograma.Programa) ? "" : $"{item.Modalidade.ShortName()}-";
                modalidadesPorAnoRetornoDto.Add(new RetornoModalidadesPorAnoItineranciaProgramaDto()
                {
                    ModalidadeAno = $"{prefixoModalidade}{item.Ano.ShortName()}",
                    Ano = (int)item.Ano
                });
            }
            return modalidadesPorAnoRetornoDto.OrderBy(a => a.Ano);
        }
    }
}
