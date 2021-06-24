using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterMediaRegistrosIndividuaisPorTurmaQueryHandler : IRequestHandler<ObterMediaRegistrosIndividuaisPorTurmaQuery, IEnumerable<GraficoBaseQuantidadeDoubleDto>>
    {
        private readonly IRepositorioConsolidacaoRegistroIndividualMedia repositorioConsolidacaoRegistroIndividualMedia;

        public ObterMediaRegistrosIndividuaisPorTurmaQueryHandler(IRepositorioConsolidacaoRegistroIndividualMedia repositorioConsolidacaoRegistroIndividualMedia)
        {
            this.repositorioConsolidacaoRegistroIndividualMedia = repositorioConsolidacaoRegistroIndividualMedia ?? throw new System.ArgumentNullException(nameof(repositorioConsolidacaoRegistroIndividualMedia));
        }
        public async Task<IEnumerable<GraficoBaseQuantidadeDoubleDto>> Handle(ObterMediaRegistrosIndividuaisPorTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConsolidacaoRegistroIndividualMedia.ObterRegistrosItineranciasMediaPorTurmaAsync(request.AnoLetivo, request.UeId, request.Modalidade);
        }
    }
}
