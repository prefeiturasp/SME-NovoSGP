using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasComFrequenciaPorTurmaCodigoQuery : IRequest<IEnumerable<AulaComFrequenciaNaDataDto>>
    {
        public ObterAulasComFrequenciaPorTurmaCodigoQuery(string turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }

        public string TurmaCodigo { get; }
    }
}
