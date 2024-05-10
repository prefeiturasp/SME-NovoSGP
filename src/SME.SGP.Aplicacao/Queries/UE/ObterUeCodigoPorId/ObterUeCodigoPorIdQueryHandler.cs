using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUeCodigoPorIdQueryHandler : IRequestHandler<ObterUeCodigoPorIdQuery, string>
    {
        private readonly IRepositorioUeConsulta repositorioUeConsultas;

        public ObterUeCodigoPorIdQueryHandler(IRepositorioUeConsulta repositorioUeConsultas)
        {
            this.repositorioUeConsultas = repositorioUeConsultas ?? throw new ArgumentNullException(nameof(repositorioUeConsultas));
        }

        public Task<string> Handle(ObterUeCodigoPorIdQuery request, CancellationToken cancellationToken)
            => repositorioUeConsultas.ObterCodigoPorId(request.UeId);
    }
}
