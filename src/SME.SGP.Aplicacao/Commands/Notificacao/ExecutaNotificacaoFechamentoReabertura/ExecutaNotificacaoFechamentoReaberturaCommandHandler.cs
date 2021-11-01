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
        private readonly IServicoEol servicoEOL;

        public ExecutaNotificacaoFechamentoReaberturaCommandHandler(IMediator mediator, IServicoEol servicoEOL)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<bool> Handle(ExecutaNotificacaoFechamentoReaberturaCommand request, CancellationToken cancellationToken)
        {
            var dreCodigo = request.DreCodigo;
            var ue = request.Ue;
            var fechamentoReabertura = request.FechamentoReabertura;

            var adminsSgpUe = await servicoEOL.ObterAdministradoresSGP(ue);
            if (adminsSgpUe != null && adminsSgpUe.Any())
            {
                foreach (var adminSgpUe in adminsSgpUe)
                    await mediator.Send(new ExecutaNotificacaoCadastroFechamentoReaberturaCommand(fechamentoReabertura, dreCodigo, ue, adminSgpUe));
            }

            var diretores = servicoEOL.ObterFuncionariosPorCargoUe(ue, (long)Cargo.Diretor);
            if (diretores != null && diretores.Any())
            {
                foreach (var diretor in diretores)
                    await mediator.Send(new ExecutaNotificacaoCadastroFechamentoReaberturaCommand(fechamentoReabertura, dreCodigo, ue, diretor.CodigoRf));                
            }
            var ads = servicoEOL.ObterFuncionariosPorCargoUe(ue, (long)Cargo.AD);
            if (ads != null && ads.Any())
            {
                foreach (var ad in ads)
                    await mediator.Send(new ExecutaNotificacaoCadastroFechamentoReaberturaCommand(fechamentoReabertura, dreCodigo, ue, ad.CodigoRf));
            }
            var cps = servicoEOL.ObterFuncionariosPorCargoUe(ue, (long)Cargo.CP);
            if (cps != null && cps.Any())
            {
                foreach (var cp in cps)
                    await mediator.Send(new ExecutaNotificacaoCadastroFechamentoReaberturaCommand(fechamentoReabertura, dreCodigo, ue, cp.CodigoRf));
            }
            return true;
        }
    }
}
