using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoAvaliacaoBimestralQuery : IRequest<TipoAvaliacao>
    {
        private static ObterTipoAvaliacaoBimestralQuery _instance;
        public static ObterTipoAvaliacaoBimestralQuery Instance => _instance ??= new();
    }
}
