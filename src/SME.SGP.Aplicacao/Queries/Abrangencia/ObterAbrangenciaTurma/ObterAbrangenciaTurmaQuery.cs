using FluentValidation;
using MediatR;
using SME.SGP.Dto;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaTurmaQuery : IRequest<AbrangenciaFiltroRetorno>
    {
        public ObterAbrangenciaTurmaQuery(string turmaCodigo, string login, Guid perfil, bool consideraHistorico = false, bool abrangenciaPermitida = false)
        {
            TurmaCodigo = turmaCodigo;
            Login = login;
            Perfil = perfil;
            ConsideraHistorico = consideraHistorico;
            AbrangenciaPermitida = abrangenciaPermitida;
        }

        public string TurmaCodigo { get; set; }
        public string Login { get; set; }
        public Guid Perfil { get; set; }
        public bool ConsideraHistorico { get; set; }
        public bool AbrangenciaPermitida { get; set; }
    }
    public class ObterAbrangenciaTurmaQueryValidator : AbstractValidator<ObterAbrangenciaTurmaQuery>
    {
        public ObterAbrangenciaTurmaQueryValidator()
        {
            RuleFor(x => x.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");

            RuleFor(x => x.Login)
                .NotEmpty()
                .WithMessage("O login do usuário logado deve ser informado.");

            RuleFor(x => x.Perfil)
                .NotEmpty()
                .WithMessage("O perfil do usuário logado deve ser informado.");
        }
    }
}
