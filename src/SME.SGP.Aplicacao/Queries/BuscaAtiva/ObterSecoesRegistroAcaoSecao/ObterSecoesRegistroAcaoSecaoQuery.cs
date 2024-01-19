using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesRegistroAcaoSecaoQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {
        public ObterSecoesRegistroAcaoSecaoQuery(long? registroAcaoId)
        {
            RegistroAcaoId = registroAcaoId;
        }

        public long? RegistroAcaoId { get; }
    }
}
