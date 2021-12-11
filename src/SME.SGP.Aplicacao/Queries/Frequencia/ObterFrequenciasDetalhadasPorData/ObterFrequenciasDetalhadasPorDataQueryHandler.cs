using System;
using System.Collections.Generic;
using SME.SGP.Infra;
using System.Text;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading.Tasks;
using System.Threading;

namespace SME.SGP.Aplicacao.Queries.Frequencia.ObterFrequenciasDetalhadasPorData
{
    public class ObterFrequenciasDetalhadasPorDataQueryHandler : IRequestHandler<ObterFrequenciasDetalhadasPorDataQuery, IEnumerable<FrequenciaDetalhadaPorDataDto>>
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;

        public ObterFrequenciasDetalhadasPorDataQueryHandler(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }

        public async Task<IEnumerable<FrequenciaDetalhadaPorDataDto>> Handle(ObterFrequenciasDetalhadasPorDataQuery request, CancellationToken cancellationToken)
        {
            var frequenciasPorData = await repositorioFrequencia.ObterFrequenciasDetalhadasPorData(request.CodigoAluno, request.DataInicio, request.DataFim);

            return frequenciasPorData;
        }
    }
}
