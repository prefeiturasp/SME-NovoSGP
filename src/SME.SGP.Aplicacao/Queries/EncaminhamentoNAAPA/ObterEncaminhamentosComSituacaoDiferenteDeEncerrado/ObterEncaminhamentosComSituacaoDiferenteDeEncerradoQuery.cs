using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentosComSituacaoDiferenteDeEncerradoQuery : IRequest<IEnumerable<EncaminhamentoNAAPADto>>
    {
    }
}
