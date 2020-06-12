using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ObterCiclosPorUeModalidadeUseCase : IObterCiclosPorUeModalidadeUseCase
    {
        public Task<IEnumerable<CicloDto>> Executar(IMediator mediator, string ueCodigo, Modalidade modalidade)
        {
            throw new NotImplementedException();
        }
    }
}
