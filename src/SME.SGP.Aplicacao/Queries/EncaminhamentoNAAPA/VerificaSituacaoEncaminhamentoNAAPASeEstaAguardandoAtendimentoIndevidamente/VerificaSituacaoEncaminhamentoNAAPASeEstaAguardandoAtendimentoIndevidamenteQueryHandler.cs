using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaSituacaoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQueryHandler : IRequestHandler<VerificaSituacaoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQuery, bool>
    {
        public IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA { get; }

        public VerificaSituacaoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQueryHandler(IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA)
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }
        public Task<bool> Handle(VerificaSituacaoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQuery request, CancellationToken cancellationToken)
         => repositorioEncaminhamentoNAAPA.VerificaSituacaoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamente(request.EncaminhamentoId);
    }
}
