using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasComFrequenciaPorTurmaCodigoQueryHandler : IRequestHandler<ObterAulasComFrequenciaPorTurmaCodigoQuery, IEnumerable<AulaComFrequenciaNaDataDto>>
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;

        public ObterAulasComFrequenciaPorTurmaCodigoQueryHandler(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }

        public async Task<IEnumerable<AulaComFrequenciaNaDataDto>> Handle(ObterAulasComFrequenciaPorTurmaCodigoQuery request, CancellationToken cancellationToken)
            => await repositorioFrequencia.ObterAulasComRegistroFrequenciaPorTurma(request.TurmaCodigo);
    }
}
