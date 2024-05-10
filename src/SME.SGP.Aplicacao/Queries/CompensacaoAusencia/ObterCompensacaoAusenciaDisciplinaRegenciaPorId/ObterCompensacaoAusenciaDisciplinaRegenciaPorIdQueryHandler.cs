using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaDisciplinaRegenciaPorIdQueryHandler : IRequestHandler<ObterCompensacaoAusenciaDisciplinaRegenciaPorIdQuery,IEnumerable<CompensacaoAusenciaDisciplinaRegencia>>
    {
        private IRepositorioCompensacaoAusenciaDisciplinaRegencia repositorioCompensacaoAusenciaDisciplinaRegencia;

        public ObterCompensacaoAusenciaDisciplinaRegenciaPorIdQueryHandler(IRepositorioCompensacaoAusenciaDisciplinaRegencia compensacaoAusenciaDisciplinaRegencia)
        {
            repositorioCompensacaoAusenciaDisciplinaRegencia = compensacaoAusenciaDisciplinaRegencia ?? throw new ArgumentNullException(nameof(compensacaoAusenciaDisciplinaRegencia));
        }

        public async Task<IEnumerable<CompensacaoAusenciaDisciplinaRegencia>> Handle(ObterCompensacaoAusenciaDisciplinaRegenciaPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCompensacaoAusenciaDisciplinaRegencia.ObterPorCompensacao(request.CompensacaoId);
        }
    }
}