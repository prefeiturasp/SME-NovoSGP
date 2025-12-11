using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarSituacaoAtendimentoNAAPACommandHandler : IRequestHandler<AtualizarSituacaoAtendimentoNAAPACommand, bool>
    {
        private readonly IRepositorioAtendimentoNAAPA repositorioAtendimentoNAAPA;

        public AtualizarSituacaoAtendimentoNAAPACommandHandler(
            IRepositorioAtendimentoNAAPA repositorioAtendimentoNAAPA)
        {
            this.repositorioAtendimentoNAAPA = repositorioAtendimentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioAtendimentoNAAPA));
        }

        public async Task<bool> Handle(AtualizarSituacaoAtendimentoNAAPACommand request, CancellationToken cancellationToken)
        {
            await repositorioAtendimentoNAAPA.AtualizarSituacaoAtendimento(request.Id, Dominio.Enumerados.SituacaoNAAPA.EmApoio);
            
            return true;
        }
    }
}