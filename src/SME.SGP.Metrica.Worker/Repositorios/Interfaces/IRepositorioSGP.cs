using System;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.Repositorios.Interfaces
{
    public interface IRepositorioSGP
    {
        Task<int> ObterQuantidadeAcessosDia(DateTime data);
    }
}
