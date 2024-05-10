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
    public class ObterFechamentoAlunoPorDisciplinaIdQueryHandler : IRequestHandler<ObterFechamentoAlunoPorDisciplinaIdQuery, IEnumerable<FechamentoAluno>>
    {
        private readonly IRepositorioFechamentoAlunoConsulta repositorioFechamentoAluno;

        public ObterFechamentoAlunoPorDisciplinaIdQueryHandler(IRepositorioFechamentoAlunoConsulta repositorioFechamentoAluno)
        {
            this.repositorioFechamentoAluno = repositorioFechamentoAluno ?? throw new ArgumentNullException(nameof(repositorioFechamentoAluno));
        }

        public async Task<IEnumerable<FechamentoAluno>> Handle(ObterFechamentoAlunoPorDisciplinaIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoAluno.ObterPorFechamentoTurmaDisciplina(request.FechamentoTurmaDisciplinaId);
        }
    }
}
