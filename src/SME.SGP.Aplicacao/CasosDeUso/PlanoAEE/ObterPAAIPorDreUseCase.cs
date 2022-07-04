using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPAAIPorDreUseCase : AbstractUseCase, IObterPAAIPorDreUseCase
    {
        public ObterPAAIPorDreUseCase(IMediator mediator) : base(mediator) { }
        public async Task<IEnumerable<SupervisorEscolasDreDto>> Executar(string codigoDre)
        {
            return await mediator.Send(new ObterPAAIPorDreQuery(codigoDre, TipoResponsavelAtribuicao.PAAI));
        }
    }
}
