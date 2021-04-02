using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentosAEEDeferidosQueryHandler : IRequestHandler<ObterEncaminhamentosAEEDeferidosQuery, IEnumerable<AEETurmaDto>>
    {
        private readonly IRepositorioEncaminhamentoAEE repositorio;

        public ObterEncaminhamentosAEEDeferidosQueryHandler(IRepositorioEncaminhamentoAEE repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<AEETurmaDto>> Handle(ObterEncaminhamentosAEEDeferidosQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterQuantidadeDeferidos(request.Ano, request.DreId, request.UeId);
    }
}
