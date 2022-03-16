using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresTitularesDasTurmasQueryHandler : IRequestHandler<ObterProfessoresTitularesDasTurmasQuery, IEnumerable<string>>
    {
        private readonly IServicoEol servicoEol;

        public ObterProfessoresTitularesDasTurmasQueryHandler(IServicoEol servicoEol)
        {
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }

        public async Task<IEnumerable<string>> Handle(ObterProfessoresTitularesDasTurmasQuery request, CancellationToken cancellationToken)
        {
            return (await servicoEol.ObterProfessoresTitularesPorTurmas(request.CodigosTurmas))?
                .SelectMany(c => c.ProfessorRf.Split(',').AsEnumerable())
                .Where(c => !string.IsNullOrEmpty(c))
                .Select(a => a.Trim());
        }
    }
}
