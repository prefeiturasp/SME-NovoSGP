using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaParecerConclusivoSituacaoHendler : IRequestHandler<ObterPendenciaParecerConclusivoSituacaoQuery, IEnumerable<ParecerConclusivoSituacaoQuantidadeDto>>
    {
        private readonly IRepositorioConselhoClasseParecerConclusivo repositorioParecer;
        public ObterPendenciaParecerConclusivoSituacaoHendler()
        {

        }
        public async Task<IEnumerable<ParecerConclusivoSituacaoQuantidadeDto>> Handle(ObterPendenciaParecerConclusivoSituacaoQuery request, 
            CancellationToken cancellationToken)
        {
            return await repositorioParecer.ObterParecerConclusivoSituacao(request.UeId,
                request.Ano, request.DreId, request.Modalidade,
                request.Semestre, request.Bimestre);
        }
    }
}
