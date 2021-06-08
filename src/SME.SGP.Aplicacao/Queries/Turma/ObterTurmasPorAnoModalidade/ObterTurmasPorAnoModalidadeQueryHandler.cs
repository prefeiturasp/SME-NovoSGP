using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorAnoModalidadeQueryHandler : IRequestHandler<ObterTurmasPorAnoModalidadeQuery, IEnumerable<Turma>>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmasPorAnoModalidadeQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<Turma>> Handle(ObterTurmasPorAnoModalidadeQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterTurmasCompletasPorAnoLetivoModalidade(request.AnoLetivo, request.Modalidades, request.TurmaCodigo);
        }
    }
}
