using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasUe : IConsultasUe
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ConsultasUe(IRepositorioUeConsulta repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new System.ArgumentNullException(nameof(repositorioUe));
        }

        public Ue ObterPorId(long id)
            => repositorioUe.ObterPorId(id);

        public Ue ObterPorCodigo(string codigoUe)
            => repositorioUe.ObterPorCodigo(codigoUe);

        public async Task<IEnumerable<TurmaRetornoDto>> ObterTurmas(string ueCodigo, int modalidadeId, int ano, bool ehHistorico)
        {
            var listaTurmas = await repositorioUe.ObterTurmas(ueCodigo, (Modalidade)modalidadeId, ano, ehHistorico);

            if (listaTurmas != null && listaTurmas.Any())
            {
                return from b in listaTurmas
                       select new TurmaRetornoDto()
                       {
                           Codigo = b.CodigoTurma,
                           Nome = !string.IsNullOrEmpty(b.NomeFiltro) ? b.NomeFiltro :  b.Nome
                       };
            }
            else return null;
        }
    }
}