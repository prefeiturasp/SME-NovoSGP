using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaDiarioBordoPorComponenteTurmaCodigoQueryHandler : IRequestHandler<ObterPendenciaDiarioBordoPorComponenteTurmaCodigoQuery, long>
    {

        private readonly IRepositorioPendenciaAulaConsulta repositorioPendenciaAula;

        public ObterPendenciaDiarioBordoPorComponenteTurmaCodigoQueryHandler(IRepositorioPendenciaAulaConsulta repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<long> Handle(ObterPendenciaDiarioBordoPorComponenteTurmaCodigoQuery request, CancellationToken cancellationToken)
        => await repositorioPendenciaAula.ObterPendenciaDiarioBordoPorComponenteTurmaCodigo(request.ComponenteCurricularId, request.TurmaCodigo);
    }
}
