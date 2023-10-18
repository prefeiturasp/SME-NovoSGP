using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesMigracaoQueryHandler : IRequestHandler<ObterQuestoesMigracaoQuery, IEnumerable<Questao>>
    {
        private readonly IRepositorioRelatorioPeriodicoPAPQuestao repositorio;

        public ObterQuestoesMigracaoQueryHandler(IRepositorioRelatorioPeriodicoPAPQuestao repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<IEnumerable<Questao>> Handle(ObterQuestoesMigracaoQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterQuestoesMigracao();
        }
    }
}
