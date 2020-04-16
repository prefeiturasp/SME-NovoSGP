using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoConselhoClasse
    {
        Task<AuditoriaDto> GerarConselhoClasse(ConselhoClasse conselhoClasse);
        Task<AuditoriaConselhoClasseAlunoDto> SalvarConselhoClasseAluno(ConselhoClasseAluno conselhoClasseAluno);
    }
}
