using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.VerificaSituacaoNovoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamente
{
    public class VerificaSituacaoNovoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQueryHandler : IRequestHandler<VerificaSituacaoNovoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQuery, bool>
    {
        public IRepositorioNovoEncaminhamentoNAAPA repositorioNovoEncaminhamentoNAAPA { get; }

        public VerificaSituacaoNovoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQueryHandler(IRepositorioNovoEncaminhamentoNAAPA repositorioNovoEncaminhamentoNAAPA)
        {
            this.repositorioNovoEncaminhamentoNAAPA = repositorioNovoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioNovoEncaminhamentoNAAPA));
        }

        public Task<bool> Handle(VerificaSituacaoNovoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQuery request, CancellationToken cancellationToken)
         => repositorioNovoEncaminhamentoNAAPA.VerificaSituacaoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamente(request.EncaminhamentoId);
    }
}