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
    public class AlterarEncaminhamentoNAAPASecaoCommandHandler : IRequestHandler<AlterarEncaminhamentoNAAPASecaoCommand, AuditoriaDto>
    {
        private readonly IRepositorioEncaminhamentoNAAPASecao repositorioEncaminhamentoNAAPASecao;

        public AlterarEncaminhamentoNAAPASecaoCommandHandler(IRepositorioEncaminhamentoNAAPASecao repositorioEncaminhamentoNAAPASecao)
        {
            this.repositorioEncaminhamentoNAAPASecao = repositorioEncaminhamentoNAAPASecao ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPASecao));
        }

        public async Task<AuditoriaDto> Handle(AlterarEncaminhamentoNAAPASecaoCommand request, CancellationToken cancellationToken)
        {
            await repositorioEncaminhamentoNAAPASecao.SalvarAsync(request.Secao);

            return (AuditoriaDto)request.Secao;
        }

    }
}
