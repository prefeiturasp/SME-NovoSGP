using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNomeProfessorQueryHandler : IRequestHandler<ObterNomeProfessorQuery, string>
    {
        private readonly IServicoEol servicoEol;

        public ObterNomeProfessorQueryHandler(IServicoEol servicoEol)
        {
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }
        public async Task<string> Handle(ObterNomeProfessorQuery request, CancellationToken cancellationToken)
        {
            return (await servicoEol.ObterNomeProfessorPeloRF(request.RFProfessor));
        }
    }
}
