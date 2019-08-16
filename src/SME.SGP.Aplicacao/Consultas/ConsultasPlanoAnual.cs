using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultasPlanoAnual
    {
        private readonly IRepositorioPlanoAnual repositorioPlanoAnual;
        private readonly ServicoJurema servicoJurema;

        public ConsultasPlanoAnual(IRepositorioPlanoAnual repositorioPlanoAnual,
                                   ServicoJurema servicoJurema)
        {
            this.repositorioPlanoAnual = repositorioPlanoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanoAnual));
            this.servicoJurema = servicoJurema ?? throw new System.ArgumentNullException(nameof(servicoJurema));
        }

        public void Listar()
        {
            var planosAnuais = repositorioPlanoAnual.Listar();
        }
    }
}