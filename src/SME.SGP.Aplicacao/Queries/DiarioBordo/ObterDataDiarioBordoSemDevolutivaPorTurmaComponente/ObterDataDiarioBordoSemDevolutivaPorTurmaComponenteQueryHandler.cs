using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDataDiarioBordoSemDevolutivaPorTurmaComponenteQueryHandler : IRequestHandler<ObterDataDiarioBordoSemDevolutivaPorTurmaComponenteQuery, DateTime?>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public ObterDataDiarioBordoSemDevolutivaPorTurmaComponenteQueryHandler(IRepositorioDiarioBordo repositorioDiarioBordo)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
        }

        public async Task<DateTime?> Handle(ObterDataDiarioBordoSemDevolutivaPorTurmaComponenteQuery request, CancellationToken cancellationToken)
            => await repositorioDiarioBordo.ObterDataDiarioSemDevolutivaPorTurmaComponente(request.TurmaCodigo, request.ComponenteCurricularCodigo);
    }
}
