using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasUe : IConsultasUe
    {
        private readonly IRepositorioUe repositorioUe;

        public ConsultasUe(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new System.ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<IEnumerable<ModalidadeRetornoDto>> ObterModalidadesPorUe(string ueCodigo)
        {
            var listaModalidades = await repositorioUe.ObterModalidadesPorUe(ueCodigo);

            if (listaModalidades != null && listaModalidades.Any())
            {
                return from b in listaModalidades
                       select new ModalidadeRetornoDto()
                       {
                           Codigo = (int)b,
                           Nome = b.GetAttribute<DisplayAttribute>().Name
                       };
            }
            else return null;
        }
    }
}