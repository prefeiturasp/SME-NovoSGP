using SME.SGP.Aplicacao.Integracoes.Respostas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public interface IServicoJurema
    {
        Task<IEnumerable<ObjetivoAprendizagemResposta>> ObterListaObjetivosAprendizagem();
    }
}