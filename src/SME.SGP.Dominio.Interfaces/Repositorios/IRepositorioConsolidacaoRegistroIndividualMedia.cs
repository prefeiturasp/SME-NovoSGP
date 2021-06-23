using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioConsolidacaoRegistroIndividualMedia
    {
        Task LimparConsolidacaoMediaRegistrosIndividuaisPorAno(int anoLetivo);
        Task<long> Inserir(ConsolidacaoRegistroIndividualMedia consolidacaoRegistroIndividualMedia);
    }
}
