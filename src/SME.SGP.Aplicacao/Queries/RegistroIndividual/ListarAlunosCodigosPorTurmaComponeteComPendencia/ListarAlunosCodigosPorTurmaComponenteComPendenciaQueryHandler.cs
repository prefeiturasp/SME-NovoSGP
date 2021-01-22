using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarAlunosCodigosPorTurmaComponenteComPendenciaQueryHandler : IRequestHandler<ListarAlunosCodigosPorTurmaComponenteComPendenciaQuery, IEnumerable<long>>
    {
        private readonly IRepositorioPendenciaRegistroIndividual repositorioPendenciaRegistroIndividual;

        public ListarAlunosCodigosPorTurmaComponenteComPendenciaQueryHandler(IRepositorioPendenciaRegistroIndividual repositorioPendenciaRegistroIndividual)
        {
            this.repositorioPendenciaRegistroIndividual = repositorioPendenciaRegistroIndividual ?? throw new ArgumentNullException(nameof(repositorioPendenciaRegistroIndividual));
        }
        public async Task<IEnumerable<long>> Handle(ListarAlunosCodigosPorTurmaComponenteComPendenciaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPendenciaRegistroIndividual.ObterAlunosCodigosComPendenciaAtivosDaTurmaAsync(request.TurmaId);
        }

    }
}
