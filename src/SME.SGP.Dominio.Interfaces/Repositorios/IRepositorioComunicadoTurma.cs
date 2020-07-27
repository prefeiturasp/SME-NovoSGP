using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioComunicadoTurma : IRepositorioBase<ComunicadoTurma>
    {
        Task RemoverTodasTurmasComunicado(long comunicadoId);

        Task<IEnumerable<ComunicadoTurma>> ObterPorComunicado(long comunicadoId);
    }
}
