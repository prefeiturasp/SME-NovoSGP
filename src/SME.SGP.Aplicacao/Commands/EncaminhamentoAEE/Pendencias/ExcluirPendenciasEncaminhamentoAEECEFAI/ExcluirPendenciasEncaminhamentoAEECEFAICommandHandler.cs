using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciasEncaminhamentoAEECEFAICommandHandler : IRequestHandler<ExcluirPendenciasEncaminhamentoAEECEFAICommand, bool>
    {
        private readonly IMediator mediator;

        public ExcluirPendenciasEncaminhamentoAEECEFAICommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirPendenciasEncaminhamentoAEECEFAICommand request, CancellationToken cancellationToken)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            var ehCEFAI = await EhCoordenadorCEFAI(usuarioLogado, request.TurmaId);
            if (ehCEFAI)
            {
                var pendencia = await mediator.Send(new ObterPendenciaEncaminhamentoAEEPorIdEUsuarioIdQuery(request.EncaminhamentoAEEId));
                if (pendencia != null)
                    await mediator.Send(new ExcluirPendenciaEncaminhamentoAEECommand(pendencia.PendenciaId));

                return true;
            }
            return false;
        }

        private async Task<bool> EhCoordenadorCEFAI(Usuario usuarioLogado, long turmaId)
        {
            if (!usuarioLogado.EhCoordenadorCEFAI())
                return false;

            var codigoDRE = await mediator.Send(new ObterCodigoDREPorTurmaIdQuery(turmaId));
            if (string.IsNullOrEmpty(codigoDRE))
                return false;

            return await UsuarioTemFuncaoCEFAINaDRE(usuarioLogado, codigoDRE);
        }

        private async Task<bool> UsuarioTemFuncaoCEFAINaDRE(Usuario usuarioLogado, string codigoDre)
        {
            var funcionarios = await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(codigoDre, new List<Guid>() { Perfis.PERFIL_CEFAI }));
            return funcionarios.Any(c => c == usuarioLogado.CodigoRf);
        }
    }
}
