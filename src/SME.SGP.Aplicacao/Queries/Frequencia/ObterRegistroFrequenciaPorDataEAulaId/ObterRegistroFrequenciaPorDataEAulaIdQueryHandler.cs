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
    public class ObterRegistroFrequenciaPorDataEAulaIdQueryHandler : IRequestHandler<ObterRegistroFrequenciaPorDataEAulaIdQuery, IEnumerable<RegistroFrequencia>>
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;

        public ObterRegistroFrequenciaPorDataEAulaIdQueryHandler(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }

        public async Task<IEnumerable<RegistroFrequencia>> Handle(ObterRegistroFrequenciaPorDataEAulaIdQuery request, CancellationToken cancellationToken)
            => await repositorioFrequencia.ObterRegistroFrequenciaPorDataEAulaId(request.DisciplinaId, request.TurmaId, request.DataInicio, request.DataFim);
    }
}
