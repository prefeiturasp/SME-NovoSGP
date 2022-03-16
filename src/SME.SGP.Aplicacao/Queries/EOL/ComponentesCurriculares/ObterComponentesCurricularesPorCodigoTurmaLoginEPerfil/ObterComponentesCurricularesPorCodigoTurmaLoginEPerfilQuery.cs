using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilQuery : IRequest<IEnumerable<ComponenteCurricularEol>>
    {
        public ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilQuery(string codigoTurma, string login, Guid perfil, bool realizarAgrupamentoComponente = false)
        {
            CodigoTurma = codigoTurma;
            Login = login;
            Perfil = perfil;
            RealizarAgrupamentoComponente = realizarAgrupamentoComponente;
        }

        public string CodigoTurma { get; }
        public string Login { get; }
        public Guid Perfil { get; }
        public bool RealizarAgrupamentoComponente { get; set; }
    }

    public class ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilQueryValidator : AbstractValidator<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilQuery>
    {
        public ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilQueryValidator()
        {
            RuleFor(c => c.CodigoTurma)
            .NotEmpty()
            .WithMessage("O código da turma deve ser informado para consulta de componentes curriculares no EOL.");

            RuleFor(c => c.Login)
            .NotEmpty()
            .WithMessage("O login deve ser informado para consulta de componentes curriculares no EOL.");

            RuleFor(c => c.Perfil)
            .NotEmpty()
            .WithMessage("O perfil deve ser informado para consulta de componentes curriculares  EOL.");
        }
    }
}
