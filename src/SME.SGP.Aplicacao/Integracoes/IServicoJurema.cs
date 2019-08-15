using System.Net.Http;

namespace SME.SGP.Aplicacao.Integracoes
{
    public interface IServicoJurema
    {
        HttpClient HttpClient { get; }

        void ObterListaObjetivosAprendizagem();
    }
}