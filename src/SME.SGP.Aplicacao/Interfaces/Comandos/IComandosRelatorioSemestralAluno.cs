using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IComandosRelatorioSemestralAluno
    {
        Task<AuditoriaRelatorioSemestralAlunoDto> Salvar(string alunoCodigo, string turmaCodigo, int semestre, long relatorioSemestralId, long relatorioSemestralAlunoId, RelatorioSemestralAlunoPersistenciaDto relatorioSemestralAlunoDto);
    }
}
