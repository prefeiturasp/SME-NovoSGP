using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamentoFinal : IServicoFechamentoFinal
    {
        private readonly IRepositorioFechamentoFinal repositorioFechamentoFinal;

        public ServicoFechamentoFinal(IRepositorioFechamentoFinal repositorioFechamentoFinal)
        {
            this.repositorioFechamentoFinal = repositorioFechamentoFinal ?? throw new ArgumentNullException(nameof(repositorioFechamentoFinal));
        }

        public async Task SalvarAsync(FechamentoFinal fechamentoFinal)
        {
        }
    }
}