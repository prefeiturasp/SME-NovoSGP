using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarDiarioBordoComDevolutivaCommandHandler : IRequestHandler<AtualizarDiarioBordoComDevolutivaCommand, bool>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public AtualizarDiarioBordoComDevolutivaCommandHandler(IRepositorioDiarioBordo repositorioDiarioBordo)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
        }

        public async Task<bool> Handle(AtualizarDiarioBordoComDevolutivaCommand request, CancellationToken cancellationToken)
        {
            await repositorioDiarioBordo.AtualizaDiariosComDevolutivaId(request.DevolutivaId, request.DiariosBordoIds);

            return true;
        }
    }
}
