using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterSupervisoresPorDreEolQuery : IRequest<IEnumerable<SupervisoresRetornoDto>>
    {
        public ObterSupervisoresPorDreEolQuery(string dreCodigo)
        {
            DreCodigo = dreCodigo;
        }

        public string DreCodigo { get; set; }
    }
}
