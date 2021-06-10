using Microsoft.AspNetCore.Http;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface ISalvarFotoEstudanteUseCase : IUseCase<EstudanteFotoDto, Guid>
    {
    }
}
