using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosLetivosHistoricoDeComunicadosQueryHandler : IRequestHandler<ObterAnosLetivosHistoricoDeComunicadosQuery, IEnumerable<int>>
    {
        private readonly IRepositorioComunicado repositorioComunicado;

        public ObterAnosLetivosHistoricoDeComunicadosQueryHandler(IRepositorioComunicado repositorioComunicado)
        {
            this.repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
        }

        public async Task<IEnumerable<int>> Handle(ObterAnosLetivosHistoricoDeComunicadosQuery request, CancellationToken cancellationToken)
            => await repositorioComunicado.ObterAnosLetivosComHistoricoDeComunicados(request.DataInicio, request.DataFim);
    }
}
