using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosLetivosComunicadoQueryHandler : IRequestHandler<ObterAnosLetivosComunicadoQuery, IEnumerable<int>>
    {
        private readonly IRepositorioComunicado repositorioComunicado;

        public ObterAnosLetivosComunicadoQueryHandler(IRepositorioComunicado repositorioComunicado)
        {
            this.repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
        }

        public async Task<IEnumerable<int>> Handle(ObterAnosLetivosComunicadoQuery request, CancellationToken cancellationToken)
            => await repositorioComunicado.ObterAnosLetivosComunicados();
    }
}
