﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioNotificacaoUseCase : AbstractUseCase, IRelatorioNotificacaoUseCase
    {
        public RelatorioNotificacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(FiltroRelatorioNotificacao filtro)
        {
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            filtro.UsuarioNome = usuarioLogado.Nome;
            filtro.UsuarioRf = usuarioLogado.CodigoRf;

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.Notificacoes, filtro, usuarioLogado, rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosNotificacoes));
        }
    }
}
