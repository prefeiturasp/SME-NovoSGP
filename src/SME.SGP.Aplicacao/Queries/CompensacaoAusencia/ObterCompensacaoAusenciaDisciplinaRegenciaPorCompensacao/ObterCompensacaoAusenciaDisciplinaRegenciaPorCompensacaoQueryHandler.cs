using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaDisciplinaRegenciaPorCompensacaoQueryHandler : IRequestHandler<ObterCompensacaoAusenciaDisciplinaRegenciaPorCompensacaoQuery, IEnumerable<CompensacaoAusenciaDisciplinaRegencia>>
    {
        private readonly IRepositorioCompensacaoAusenciaDisciplinaRegencia repositorioCompensacaoAusenciaDisciplinaRegencia;

        public ObterCompensacaoAusenciaDisciplinaRegenciaPorCompensacaoQueryHandler(IRepositorioCompensacaoAusenciaDisciplinaRegencia repositorioCompensacaoAusenciaDisciplinaRegencia)
        {
            this.repositorioCompensacaoAusenciaDisciplinaRegencia = repositorioCompensacaoAusenciaDisciplinaRegencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaDisciplinaRegencia));
        }

        public Task<IEnumerable<CompensacaoAusenciaDisciplinaRegencia>> Handle(ObterCompensacaoAusenciaDisciplinaRegenciaPorCompensacaoQuery request, CancellationToken cancellationToken)
        {
            return repositorioCompensacaoAusenciaDisciplinaRegencia.ObterPorCompensacao(request.CompensacaoAusenciaId);
        }
    }
}
