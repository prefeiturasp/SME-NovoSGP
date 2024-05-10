using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirReferenciaDiarioBordoDevolutivaCommandHandler : IRequestHandler<ExcluirReferenciaDiarioBordoDevolutivaCommand, bool>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public ExcluirReferenciaDiarioBordoDevolutivaCommandHandler(IRepositorioDiarioBordo repositorioDiarioBordo)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
        }

        public async Task<bool> Handle(ExcluirReferenciaDiarioBordoDevolutivaCommand request, CancellationToken cancellationToken)
        {
            await repositorioDiarioBordo.ExcluirReferenciaDevolutiva(request.DevolutivaId);
            return true;
        }
    }
}
