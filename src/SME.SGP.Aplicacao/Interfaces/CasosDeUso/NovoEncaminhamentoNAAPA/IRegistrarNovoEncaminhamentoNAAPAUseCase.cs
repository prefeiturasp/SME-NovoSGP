using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.NovoEncaminhamentoNAAPA
{
    public interface IRegistrarNovoEncaminhamentoNAAPAUseCase
    {
        Task<ResultadoNovoEncaminhamentoNAAPADto> Executar(NovoEncaminhamentoNAAPADto encaminhamentoNAAPADto);
    }
}