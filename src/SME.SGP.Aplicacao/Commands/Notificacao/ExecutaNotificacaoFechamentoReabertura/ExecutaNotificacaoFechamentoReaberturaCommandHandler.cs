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
            var fechamentoReabertura = request.FechamentoReabertura;

            var adminsSgpUe = await servicoEOL.ObterAdministradoresSGP(fechamentoReabertura.UeCodigo);
            if (adminsSgpUe != null && adminsSgpUe.Any())
            {
                foreach (var adminSgpUe in adminsSgpUe)
                {
                    fechamentoReabertura.CodigoRf = adminSgpUe;
                    await mediator.Send(new ExecutaNotificacaoCadastroFechamentoReaberturaCommand(fechamentoReabertura));
                }
                    
            }

            var diretores = servicoEOL.ObterFuncionariosPorCargoUe(fechamentoReabertura.UeCodigo, (long)Cargo.Diretor);
            if (diretores != null && diretores.Any())
            {
                foreach (var diretor in diretores)
                {
                    fechamentoReabertura.CodigoRf = diretor.CodigoRf;
                    await mediator.Send(new ExecutaNotificacaoCadastroFechamentoReaberturaCommand(fechamentoReabertura));
                }
                    
            }
            var ads = servicoEOL.ObterFuncionariosPorCargoUe(fechamentoReabertura.UeCodigo, (long)Cargo.AD);
            if (ads != null && ads.Any())
            {
                foreach (var ad in ads)
                {
                    fechamentoReabertura.CodigoRf = ad.CodigoRf;
                    await mediator.Send(new ExecutaNotificacaoCadastroFechamentoReaberturaCommand(fechamentoReabertura));
                }
                    
            }
            var cps = servicoEOL.ObterFuncionariosPorCargoUe(fechamentoReabertura.UeCodigo, (long)Cargo.CP);
            if (cps != null && cps.Any())
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
