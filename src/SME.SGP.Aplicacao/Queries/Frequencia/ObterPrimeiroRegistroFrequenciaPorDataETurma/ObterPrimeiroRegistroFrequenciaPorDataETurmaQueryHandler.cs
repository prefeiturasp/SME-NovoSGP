using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Frequencia;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPrimeiroRegistroFrequenciaPorDataETurmaQueryHandler : IRequestHandler<ObterPrimeiroRegistroFrequenciaPorDataETurmaQuery, ComponenteCurricularSugeridoDto>
    {
        private readonly IRepositorioFrequenciaConsulta repositorioFrequencia;

        public ObterPrimeiroRegistroFrequenciaPorDataETurmaQueryHandler(IRepositorioFrequenciaConsulta repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }

        public async Task<ComponenteCurricularSugeridoDto> Handle(ObterPrimeiroRegistroFrequenciaPorDataETurmaQuery request, CancellationToken cancellationToken)
            => await repositorioFrequencia.ObterPrimeiroRegistroFrequenciaPorDataETurma(request.TurmaId, request.DataAula);
    }
}
