using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadesPorCodigosUeQueryHandler : IRequestHandler<ObterModalidadesPorCodigosUeQuery, IEnumerable<Modalidade>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterModalidadesPorCodigosUeQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<IEnumerable<Modalidade>> Handle(ObterModalidadesPorCodigosUeQuery request, CancellationToken cancellationToken)
        {
            var retorno = await repositorioAbrangencia.ObterModalidadesPorCodigosUe(request.CodigosUe);
            return retorno ?? throw new NegocioException("Não foi encontrar as modalidades das UE's");
        }

    }
}
