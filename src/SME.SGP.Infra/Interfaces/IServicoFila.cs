using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Interfaces
{
    public interface IServicoFila
    {
        void AdicionaFilaWorkerRelatorios(AdicionaFilaDto adicionaFilaDto);
    }
}
