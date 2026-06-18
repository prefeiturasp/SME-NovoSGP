using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioComunicadoConsulta : IRepositorioBase<Comunicado>
    {
        Task<IEnumerable<ComunicadoTurmaAlunoDto>> ObterComunicadosAnoAtual();
    }
}