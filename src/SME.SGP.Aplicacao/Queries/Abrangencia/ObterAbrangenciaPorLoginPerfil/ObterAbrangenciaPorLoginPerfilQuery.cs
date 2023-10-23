using System;
using FluentValidation;
using MediatR;
using SME.SGP.Dto;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaPorLoginPerfilQuery : IRequest<AbrangenciaRetornoEolDto>
    {
        public ObterAbrangenciaPorLoginPerfilQuery(string login, Guid perfil)
        {
            Login = login;
            Perfil = perfil;
        }
        public string Login { get; set; }
        public Guid Perfil { get; set; }
    }

    public class ObterAbrangenciaPorLoginPerfilQueryValidator : AbstractValidator<ObterAbrangenciaPorLoginPerfilQuery>
    {
        public ObterAbrangenciaPorLoginPerfilQueryValidator()
        {
            RuleFor(c => c.Login)
                .NotEmpty()
                .WithMessage("O login deve ser informado para obter a abrangência por login e perfil.");

            RuleFor(c => c.Perfil)
              .NotNull()
              .WithMessage("O perfil deve ser informado para obter a abrangência por login e perfil.");
        }
    }
}
