using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasIdepPainelEducacionalUseCase
    {
        Task<IEnumerable<PainelEducacionalConsolidacaoIdep>> ObterIdepPorAnoEtapa(int ano, int etapa);
    }
}
