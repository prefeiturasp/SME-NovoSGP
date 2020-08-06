using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAnoEscolar
    {
        Task<IEnumerable<ModalidadeAnoDto>> ObterPorModalidadeCicloIdAbrangencia(Modalidade modalidade, long cicloId, long usuarioId, Guid perfil);
    }
}
