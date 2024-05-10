﻿using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasProfessor
    {
        Task<IEnumerable<ProfessorTurmaDto>> Listar(string codigoRf);

        Task<IEnumerable<ProfessorResumoDto>> ObterResumoAutoComplete(int anoLetivo, string dreId, string ueId,string nomeProfessor);

        Task<ProfessorResumoDto> ObterResumoPorRFUeDreAnoLetivo(string codigoRF, int anoLetivo, string dreId, string ueId, bool buscarOutrosCargos = false, bool buscarPorTodasDre = false);

        Task<IEnumerable<TurmaDto>> ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(string rfProfessor, string codigoEscola, int anoLetivo);
    }
}