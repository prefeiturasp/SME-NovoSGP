using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultasPlanoAnual
    {
        private readonly IRepositorioPlanoAnual repositorioPlanoAnual;

        public ConsultasPlanoAnual(IRepositorioPlanoAnual repositorioPlanoAnual)
        {
            this.repositorioPlanoAnual = repositorioPlanoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanoAnual));
        }

        public void Listar()
        {
            var planosAnuais = repositorioPlanoAnual.Listar();
        }
    }
}