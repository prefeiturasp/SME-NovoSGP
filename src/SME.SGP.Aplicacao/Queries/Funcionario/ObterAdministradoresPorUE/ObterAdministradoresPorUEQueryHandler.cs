using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAdministradoresPorUEQueryHandler : IRequestHandler<ObterAdministradoresPorUEQuery, string[]>
    {
        private readonly IServicoEol servicoEol;

        public ObterAdministradoresPorUEQueryHandler(IServicoEol servicoEol)
        {
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }

        public async Task<string[]> Handle(ObterAdministradoresPorUEQuery request, CancellationToken cancellationToken)
                      => await servicoEol.ObterAdministradoresSGP(request.CodigoUe);
    }
}
