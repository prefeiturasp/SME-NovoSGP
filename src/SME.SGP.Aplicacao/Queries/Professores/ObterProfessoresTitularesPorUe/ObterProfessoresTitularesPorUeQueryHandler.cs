using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresTitularesPorUeQueryHandler : IRequestHandler<ObterProfessoresTitularesPorUeQuery, IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        private readonly IServicoEol _servicoEol;

        public ObterProfessoresTitularesPorUeQueryHandler(IServicoEol servicoEol)
        {
            _servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }

        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> Handle(ObterProfessoresTitularesPorUeQuery request, CancellationToken cancellationToken)
        {
            return await _servicoEol.ObterProfessoresTitularesPorUe(request.UeCodigo, request.DataReferencia);
        }
    }
}