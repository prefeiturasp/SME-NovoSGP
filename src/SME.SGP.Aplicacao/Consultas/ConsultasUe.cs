using SME.SGP.Dominio;
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

        public async Task<IEnumerable<ModalidadeRetornoDto>> ObterModalidadesPorUe(string ueCodigo, int ano)
        {
            var listaModalidades = await repositorioUe.ObterModalidades(ueCodigo, ano);

            if (listaModalidades != null && listaModalidades.Any())
            {
                return from b in listaModalidades
                       select new ModalidadeRetornoDto()
                       {
                           Id = (int)b,
                           Nome = b.GetAttribute<DisplayAttribute>().Name
                       };
            }
            else return null;
        }

        public Ue ObterPorId(long id)
            => repositorioUe.ObterPorId(id);

        public Ue ObterPorCodigo(string codigoUe)
            => repositorioUe.ObterPorCodigo(codigoUe);

        public async Task<IEnumerable<TurmaRetornoDto>> ObterTurmas(string ueCodigo, int modalidadeId, int ano)
        {
            var listaTurmas = await repositorioUe.ObterTurmas(ueCodigo, (Modalidade)modalidadeId, ano);

            if (listaTurmas != null && listaTurmas.Any())
            {
                return from b in listaTurmas
                       select new TurmaRetornoDto()
                       {
                           Codigo = b.CodigoTurma,
                           Nome = b.Nome
                       };
            }
            else return null;
        }
    }
}