using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaDiaria;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalRegistroFrequenciaDiariaConsulta
    {
        Task<PaginacaoResultadoDto<RegistroFrequenciaDiariaUeDto>> ObterFrequenciaDiariaPorDre(FiltroFrequenciaDiariaDreDto filtro);
        Task<PaginacaoResultadoDto<RegistroFrequenciaDiariaTurmaDto>> ObterFrequenciaDiariaPorUe(FiltroFrequenciaDiariaUeDto filtro);
    }
}
