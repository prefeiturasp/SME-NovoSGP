using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.Consultas
{
    public interface IConsultaMovitosAusencia
    {
        Task<IEnumerable<MotivoAusencia>> Listar();
    }
}
