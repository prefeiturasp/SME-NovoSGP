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
    public class ExecutarExclusaoNotificacaoInformativoUsuariosUseCase : AbstractUseCase, IExecutarExclusaoNotificacaoInformativoUsuariosUseCase
    {
        public ExecutarExclusaoNotificacaoInformativoUsuariosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var informativoId = long.Parse(param.ObterObjetoMensagem<string>());
            var informativo = await mediator.Send(new ObterInformesPorIdQuery(informativoId));
            if (informativo.EhNulo())
                throw new NegocioException("Não foi possível encontrar o Informativo informado");

            var idsNotificacao = await mediator.Send(new ObterIdsNotificacaoPorInformativoIdQuery(informativoId));
            foreach (var id in idsNotificacao)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExcluirNotificacaoInformativoUsuario, id, Guid.NewGuid(), null));  
            return true;
        }
    }
}