using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQueryHandler : IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQuery, bool>
    {
        private readonly IServicoEol servicoEOL;

        public ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQueryHandler(IServicoEol servicoEOL)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<bool> Handle(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQuery request, CancellationToken cancellationToken)
            => await servicoEOL.PodePersistirTurmaNoPeriodo(request.UsuarioRf, request.CodigoTurma, request.ComponenteCurricularId, request.DataInicio, request.DataFim);
    }
}
