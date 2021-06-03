using SME.SGP.Dominio;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterUEsPorDreUseCase
    {
        Task<IEnumerable<AbrangenciaUeRetorno>> Executar(string codigoDre, Modalidade? modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, bool consideraNovasUEs = false, string filtro = "");
    }
}
