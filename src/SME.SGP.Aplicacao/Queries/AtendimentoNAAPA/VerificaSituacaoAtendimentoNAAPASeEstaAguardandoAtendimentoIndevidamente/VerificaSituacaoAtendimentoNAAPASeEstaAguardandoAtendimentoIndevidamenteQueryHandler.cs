using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaSituacaoAtendimentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQueryHandler : IRequestHandler<VerificaSituacaoAtendimentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQuery, bool>
    {
        public IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA { get; }

        public VerificaSituacaoAtendimentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQueryHandler(IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA)
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }
        public Task<bool> Handle(VerificaSituacaoAtendimentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQuery request, CancellationToken cancellationToken)
         => repositorioEncaminhamentoNAAPA.VerificaSituacaoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamente(request.EncaminhamentoId);
    }
}
