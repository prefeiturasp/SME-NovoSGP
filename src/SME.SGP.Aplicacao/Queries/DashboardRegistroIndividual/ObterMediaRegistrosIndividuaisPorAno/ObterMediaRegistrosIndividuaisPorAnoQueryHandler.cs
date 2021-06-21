
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterMediaRegistrosIndividuaisPorAnoQueryHandler : IRequestHandler<ObterMediaRegistrosIndividuaisPorAnoQuery, IEnumerable<GraficoBaseQuantidadeDoubleDto>>
    {
        private readonly IRepositorioConsolidacaoRegistroIndividualMedia repositorioConsolidacaoRegistroIndividualMedia;

        public ObterMediaRegistrosIndividuaisPorAnoQueryHandler(IRepositorioConsolidacaoRegistroIndividualMedia repositorioConsolidacaoRegistroIndividualMedia)
        {
            this.repositorioConsolidacaoRegistroIndividualMedia = repositorioConsolidacaoRegistroIndividualMedia ?? throw new System.ArgumentNullException(nameof(repositorioConsolidacaoRegistroIndividualMedia));
        }

        public async Task<IEnumerable<GraficoBaseQuantidadeDoubleDto>> Handle(ObterMediaRegistrosIndividuaisPorAnoQuery request, CancellationToken cancellationToken)
        {
            var dados = await repositorioConsolidacaoRegistroIndividualMedia.ObterRegistrosItineranciasMediaPorAnoAsync(request.AnoLetivo, request.DreId, request.Modalidade);
            return ObterDadosDto(dados);
        }

        private IEnumerable<GraficoBaseQuantidadeDoubleDto> ObterDadosDto(IEnumerable<RegistroItineranciaMediaPorAnoDto> dadosPorAno)
        {
            var listaDto = new List<GraficoBaseQuantidadeDoubleDto>();
            foreach(var registro in dadosPorAno)
            {
                listaDto.Add(new GraficoBaseQuantidadeDoubleDto() {
                    Quantidade = Math.Round(registro.Quantidade),
                    Descricao = $"{registro.Modalidade.ShortName()}-{registro.Ano}"
                }
                );
            }
            return listaDto;
        }
    }
}
