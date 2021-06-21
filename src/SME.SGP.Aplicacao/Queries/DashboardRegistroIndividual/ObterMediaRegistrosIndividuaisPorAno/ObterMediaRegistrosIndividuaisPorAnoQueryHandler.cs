
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterMediaRegistrosIndividuaisPorAnoQueryHandler : IRequestHandler<ObterMediaRegistrosIndividuaisPorAnoQuery, IEnumerable<GraficoBaseDto>>
    {
        private readonly IRepositorioConsolidacaoRegistroIndividualMedia repositorioConsolidacaoRegistroIndividualMedia;

        public ObterMediaRegistrosIndividuaisPorAnoQueryHandler(IRepositorioConsolidacaoRegistroIndividualMedia repositorioConsolidacaoRegistroIndividualMedia)
        {
            this.repositorioConsolidacaoRegistroIndividualMedia = repositorioConsolidacaoRegistroIndividualMedia ?? throw new System.ArgumentNullException(nameof(repositorioConsolidacaoRegistroIndividualMedia));
        }

        public async Task<IEnumerable<GraficoBaseDto>> Handle(ObterMediaRegistrosIndividuaisPorAnoQuery request, CancellationToken cancellationToken)
        {
            var dados = await repositorioConsolidacaoRegistroIndividualMedia.ObterRegistrosItineranciasMediaPorAnoAsync(request.AnoLetivo, request.DreId, request.Modalidade);
            return ObterDadosDto(dados);
        }

        private IEnumerable<GraficoBaseDto> ObterDadosDto(IEnumerable<RegistroItineranciaMediaPorAnoDto> dadosPorAno)
        {
            var listaDto = new List<GraficoBaseDto>();
            foreach(var registro in dadosPorAno)
            {
                listaDto.Add(new GraficoBaseDto() {
                    Quantidade = registro.Quantidade,
                    Descricao = $"{registro.Modalidade.ShortName()}-{registro.Ano}"
                }
                );
            }
            return listaDto;
        }
    }
}
