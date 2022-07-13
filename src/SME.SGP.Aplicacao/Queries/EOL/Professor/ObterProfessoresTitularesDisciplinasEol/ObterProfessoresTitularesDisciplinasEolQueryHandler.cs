using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresTitularesDisciplinasEolQueryHandler : IRequestHandler<ObterProfessoresTitularesDisciplinasEolQuery,IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        public IServicoEol servicoEol;

        public ObterProfessoresTitularesDisciplinasEolQueryHandler(IServicoEol servicoEol)
        {
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }

        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> Handle(ObterProfessoresTitularesDisciplinasEolQuery request, CancellationToken cancellationToken)
        {
            return await servicoEol.ObterProfessoresTitularesDisciplinas(request.CodigoTurma, request.DataReferencia,
                request.ProfessorRf, request.RealizaAgrupamento);
        }
    }
}