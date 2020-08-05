using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
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
            var diarioBordo = await repositorioDiarioBordo.ObterPorIdAsync(request.DiarioBordoId);

            return diarioBordo;
        }
    }
}
