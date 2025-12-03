using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

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
            if (request.encaminhamentoNAAPAObservacaoSalvarDto.Id > 0)
                await AtualizacaoObservacao(request.encaminhamentoNAAPAObservacaoSalvarDto);
            else
                await SalvarObservacao(request.encaminhamentoNAAPAObservacaoSalvarDto);

            return true;
        }

        private async Task SalvarObservacao(AtendimentoNAAPAObservacaoSalvarDto encaminhamentoNAAPAObservacaoSalvarDto)
        {
            await repositorioObs.SalvarAsync(new EncaminhamentoNAAPAObservacao
            {
                EncaminhamentoNAAPAId = encaminhamentoNAAPAObservacaoSalvarDto.EncaminhamentoNAAPAId,
                Observacao = encaminhamentoNAAPAObservacaoSalvarDto.Observacao,
                Excluido = false,
                Id = encaminhamentoNAAPAObservacaoSalvarDto.Id
            });
        }
        private async Task AtualizacaoObservacao(AtendimentoNAAPAObservacaoSalvarDto encaminhamentoNAAPAObservacaoSalvarDto)
        {
            var observacaoExistente = await repositorioObs.ObterPorIdAsync(encaminhamentoNAAPAObservacaoSalvarDto.Id);
            observacaoExistente.Observacao = encaminhamentoNAAPAObservacaoSalvarDto.Observacao;
            await repositorioObs.SalvarAsync(observacaoExistente);
        }
    }
}