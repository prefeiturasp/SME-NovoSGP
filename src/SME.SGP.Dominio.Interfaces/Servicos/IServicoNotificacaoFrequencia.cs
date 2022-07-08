using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoNotificacaoFrequencia
    {
        Task ExecutaNotificacaoRegistroFrequencia();
        Task NotificarAlunosFaltosos();
        Task NotificarAlunosFaltososBimestre();
    }
}
