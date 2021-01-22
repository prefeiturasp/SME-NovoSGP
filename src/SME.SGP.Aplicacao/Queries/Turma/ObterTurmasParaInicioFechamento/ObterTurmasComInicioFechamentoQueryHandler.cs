using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComInicioFechamentoQueryHandler : IRequestHandler<ObterTurmasComInicioFechamentoQuery, IEnumerable<Turma>>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmasComInicioFechamentoQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<Turma>> Handle(ObterTurmasComInicioFechamentoQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterTurmasComInicioFechamento(request.UeId, request.PeriodoEscolarId, request.Modalidades);
    }
}
