using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeDeAunosSemRegistroPorPeriodoAnoQueryHandler : IRequestHandler<ObterQuantidadeDeAunosSemRegistroPorPeriodoAnoQuery, IEnumerable<GraficoBaseDto>>
    {
        private readonly IRepositorioRegistroIndividual repositorioRegistroIndividual;

        public ObterQuantidadeDeAunosSemRegistroPorPeriodoAnoQueryHandler(IRepositorioRegistroIndividual repositorioRegistroIndividual)
        {
            this.repositorioRegistroIndividual = repositorioRegistroIndividual ?? throw new System.ArgumentNullException(nameof(repositorioRegistroIndividual));
        }

        public async Task<IEnumerable<GraficoBaseDto>> Handle(ObterQuantidadeDeAunosSemRegistroPorPeriodoAnoQuery request, CancellationToken cancellationToken)
        {
            var registros = await repositorioRegistroIndividual.ObterQuantidadeDeAunosSemRegistroPorPeriodoAsync(request.AnoLetivo, request.DreId, request.Modalidade, request.DataInicicial);
            return converterParaListDto(registros);
        }

        private IEnumerable<GraficoBaseDto> converterParaListDto(IEnumerable<RegistroItineranciaPorAnoDto> registros)
        {
            var lista = new List<GraficoBaseDto>();
            foreach(var registro in registros)
            {
                lista.Add(new GraficoBaseDto()
                {
                    Descricao = $"{registro.Modalidade.ShortName()}-{registro.Ano}",
                    Quantidade = registro.Quantidade
                });
            }
            return lista;
        }
    }
}
