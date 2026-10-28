using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasEmAprovacaoQueryHandler : IRequestHandler<ObterNotasEmAprovacaoQuery, IEnumerable<NotaEmAprovacaoFechamentoDto>>
    {
        private readonly IRepositorioNotasConceitosConsulta repositorioNotasConceitos;

        public ObterNotasEmAprovacaoQueryHandler(IRepositorioNotasConceitosConsulta repositorioNotasConceitos)
        {
            this.repositorioNotasConceitos = repositorioNotasConceitos ?? throw new ArgumentNullException(nameof(repositorioNotasConceitos));
        }

        public async Task<IEnumerable<NotaEmAprovacaoFechamentoDto>> Handle(ObterNotasEmAprovacaoQuery request, CancellationToken cancellationToken)
        {
            if (request.CodigosAlunos == null || !request.CodigosAlunos.Any() ||
                request.TurmaFechamentoIds == null || !request.TurmaFechamentoIds.Any())
                return Enumerable.Empty<NotaEmAprovacaoFechamentoDto>();

            return await repositorioNotasConceitos.ObterNotasEmAprovacao(request.CodigosAlunos, request.TurmaFechamentoIds);
        }
    }
}
