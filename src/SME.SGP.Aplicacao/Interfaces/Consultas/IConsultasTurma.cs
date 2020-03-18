using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasTurma
    {
        Task<Turma> ObterPorCodigo(string codigoTurma);
        Task<Turma> ObterComUeDrePorCodigo(string codigoTurma);
        Task<Turma> ObterComUeDrePorId(long turmaId);
        Task<bool> TurmaEmPeriodoAberto(string codigoTurma, DateTime dataReferencia);
        Task<bool> TurmaEmPeriodoAberto(long turmaId, DateTime dataReferencia);
    }
}
