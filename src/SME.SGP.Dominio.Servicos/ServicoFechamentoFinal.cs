using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamentoFinal : IServicoFechamentoFinal
    {
        private readonly IRepositorioFechamentoFinal repositorioFechamentoFinal;
        private readonly IRepositorioPeriodoFechamento repositorioPeriodoFechamento;

        public ServicoFechamentoFinal(IRepositorioFechamentoFinal repositorioFechamentoFinal, IRepositorioPeriodoFechamento repositorioPeriodoFechamento)
        {
            this.repositorioFechamentoFinal = repositorioFechamentoFinal ?? throw new ArgumentNullException(nameof(repositorioFechamentoFinal));
            this.repositorioPeriodoFechamento = repositorioPeriodoFechamento ?? throw new ArgumentNullException(nameof(repositorioPeriodoFechamento));
        }

        public async Task SalvarAsync(FechamentoFinal fechamentoFinal)
        {
            var periodoFechamento = repositorioPeriodoFechamento.ObterPorFiltros(null, null, null, fechamentoFinal.TurmaId);
            if (periodoFechamento == null)
                throw new NegocioException("Não foi possível localizar um fechamento de período para esta turma.");

            //fechamentoFinal.PodeSalvar()
        }
    }
}