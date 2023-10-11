using MediatR;
using SME.SGP.Aplicacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConsultaDisciplina.ServicosFake
{
    public class ObterParametrosSistemaHandlerFake : IRequestHandler<ObterParametroSistemaPorTipoQuery, string>
    {
        public ObterParametrosSistemaHandlerFake() { }
        public async Task<string> Handle(ObterParametroSistemaPorTipoQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult("2020");
        }
    }
}
