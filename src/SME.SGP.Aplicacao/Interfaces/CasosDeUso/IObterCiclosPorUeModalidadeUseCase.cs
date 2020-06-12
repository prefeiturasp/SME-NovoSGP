using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterCiclosPorUeModalidadeUseCase
    {
        Task<IEnumerable<CicloDto>> Executar(IMediator mediator, string ueCodigo, Modalidade modalidade);
    }
}
