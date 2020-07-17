using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorTurmaECodigoUeQueryHandler : IRequestHandler<ObterComponentesCurricularesPorTurmaECodigoUeQuery, IEnumerable<ComponenteCurricularDto>>
    {
        private readonly IServicoEol servicoEOL;
        public ObterComponentesCurricularesPorTurmaECodigoUeQueryHandler(IServicoEol servicoEOL)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<IEnumerable<ComponenteCurricularDto>> Handle(ObterComponentesCurricularesPorTurmaECodigoUeQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
