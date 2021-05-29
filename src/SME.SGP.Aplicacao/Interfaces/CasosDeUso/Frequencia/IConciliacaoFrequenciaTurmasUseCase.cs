using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IConciliacaoFrequenciaTurmasUseCase
    {
        Task Executar();
        Task ProcessarNaData(DateTime dataPeriodo, string turmaCodigo);
    }
}
