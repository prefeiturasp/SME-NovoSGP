using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class SalvarObservacaoNAAPACommandHandler : IRequestHandler<SalvarObservacaoNAAPACommand, bool>
    {
        private readonly IRepositorioObservacaoEncaminhamentoNAAPA repositorioObs;

        public SalvarObservacaoNAAPACommandHandler(IRepositorioObservacaoEncaminhamentoNAAPA repositorioObs)
        {
            this.repositorioObs = repositorioObs ?? throw new ArgumentNullException(nameof(repositorioObs));
        }

        public async Task<bool> Handle(SalvarObservacaoNAAPACommand request, CancellationToken cancellationToken)
        {
            await repositorioObs.SalvarAsync(new EncaminhamentoNAAPAObservacao
            {
                EncaminhamentoNAAPAId = request.encaminhamentoNAAPAObservacaoSalvarDto.EncaminhamentoNAAPAId,
                Observacao = request.encaminhamentoNAAPAObservacaoSalvarDto.Observacao,
                Excluido = false,
                Id = request.encaminhamentoNAAPAObservacaoSalvarDto.Id
            });
            return true;
        }
    }
}