using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasFechamentoTurma
    {
        Task<FechamentoTurma> ObterPorTurmaCodigoBimestreAsync(string turmaCodigo, int bimestre = 0);
    }
}
