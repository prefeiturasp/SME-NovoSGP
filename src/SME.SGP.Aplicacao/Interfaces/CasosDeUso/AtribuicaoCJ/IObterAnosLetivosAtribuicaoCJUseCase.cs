using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterAnosLetivosAtribuicaoCJUseCase
    {
        Task<int[]> Executar();
    }
}
