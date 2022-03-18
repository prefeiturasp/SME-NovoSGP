using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoAEEEncerrarAutomaticoQueryHandler : IRequestHandler<ObterEncaminhamentoAEEEncerrarAutomaticoQuery,
        IEnumerable<EncaminhamentoAEEEncerrarAutomaticoDto>>
    {
        private readonly IRepositorioEncaminhamentoAEE _repositorioEncaminhamentoAEE;

        public ObterEncaminhamentoAEEEncerrarAutomaticoQueryHandler(IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE)
        {
            _repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        public async Task<IEnumerable<EncaminhamentoAEEEncerrarAutomaticoDto>> Handle(ObterEncaminhamentoAEEEncerrarAutomaticoQuery request, CancellationToken cancellationToken)
        {
            return await _repositorioEncaminhamentoAEE.ObterEncaminhamentoEncerrarAutomatico();
        }
    }
}
