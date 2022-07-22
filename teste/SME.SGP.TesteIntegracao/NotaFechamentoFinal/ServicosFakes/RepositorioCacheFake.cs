using SME.SGP.Dados.Repositorios;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.NotaFechamento.ServicosFakes
{
    public class RepositorioCacheFake : RepositorioCache
    {
        public RepositorioCacheFake(IServicoTelemetria servicoTelemetria) : base(servicoTelemetria)
        {
        }
        
        protected override string ObterValor(string nomeChave)
        {
            return string.Empty;
        }
    }
}