using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class ObterSupervisorPorCodigoQueryHandlerFake : IRequestHandler<ObterSupervisorPorCodigoDreQuery, IEnumerable<SupervisoresRetornoDto>>
    {
        public async Task<IEnumerable<SupervisoresRetornoDto>> Handle(ObterSupervisorPorCodigoDreQuery request, CancellationToken cancellationToken)
        {
            return new List<SupervisoresRetornoDto> {
                new SupervisoresRetornoDto() {CodigoRf = "1", NomeServidor = "Jose Teste" } ,
                new SupervisoresRetornoDto() {CodigoRf = "2", NomeServidor = "Carlos Teste" }
            };
        }
    }


}
