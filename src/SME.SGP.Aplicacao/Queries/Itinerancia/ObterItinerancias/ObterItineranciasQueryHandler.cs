using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterItineranciasQueryHandler : ConsultasBase, IRequestHandler<ObterItineranciasQuery, PaginacaoResultadoDto<ItineranciaRetornoQueryDto>>
    {
        private readonly IRepositorioItinerancia repositorioItinerancia;

        public ObterItineranciasQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioItinerancia repositorioItinerancia) : base(contextoAplicacao)
        {
            this.repositorioItinerancia = repositorioItinerancia ?? throw new ArgumentNullException(nameof(repositorioItinerancia));
        }

        public async Task<PaginacaoResultadoDto<ItineranciaRetornoQueryDto>> Handle(ObterItineranciasQuery request, CancellationToken cancellationToken)
        {
            return await repositorioItinerancia.ObterItineranciasPaginado(request.DreId,
                                                                            request.UeId,
                                                                            request.TurmaId,
                                                                            request.AlunoCodigo,
                                                                            (int?)request.Situacao,
                                                                            request.AnoLetivo,
                                                                            request.DataInicio,
                                                                            request.DataFim,
                                                                            request.CriadoRf,
                                                                            Paginacao);
        }
    }
}
