using MediatR;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Disciplina.ObterDisciplinasPerfilCJ
{
    public class ObterDisciplinasPerfilCJQueryHandler : IRequestHandler<ObterDisciplinasPerfilCJQuery, IEnumerable<DisciplinaResposta>>
    {
        private readonly IConsultasDisciplina consultasDisciplina;

        public ObterDisciplinasPerfilCJQueryHandler(IConsultasDisciplina consultasDisciplina)
        {
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
        }

        public Task<IEnumerable<DisciplinaResposta>> Handle(ObterDisciplinasPerfilCJQuery request, CancellationToken cancellationToken)
        {
            return consultasDisciplina.ObterDisciplinasPerfilCJ(request.CodigoTurma, request.Login);
        }
    }
}
