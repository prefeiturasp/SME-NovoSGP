using MediatR;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    /// <summary>
    /// Retorna a configuração do cache dos filtros de abrangência (parâmetro UtilizarCacheAbrangencia),
    /// ou null quando o cache está desligado (parâmetro inativo ou inexistente).
    /// </summary>
    public class ObterConfiguracaoCacheAbrangenciaQuery : IRequest<ConfiguracaoCacheAbrangenciaDto>
    {
        private static ObterConfiguracaoCacheAbrangenciaQuery _instance;
        public static ObterConfiguracaoCacheAbrangenciaQuery Instance => _instance ??= new();
    }
}
