using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentosComSituacaoDiferenteDeEncerradoQuery : IRequest<IEnumerable<AtendimentoNAAPADto>>
    {
        private static ObterEncaminhamentosComSituacaoDiferenteDeEncerradoQuery _instance;
        public static ObterEncaminhamentosComSituacaoDiferenteDeEncerradoQuery Instance => _instance ??= new();
    }
}
