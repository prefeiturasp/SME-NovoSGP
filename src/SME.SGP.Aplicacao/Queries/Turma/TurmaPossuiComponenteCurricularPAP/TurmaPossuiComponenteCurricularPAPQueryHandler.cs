using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TurmaPossuiComponenteCurricularPAPQueryHandler : IRequestHandler<TurmaPossuiComponenteCurricularPAPQuery, bool>
    {
        private readonly IServicoEol servicoEol;

        public TurmaPossuiComponenteCurricularPAPQueryHandler(IServicoEol servicoEol)
        {
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }

        public async Task<bool> Handle(TurmaPossuiComponenteCurricularPAPQuery request, CancellationToken cancellationToken)
        {
            return await servicoEol.TurmaPossuiComponenteCurricularPAP(request.TurmaCodigo, request.Login, request.Perfil);
        }
    }
}
