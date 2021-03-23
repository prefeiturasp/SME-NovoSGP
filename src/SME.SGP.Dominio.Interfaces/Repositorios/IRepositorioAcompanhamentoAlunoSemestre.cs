using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAcompanhamentoAlunoSemestre : IRepositorioBase<AcompanhamentoAlunoSemestre>
    {
        Task<int> ObterAnoPorId(long acompanhamentoAlunoSemestreId);
    }
}
