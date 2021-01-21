using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Funcionario.ObterListaNomePorListaRF
{
    public class ObterListaNomePorListaRFQueryHandler : IRequestHandler<ObterListaNomePorListaRFQuery, IEnumerable<ProfessorResumoDto>>
    {
        private readonly IServicoEol servicoEol;

        public ObterListaNomePorListaRFQueryHandler(IServicoEol servicoEol)
        {
            this.servicoEol = servicoEol;
        }

        public Task<IEnumerable<ProfessorResumoDto>> Handle(ObterListaNomePorListaRFQuery request, CancellationToken cancellationToken)
            => servicoEol.ObterListaNomePorListaRF(request.CodigosRf);
    }
}