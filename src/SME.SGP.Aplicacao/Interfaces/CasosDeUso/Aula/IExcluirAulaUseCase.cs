using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Aula;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IExcluirAulaUseCase : IUseCase<ExcluirAulaDto, RetornoBaseDto>
    {
    }
}
