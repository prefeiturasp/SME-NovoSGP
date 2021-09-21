using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterAbrangenciaDresUseCase
    {
        Task<IEnumerable<AbrangenciaDreRetornoDto>> Executar(Modalidade? modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, string filtro = "");
    }
}