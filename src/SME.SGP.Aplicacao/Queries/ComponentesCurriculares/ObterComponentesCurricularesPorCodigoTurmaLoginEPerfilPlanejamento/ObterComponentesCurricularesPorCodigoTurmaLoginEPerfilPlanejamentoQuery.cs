using MediatR;
using System;
using System.Collections.Generic;
using FluentValidation;
using SME.SGP.Aplicacao.Integracoes.Respostas;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilPlanejamentoQuery : IRequest<IEnumerable<DisciplinaResposta>>
    {
        public ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilPlanejamentoQuery(string codigoTurma, string login, Guid perfil)
        {
            CodigoTurma = codigoTurma;
            Login = login;
            Perfil = perfil;
        }

        public string CodigoTurma { get; set; }

        public string Login { get; set; }

        public Guid Perfil { get; set; }
    }
   
    public class ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilPlanejamentoQueryValidator : AbstractValidator<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilPlanejamentoQuery>
    {
        public ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilPlanejamentoQueryValidator()
        {
            RuleFor(c => c.CodigoTurma)
                .NotEmpty()
                .NotNull()
                .WithMessage("O código da turma deve ser informado para obter componentes curriculares por turma, login e perfil.");

            RuleFor(c => c.Login)
                .NotEmpty()
                .WithMessage("O login deve ser informado para obter componentes curriculares por turma, login e perfil.");

            RuleFor(c => c.Perfil)
                .NotEmpty()
                .WithMessage("O perfil deve ser informado para obter componentes curriculares por turma, login e perfil.");
        }
    }
}