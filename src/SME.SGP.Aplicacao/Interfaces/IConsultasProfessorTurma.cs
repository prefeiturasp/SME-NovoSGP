using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasProfessorTurma
    {
        IEnumerable<ProfessorTurmaDto> Listar(string codigoRf);
    }
}