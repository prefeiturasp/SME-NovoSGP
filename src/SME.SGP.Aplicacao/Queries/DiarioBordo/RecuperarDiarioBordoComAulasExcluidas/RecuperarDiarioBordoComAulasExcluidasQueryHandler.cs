using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RecuperarDiarioBordoComAulasExcluidasQueryHandler : IRequestHandler<RecuperarDiarioBordoComAulasExcluidasQuery, IEnumerable<DiarioBordo>>
    {
        private readonly IRepositorioDiarioBordoConsulta repositorioDiarioBordo;

        public RecuperarDiarioBordoComAulasExcluidasQueryHandler(IRepositorioDiarioBordoConsulta repositorioDiarioBordo)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
        }

        public async Task<IEnumerable<DiarioBordo>> Handle(RecuperarDiarioBordoComAulasExcluidasQuery request, CancellationToken cancellationToken)
        {
            return await repositorioDiarioBordo
                .ObterIdDiarioBordoAulasExcluidas(request.CodigoTurma, request.CodigosDisciplinas, request.TipoCalendarioId, request.DatasConsideradas);
        }
    }
}
