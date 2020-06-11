using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IBoletimUseCase
    {
        Task<bool> Executar(FiltroRelatorioBoletimDto filtroRelatorioBoletimDto);
    }
}
