using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class ExcluirDiarioBordoDaAulaCommandHandler : IRequestHandler<ExcluirDiarioBordoDaAulaCommand, bool>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public ExcluirDiarioBordoDaAulaCommandHandler(IRepositorioDiarioBordo repositorioDiarioBordo)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
        }

        public async Task<bool> Handle(ExcluirDiarioBordoDaAulaCommand request, CancellationToken cancellationToken)
        {
            await repositorioDiarioBordo.ExcluirDiarioBordoDaAula(request.AulaId);
            return true;
        }
    }
}
