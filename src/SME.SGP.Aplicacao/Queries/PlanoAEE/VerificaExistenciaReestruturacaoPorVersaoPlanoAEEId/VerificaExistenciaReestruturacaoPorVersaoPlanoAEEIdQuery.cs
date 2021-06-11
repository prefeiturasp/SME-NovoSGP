using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class VerificaExistenciaReestruturacaoPorVersaoPlanoAEEIdQuery : IRequest<bool>
    {
        public VerificaExistenciaReestruturacaoPorVersaoPlanoAEEIdQuery(long versaoId, long reestruturacaoId)
        {
            VersaoId = versaoId;
            ReestruturacaoId = reestruturacaoId;
        }

        public long VersaoId { get; }
        public long ReestruturacaoId { get; }
    }

    public class VerificaExistenciaReestruturacaoPorVersaoPlanoAEEIdQueryValidator : AbstractValidator<VerificaExistenciaReestruturacaoPorVersaoPlanoAEEIdQuery>
    {
        public VerificaExistenciaReestruturacaoPorVersaoPlanoAEEIdQueryValidator()
        {
            RuleFor(a => a.VersaoId)
                .NotEmpty()
                .WithMessage("O id da versão do plano AEE deve ser informado para consulta de reestruturação");
        }
    }
}
