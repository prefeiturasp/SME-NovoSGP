using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresTitularesDaTurmaQueryHandler : IRequestHandler<ObterProfessoresTitularesDaTurmaQuery, IEnumerable<string>>
    {
        private readonly IServicoEol servicoEol;

        public ObterProfessoresTitularesDaTurmaQueryHandler(IServicoEol servicoEol)
        {
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }
        public async Task<IEnumerable<string>> Handle(ObterProfessoresTitularesDaTurmaQuery request, CancellationToken cancellationToken)
        {
            return (await servicoEol.ObterProfessoresTitularesDisciplinas(request.CodigoTurma))?.Select(c => c.ProfessorRf);
        }
    }
}
