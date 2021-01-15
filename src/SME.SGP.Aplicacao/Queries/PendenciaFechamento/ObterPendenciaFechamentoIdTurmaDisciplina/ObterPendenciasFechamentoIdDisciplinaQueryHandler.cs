using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasFechamentoIdDisciplinaQueryHandler : IRequestHandler<ObterPendenciasFechamentoIdDisciplinaQuery, IEnumerable<PendenciaFechamento>>
    {
        private readonly IRepositorioPendenciaFechamento repositorioPendenciaFechamento;

        public ObterPendenciasFechamentoIdDisciplinaQueryHandler(IRepositorioPendenciaFechamento repositorioPendenciaFechamento)
        {
            this.repositorioPendenciaFechamento = repositorioPendenciaFechamento ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamento));
        }

        public async Task<IEnumerable<PendenciaFechamento>> Handle(ObterPendenciasFechamentoIdDisciplinaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPendenciaFechamento.ObterPorFechamentoIdDisciplinaId(request.FechamentoId, request.DisciplinaId);
        }
    }
}
