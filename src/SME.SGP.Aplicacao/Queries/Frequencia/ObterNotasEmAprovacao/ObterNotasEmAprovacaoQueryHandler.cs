using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasEmAprovacaoQueryHandler : IRequestHandler<ObterNotasEmAprovacaoQuery, IEnumerable<NotaEmAprovacaoFechamentoDto>>
    {
        private readonly IRepositorioNotasConceitosConsulta repositorio;
        public ObterNotasEmAprovacaoQueryHandler(IRepositorioNotasConceitosConsulta repositorio)
            => this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));

        public Task<IEnumerable<NotaEmAprovacaoFechamentoDto>> Handle(ObterNotasEmAprovacaoQuery request, CancellationToken cancellationToken)
            => request.Filtros != null
                ? repositorio.ObterNotasEmAprovacaoAsync(request.Filtros)
                : repositorio.ObterNotasEmAprovacao(request.CodigosAlunos, request.TurmaFechamentoIds);
    }
}
