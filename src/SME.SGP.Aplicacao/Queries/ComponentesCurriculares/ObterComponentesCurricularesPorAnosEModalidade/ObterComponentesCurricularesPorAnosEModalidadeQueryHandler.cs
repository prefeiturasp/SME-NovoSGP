using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.ComponentesCurriculares.ObterComponentesCurricularesPorAnosEModalidade
{
    public class ObterComponentesCurricularesPorAnosEModalidadeQueryHandler : IRequestHandler<ObterComponentesCurricularesPorAnosEModalidadeQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly IServicoEol servicoEol;

        public ObterComponentesCurricularesPorAnosEModalidadeQueryHandler(IServicoEol servicoEol)
        {
            this.servicoEol = servicoEol ?? throw new System.ArgumentNullException(nameof(servicoEol));
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesPorAnosEModalidadeQuery request, CancellationToken cancellationToken)
        {
            return await servicoEol.ObterComponentesCurricularesPorAnosEModalidade(request.Modalidade, request.AnosEscolares, request.AnoLetivo);
        }
    }
}
