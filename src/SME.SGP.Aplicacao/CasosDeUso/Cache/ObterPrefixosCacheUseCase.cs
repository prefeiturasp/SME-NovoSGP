using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPrefixosCacheUseCase : IObterPrefixosCacheUseCase
    {
        private readonly RedisOptions config;

        public ObterPrefixosCacheUseCase(RedisOptions config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public IEnumerable<string> Executar()
        {
            foreach(var chave in typeof(NomeChaveCache).ObterConstantesPublicas<string>())
            {
                yield return $"{config.Prefixo}{chave.Split(':')[0]}";
            }
        }
    }
}
