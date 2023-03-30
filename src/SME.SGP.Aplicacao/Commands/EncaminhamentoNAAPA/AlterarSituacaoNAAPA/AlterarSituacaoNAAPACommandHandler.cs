using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarSituacaoNAAPACommandHandler : IRequestHandler<AlterarSituacaoNAAPACommand, bool>
    {
        private readonly IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA;

        public AlterarSituacaoNAAPACommandHandler(IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA)
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }
        public async Task<bool> Handle(AlterarSituacaoNAAPACommand request, CancellationToken cancellationToken)
        {
            request.Encaminhamento.Situacao = request.Situacao;
            await repositorioEncaminhamentoNAAPA.SalvarAsync(request.Encaminhamento);

            return true;
        }
    }
}
