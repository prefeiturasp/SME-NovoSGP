using MediatR;
using Sentry;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class LimparConsolidacaoAcompanhamentoAprendizagemCommandHandler : IRequestHandler<LimparConsolidacaoAcompanhamentoAprendizagemCommand, bool>
    {
        private readonly IRepositorioConsolidacaoAcompanhamentoAprendizagemAluno repositorio;

        public LimparConsolidacaoAcompanhamentoAprendizagemCommandHandler(IRepositorioConsolidacaoAcompanhamentoAprendizagemAluno repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(LimparConsolidacaoAcompanhamentoAprendizagemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await repositorio.Limpar();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }

            return true;
        }
    }
}
