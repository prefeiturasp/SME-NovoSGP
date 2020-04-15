using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasConselhoClasse
    {
        ConselhoClasse ObterPorId(long conselhoClasseId);
        Task<(int, bool)> ValidaConselhoClasseUltimoBimestre(Turma turma);
    }
}
