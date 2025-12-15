using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.NovoEncaminhamentoNAAPA
{
    public interface IObterNovoEncaminhamentoNAAPAPorIdUseCase : IUseCase<long, NovoEncaminhamentoNAAPARespostaDto>
    {
    }
}