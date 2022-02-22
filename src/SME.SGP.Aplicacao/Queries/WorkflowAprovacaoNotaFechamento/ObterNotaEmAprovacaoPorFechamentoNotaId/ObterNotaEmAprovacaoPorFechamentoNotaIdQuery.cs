using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaEmAprovacaoPorFechamentoNotaIdQuery : IRequest<IEnumerable<FechamentoNotaAprovacaoDto>>
    {
        public IEnumerable<long> IdsFechamentoNota { get; set; }
    }
}
