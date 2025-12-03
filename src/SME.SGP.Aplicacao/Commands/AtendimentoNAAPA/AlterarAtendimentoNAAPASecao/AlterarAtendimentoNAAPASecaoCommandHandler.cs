using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarAtendimentoNAAPASecaoCommandHandler : IRequestHandler<AlterarAtendimentoNAAPASecaoCommand, AuditoriaDto>
    {
        private readonly IRepositorioEncaminhamentoNAAPASecao repositorioEncaminhamentoNAAPASecao;

        public AlterarAtendimentoNAAPASecaoCommandHandler(IRepositorioEncaminhamentoNAAPASecao repositorioEncaminhamentoNAAPASecao)
        {
            this.repositorioEncaminhamentoNAAPASecao = repositorioEncaminhamentoNAAPASecao ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPASecao));
        }

        public async Task<AuditoriaDto> Handle(AlterarAtendimentoNAAPASecaoCommand request, CancellationToken cancellationToken)
        {
            await repositorioEncaminhamentoNAAPASecao.SalvarAsync(request.Secao);

            return (AuditoriaDto)request.Secao;
        }

    }
}
