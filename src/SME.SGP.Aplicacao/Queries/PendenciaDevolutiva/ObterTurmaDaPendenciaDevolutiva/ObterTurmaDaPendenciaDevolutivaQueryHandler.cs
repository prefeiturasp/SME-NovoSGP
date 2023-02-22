using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaDaPendenciaDevolutivaQueryHandler : IRequestHandler<ObterTurmaDaPendenciaDevolutivaQuery, Turma>
    {
        private readonly IRepositorioPendenciaDevolutiva _repositorioPendenciaDevolutiva;

        public ObterTurmaDaPendenciaDevolutivaQueryHandler(IRepositorioPendenciaDevolutiva repositorioPendenciaDevolutiva)
        {
            this._repositorioPendenciaDevolutiva = repositorioPendenciaDevolutiva ?? throw new ArgumentNullException(nameof(repositorioPendenciaDevolutiva));
        }

        public async Task<Turma> Handle(ObterTurmaDaPendenciaDevolutivaQuery request, CancellationToken cancellationToken)
        {
            return await _repositorioPendenciaDevolutiva.ObterTurmaPorPendenciaId(request.PendenciaId);
        }
    }
}
