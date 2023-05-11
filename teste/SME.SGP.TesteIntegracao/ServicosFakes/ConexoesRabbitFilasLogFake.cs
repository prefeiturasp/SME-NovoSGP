using Microsoft.Extensions.ObjectPool;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ConexoesRabbitFilasLogFake : ConexoesRabbitFake, IConexoesRabbitFilasLog
    {
        public ConexoesRabbitFilasLogFake(ConfiguracaoRabbitLogOptions configuracaoRabbit,
            ObjectPoolProvider poolProvider) : base(configuracaoRabbit, poolProvider)
        {
        }
    }
}