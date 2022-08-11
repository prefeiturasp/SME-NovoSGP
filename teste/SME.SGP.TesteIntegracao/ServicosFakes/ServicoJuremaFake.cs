using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ServicoJuremaFake : IServicoJurema
    {
        public async Task<IEnumerable<ObjetivoAprendizagemResposta>> ObterListaObjetivosAprendizagem()
        {
            return new List<ObjetivoAprendizagemResposta>();
        }
    }
}
