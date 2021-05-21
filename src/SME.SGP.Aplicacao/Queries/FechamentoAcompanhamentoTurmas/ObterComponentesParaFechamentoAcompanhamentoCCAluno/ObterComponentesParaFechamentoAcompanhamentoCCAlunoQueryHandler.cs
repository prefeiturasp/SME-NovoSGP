using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesParaFechamentoAcompanhamentoCCAlunoQueryHandler : IRequestHandler<ObterComponentesParaFechamentoAcompanhamentoCCAlunoQuery, IEnumerable<long>>
    {
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;

        public ObterComponentesParaFechamentoAcompanhamentoCCAlunoQueryHandler(IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
        }
        public async Task<IEnumerable<long>> Handle(ObterComponentesParaFechamentoAcompanhamentoCCAlunoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasseAluno.ObterComponentesPorAlunoTurmaBimestreAsync(request.AlunoCodigo, request.Bimestre, request.TurmaId);
        }
    }
}
