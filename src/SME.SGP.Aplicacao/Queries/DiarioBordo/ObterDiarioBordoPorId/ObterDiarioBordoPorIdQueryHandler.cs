using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.DiarioBordo.ObterDiarioBordoPorId
{
    public class ObterDiarioBordoPorIdQueryHandler : IRequestHandler<ObterDiarioBordoPorIdQuery, DiarioBordo>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public ObterDiarioBordoPorIdQueryHandler(IRepositorioDiarioBordo repositorioDiarioBordo)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
        }

        public async Task<DiarioBordo> Handle(ObterDiarioBordoPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioDiarioBordo.ObterPorIdAsync(request.DiarioBordoId);
        }
    }
}
