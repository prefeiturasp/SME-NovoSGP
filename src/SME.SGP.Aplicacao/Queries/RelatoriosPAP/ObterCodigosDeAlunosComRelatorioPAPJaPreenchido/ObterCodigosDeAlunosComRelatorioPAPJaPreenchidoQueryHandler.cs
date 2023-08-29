using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosDeAlunosComRelatorioPAPJaPreenchidoQueryHandler : IRequestHandler<ObterCodigosDeAlunosComRelatorioPAPJaPreenchidoQuery, IEnumerable<string>>
    {
        private readonly IRepositorioRelatorioPeriodicoPAPAluno repositorio;

        public ObterCodigosDeAlunosComRelatorioPAPJaPreenchidoQueryHandler(IRepositorioRelatorioPeriodicoPAPAluno repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<IEnumerable<string>> Handle(ObterCodigosDeAlunosComRelatorioPAPJaPreenchidoQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterCodigosDeAlunosComRelatorioJaPreenchido(request.TurmaId, request.PeriodoRelatorioPAPId);
        }
    }
}
