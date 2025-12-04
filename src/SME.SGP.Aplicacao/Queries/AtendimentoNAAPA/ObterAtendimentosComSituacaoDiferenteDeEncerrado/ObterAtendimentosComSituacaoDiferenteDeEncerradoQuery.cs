using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAtendimentosComSituacaoDiferenteDeEncerradoQuery : IRequest<IEnumerable<AtendimentoNAAPADto>>
    {
        private static ObterAtendimentosComSituacaoDiferenteDeEncerradoQuery _instance;
        public static ObterAtendimentosComSituacaoDiferenteDeEncerradoQuery Instance => _instance ??= new();
    }
}
