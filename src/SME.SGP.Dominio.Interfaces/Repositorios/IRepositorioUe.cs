using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioUe
    {
        Task<IEnumerable<Modalidade>> ObterModalidades(string ueCodigo, int ano);

        Ue ObterPorId(string ueId);

        Task<IEnumerable<Turma>> ObterTurmas(string ueCodigo, Modalidade modalidade, int ano);

        Ue ObterUEPorTurma(string turmaId);

        IEnumerable<Ue> Sincronizar(IEnumerable<Ue> entidades, IEnumerable<Dre> dres);
    }
}