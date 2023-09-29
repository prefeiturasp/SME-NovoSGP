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
            if (adminsSgpUe.NaoEhNulo() && adminsSgpUe.Any())
            {
                foreach (var adminSgpUe in adminsSgpUe)
                {
                    fechamentoReabertura.CodigoRf = adminSgpUe;
                    await mediator.Send(new ExecutaNotificacaoCadastroFechamentoReaberturaCommand(fechamentoReabertura));
                }
                    
            }

            var diretores =
                await mediator.Send(
                    new ObterFuncionariosPorCargoUeQuery(fechamentoReabertura.UeCodigo, (long) Cargo.Diretor));
            if (diretores.NaoEhNulo() && diretores.Any())
            {
                foreach (var diretor in diretores)
                {
                    fechamentoReabertura.CodigoRf = diretor.CodigoRf;
                    await mediator.Send(new ExecutaNotificacaoCadastroFechamentoReaberturaCommand(fechamentoReabertura));
                }
                    
            }
            var ads = await mediator.Send(
                new ObterFuncionariosPorCargoUeQuery(fechamentoReabertura.UeCodigo, (long)Cargo.AD));
            if (ads.NaoEhNulo() && ads.Any())
            {
                foreach (var ad in ads)
                {
                    fechamentoReabertura.CodigoRf = ad.CodigoRf;
                    await mediator.Send(new ExecutaNotificacaoCadastroFechamentoReaberturaCommand(fechamentoReabertura));
                }
                    
            }
            var cps = await mediator.Send(
                new ObterFuncionariosPorCargoUeQuery(fechamentoReabertura.UeCodigo, (long)Cargo.CP));
            if (cps.NaoEhNulo() && cps.Any())
            {
                foreach (var cp in cps)
                {
                    fechamentoReabertura.CodigoRf = cp.CodigoRf;
                    await mediator.Send(new ExecutaNotificacaoCadastroFechamentoReaberturaCommand(fechamentoReabertura));
                }
                    
            }
            return true;
        }
    }
}
