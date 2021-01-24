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
    public class ObterDocumentoUseCase : AbstractUseCase, IObterDocumentoUseCase
    {
        public ObterDocumentoUseCase(IMediator mediator) : base(mediator)
        { }

        public async Task<ObterDocumentoDto> Executar(long documentoId)
        {
            return await mediator.Send(new ObterDocumentoPorIdCompletoQuery(documentoId));
        }
    }
}
