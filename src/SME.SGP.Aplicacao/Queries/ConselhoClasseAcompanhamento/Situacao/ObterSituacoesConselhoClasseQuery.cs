using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
   public class ObterSituacoesConselhoClasseQuery : IRequest<List<SituacaoDto>>
    {
    }
}
