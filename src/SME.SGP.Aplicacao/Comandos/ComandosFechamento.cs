using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosFechamento : IComandosFechamento
    {
        private readonly IServicoFechamento servicoPeriodoFechamento;

        public ComandosFechamento(IServicoFechamento servicoPeriodoFechamento)
        {
            this.servicoPeriodoFechamento = servicoPeriodoFechamento ?? throw new System.ArgumentNullException(nameof(servicoPeriodoFechamento));
        }

        public async Task Salvar(FechamentoDto fechamentoDto)
        {
            await servicoPeriodoFechamento.Salvar(fechamentoDto);
        }
    }
}