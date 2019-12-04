using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasUe
    {
        private readonly IRepositorioUe repositorioUe;

        public ConsultasUe(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new System.ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<IEnumerable<Modalidade>> ObterModalidadesPorUe(string ueCodigo)
        {
            return await repositorioUe.ObterModalidadesPorUe(ueCodigo);
        }
    }
}