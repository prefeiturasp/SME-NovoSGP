using System.Collections.Generic;
using SME.SGP.Aplicacao.Integracoes.Respostas;

namespace SME.SGP.Aplicacao.Integracoes
{
    public interface IServicoJurema
    {
        IEnumerable<ObjetivoAprendizagemResposta> ObterListaObjetivosAprendizagem();
    }
}