using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComFechamentoOuConselhoNaoFinalizadosQueryHandler : IRequestHandler<ObterTurmasComFechamentoOuConselhoNaoFinalizadosQuery, IEnumerable<Turma>>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurma;

        public ObterTurmasComFechamentoOuConselhoNaoFinalizadosQueryHandler(IRepositorioTurmaConsulta repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<Turma>> Handle(ObterTurmasComFechamentoOuConselhoNaoFinalizadosQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterTurmasComFechamentoOuConselhoNaoFinalizados(request.UeId, request.AnoLetivo, request.PeriodoEscolarId, request.Modalidades, request.Semestre);
    }
}
