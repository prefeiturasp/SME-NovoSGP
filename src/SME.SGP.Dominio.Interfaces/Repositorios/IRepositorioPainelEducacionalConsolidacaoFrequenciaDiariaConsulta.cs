using SME.SGP.Infra.Dtos.PainelEducacional.Frequencia;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalConsolidacaoFrequenciaDiariaConsulta
    {
        Task<IEnumerable<DadosParaConsolidarFrequenciaSemanalAlunoDto>> ObterFrequenciaSemanal(IEnumerable<DateTime> dataAulas);
    }
}
