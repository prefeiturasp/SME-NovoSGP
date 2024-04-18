using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesMapeamentoEstudanteSecaoQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {
        public ObterSecoesMapeamentoEstudanteSecaoQuery(long? mapeamentoEstudanteId)
        {
            MapeamentoEstudanteId = mapeamentoEstudanteId;
        }

        public long? MapeamentoEstudanteId { get; }
    }
}
