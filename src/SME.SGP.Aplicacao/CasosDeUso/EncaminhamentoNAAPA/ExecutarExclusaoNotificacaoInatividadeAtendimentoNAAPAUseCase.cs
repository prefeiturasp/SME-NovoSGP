using MediatR;
using Minio.DataModel;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarExclusaoNotificacaoInatividadeAtendimentoNAAPAUseCase : AbstractUseCase, IExecutarExclusaoNotificacaoInatividadeAtendimentoNAAPAUseCase
    {
        public ExecutarExclusaoNotificacaoInatividadeAtendimentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var param = mensagem.ObterObjetoMensagem<string>();
            if (string.IsNullOrEmpty(param)) return false;
            var encaminhamentoNAAPAId = long.Parse(param);

            var idsNotificacao = await mediator.Send(new ObterIdsNotificacaoPorNAAPAIdQuery(encaminhamentoNAAPAId));
            await mediator.Send(new ExcluirInatividadesAtendimentoNotificacaoPorIdNAAPACommand(encaminhamentoNAAPAId));
            foreach (var id in idsNotificacao)
                await mediator.Send(new ExcluirNotificacaoPorIdCommand(id));
            return true;
        }
    }
}