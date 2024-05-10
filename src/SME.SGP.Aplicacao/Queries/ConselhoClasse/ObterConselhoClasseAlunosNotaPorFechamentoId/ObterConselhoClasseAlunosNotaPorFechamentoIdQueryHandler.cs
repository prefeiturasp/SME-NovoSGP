using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseAlunosNotaPorFechamentoIdQueryHandler : IRequestHandler<ObterConselhoClasseAlunosNotaPorFechamentoIdQuery, IEnumerable<ConselhoClasseAlunosNotaPorFechamentoIdDto>>
    {
        private readonly IRepositorioConselhoClasse repositorio;

        public ObterConselhoClasseAlunosNotaPorFechamentoIdQueryHandler(IRepositorioConselhoClasse repositorioConselhoClasse)
        {
            this.repositorio = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
        }

        public async Task<IEnumerable<ConselhoClasseAlunosNotaPorFechamentoIdDto>> Handle(ObterConselhoClasseAlunosNotaPorFechamentoIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterConselhoClasseAlunosNotaPorFechamentoId(request.FechamentoTurmaId);
        }
    }
}
