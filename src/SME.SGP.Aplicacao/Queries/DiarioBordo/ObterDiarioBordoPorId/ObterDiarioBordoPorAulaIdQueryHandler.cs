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
    public class ObterDiarioBordoPorAulaIdQueryHandler : IRequestHandler<ObterDiarioBordoPorAulaIdQuery, DiarioBordo>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public ObterDiarioBordoPorAulaIdQueryHandler(IRepositorioDiarioBordo repositorioDiarioBordo)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
        }

        public async Task<DiarioBordo> Handle(ObterDiarioBordoPorAulaIdQuery request, CancellationToken cancellationToken)
        {
            var diarioBordo = await repositorioDiarioBordo.ObterPorAulaId(request.AulaId,request.ComponenteCurricularId);

            return diarioBordo;
        }
    }
}
