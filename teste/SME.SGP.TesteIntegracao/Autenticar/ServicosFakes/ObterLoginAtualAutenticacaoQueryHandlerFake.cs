using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Autenticar
{
    public class ObterLoginAtualAutenticacaoQueryHandlerFake : IRequestHandler<ObterLoginAtualQuery, string>
    {
        public async Task<string> Handle(ObterLoginAtualQuery request, CancellationToken cancellationToken)
         => await Task.FromResult("PROFINF1");
    }
}
