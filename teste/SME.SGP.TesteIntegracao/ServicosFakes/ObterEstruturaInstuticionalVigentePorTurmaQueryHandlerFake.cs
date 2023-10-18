using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dto;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterEstruturaInstuticionalVigentePorTurmaQueryHandlerFake : IRequestHandler<ObterEstruturaInstuticionalVigentePorTurmaQuery, EstruturaInstitucionalRetornoEolDTO>
    {
       public async Task<EstruturaInstitucionalRetornoEolDTO> Handle(ObterEstruturaInstuticionalVigentePorTurmaQuery request, CancellationToken cancellationToken)
       {
           return null;
       }
    }
}
