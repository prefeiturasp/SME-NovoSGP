using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class DevolverEncaminhamentoUseCase : AbstractUseCase, IDevolverEncaminhamentoUseCase
    {
        public DevolverEncaminhamentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(DevolucaoEncaminhamentoAEEDto devolucaoDto)
        {
            var encaminhamento = await mediator.Send(new ObterEncaminhamentoAEEPorIdQuery(devolucaoDto.EncaminhamentoAEEId));

            if (encaminhamento.Situacao != SituacaoAEE.Encaminhado)
                throw new NegocioException($"Encaminhamento só podem ser devolvidos na situação '{SituacaoAEE.Encaminhado.Name()}'");

            encaminhamento.Situacao = SituacaoAEE.Devolvido;

            await mediator.Send(new SalvarEncaminhamentoAEECommand(encaminhamento));

            return true;
        }
    }
}
