using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresTitularesDaTurmaQueryCompletosHandler : IRequestHandler<ObterProfessoresTitularesDaTurmaCompletosQuery, IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        private readonly IServicoEol servicoEol;

        public ObterProfessoresTitularesDaTurmaQueryCompletosHandler(IServicoEol servicoEol)
        {
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }

        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> Handle(ObterProfessoresTitularesDaTurmaCompletosQuery request, CancellationToken cancellationToken)
            => await servicoEol.ObterProfessoresTitularesDisciplinas(request.CodigoTurma);
    }
}