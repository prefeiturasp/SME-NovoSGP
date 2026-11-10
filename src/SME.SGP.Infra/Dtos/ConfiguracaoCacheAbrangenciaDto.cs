using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra.Dtos
{
    /// <summary>
    /// Configuração do cache dos filtros de abrangência, lida do valor (JSON) do parâmetro UtilizarCacheAbrangencia.
    /// Ex.: {"minutos":5,"cj":1}
    /// </summary>
    public class ConfiguracaoCacheAbrangenciaDto
    {
        public const int MINUTOS_PADRAO = 5;
        public const int MINUTOS_CJ_PADRAO = 1;

        public int Minutos { get; set; } = MINUTOS_PADRAO;
        public int Cj { get; set; } = MINUTOS_CJ_PADRAO;

        public bool EhValida()
            => Minutos >= 1 && Cj >= 1;

        public int ObterMinutosExpiracao(Guid perfil)
            => perfil == Perfis.PERFIL_CJ || perfil == Perfis.PERFIL_CJ_INFANTIL ? Cj : Minutos;
    }
}
