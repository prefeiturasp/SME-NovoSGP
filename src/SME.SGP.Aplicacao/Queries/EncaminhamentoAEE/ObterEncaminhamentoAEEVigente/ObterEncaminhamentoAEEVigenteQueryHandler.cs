using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoAEEVigenteQueryHandler : IRequestHandler<ObterEncaminhamentoAEEVigenteQuery,
        IEnumerable<EncaminhamentoAEEVigenteDto>>
    {
        private readonly IRepositorioEncaminhamentoAEE _repositorioEncaminhamentoAEE;

        public ObterEncaminhamentoAEEVigenteQueryHandler(IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE)
        {
            _repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        public async Task<IEnumerable<EncaminhamentoAEEVigenteDto>> Handle(ObterEncaminhamentoAEEVigenteQuery request, CancellationToken cancellationToken)
        {
            return await _repositorioEncaminhamentoAEE.ObterEncaminhamentosVigentes(request.AnoLetivo);
        }
    }
}
