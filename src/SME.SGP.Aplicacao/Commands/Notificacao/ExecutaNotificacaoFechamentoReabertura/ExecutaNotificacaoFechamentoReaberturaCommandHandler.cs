using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Queries.Funcionario;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoFechamentoReaberturaCommandHandler : IRequestHandler<ExecutaNotificacaoFechamentoReaberturaCommand, bool>
    {
        private readonly IMediator mediator;

        public ExecutaNotificacaoFechamentoReaberturaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExecutaNotificacaoFechamentoReaberturaCommand request, CancellationToken cancellationToken)
        {
            var fechamentoReabertura = request.FechamentoReabertura;
            var adminsSgpUe = await mediator.Send(new ObterAdministradoresPorUEQuery(fechamentoReabertura.UeCodigo));
            if (adminsSgpUe.PossuiRegistros())
                await NotificarUsuariosCadastroFechamentoReabertura(adminsSgpUe, fechamentoReabertura);

            var diretores =
                await mediator.Send(
                    new ObterFuncionariosPorCargoUeQuery(fechamentoReabertura.UeCodigo, (long) Cargo.Diretor));
            if (diretores.PossuiRegistros())
                await NotificarUsuariosCadastroFechamentoReabertura(diretores.Select(d => d.CodigoRf).ToArray(), fechamentoReabertura);
            

            var ads = await mediator.Send(
                new ObterFuncionariosPorCargoUeQuery(fechamentoReabertura.UeCodigo, (long)Cargo.AD));
            if (ads.PossuiRegistros())
                await NotificarUsuariosCadastroFechamentoReabertura(ads.Select(a => a.CodigoRf).ToArray(), fechamentoReabertura);


            var cps = await mediator.Send(
                new ObterFuncionariosPorCargoUeQuery(fechamentoReabertura.UeCodigo, (long)Cargo.CP));
            if (cps.PossuiRegistros())
                await NotificarUsuariosCadastroFechamentoReabertura(cps.Select(c => c.CodigoRf).ToArray(), fechamentoReabertura);

            return true;
        }

        private async Task NotificarUsuariosCadastroFechamentoReabertura(string[] adminsSgpUe, FiltroFechamentoReaberturaNotificacaoDto fechamentoReabertura)
        {
            foreach (var adminSgpUe in adminsSgpUe)
            {
                fechamentoReabertura.CodigoRf = adminSgpUe;
                await mediator.Send(new ExecutaNotificacaoCadastroFechamentoReaberturaCommand(fechamentoReabertura));
            }
        }
    }
}
