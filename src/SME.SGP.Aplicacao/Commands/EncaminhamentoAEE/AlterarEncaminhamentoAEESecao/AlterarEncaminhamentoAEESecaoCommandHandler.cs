using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarEncaminhamentoAEESecaoCommandHandler : IRequestHandler<AlterarEncaminhamentoAEESecaoCommand, AuditoriaDto>
    {
        private readonly IRepositorioEncaminhamentoAEESecao repositorioEncaminhamentoAEESecao;

        public AlterarEncaminhamentoAEESecaoCommandHandler(IRepositorioEncaminhamentoAEESecao repositorioEncaminhamentoAEESecao)
        {
            this.repositorioEncaminhamentoAEESecao = repositorioEncaminhamentoAEESecao ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEESecao));
        }

        public async Task<AuditoriaDto> Handle(AlterarEncaminhamentoAEESecaoCommand request, CancellationToken cancellationToken)
        {
            await repositorioEncaminhamentoAEESecao.SalvarAsync(request.Secao);

            return (AuditoriaDto)request.Secao;
        }

    }
}
