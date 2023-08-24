﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioAcompanhamentoFechamentoUseCase : AbstractUseCase, IRelatorioAcompanhamentoFechamentoUseCase
    {
        public RelatorioAcompanhamentoFechamentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(FiltroRelatorioAcompanhamentoFechamentoDto filtro)
        {
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            filtro.Usuario = usuarioLogado;

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.AcompanhamentoFechamento, filtro, usuarioLogado, rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosAcompanhamentoFechamento));
        }
    }
}
