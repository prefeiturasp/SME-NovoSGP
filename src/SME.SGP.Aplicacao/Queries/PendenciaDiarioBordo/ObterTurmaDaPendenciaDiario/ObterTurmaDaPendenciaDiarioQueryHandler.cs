using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaDaPendenciaDiarioQueryHandler : IRequestHandler<ObterTurmaDaPendenciaDiarioQuery, Turma>
    {
        private readonly IRepositorioPendenciaAulaConsulta repositorioPendenciaAula;

        public ObterTurmaDaPendenciaDiarioQueryHandler(IRepositorioPendenciaAulaConsulta repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<Turma> Handle(ObterTurmaDaPendenciaDiarioQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaAula.ObterTurmaPorPendenciaDiario(request.PendenciaId);
    }
}
