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
    public class ObterPAAIPorUeUseCase : AbstractUseCase, IObterPAAIPorUeUseCase
    {
        public ObterPAAIPorUeUseCase(IMediator mediator) : base(mediator) { }

        public async Task<IEnumerable<ServidorDto>> Executar(string codigoUe)
        {
            return await mediator.Send(new ObterPAAIPorUeTipoResponsavelAtribuicaoQuery(codigoUe, TipoResponsavelAtribuicao.PAAI));
        }
    }
}
