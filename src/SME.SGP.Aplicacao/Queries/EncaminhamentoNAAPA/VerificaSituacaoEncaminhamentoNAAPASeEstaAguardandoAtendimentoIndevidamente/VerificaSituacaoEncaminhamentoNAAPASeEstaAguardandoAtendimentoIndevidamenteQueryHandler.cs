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
        public IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA { get; }

        public VerificaSituacaoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQueryHandler(IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA)
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }
        public Task<bool> Handle(VerificaSituacaoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQuery request, CancellationToken cancellationToken)
         => repositorioEncaminhamentoNAAPA.VerificaSituacaoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamente(request.EncaminhamentoId);
    }
}
