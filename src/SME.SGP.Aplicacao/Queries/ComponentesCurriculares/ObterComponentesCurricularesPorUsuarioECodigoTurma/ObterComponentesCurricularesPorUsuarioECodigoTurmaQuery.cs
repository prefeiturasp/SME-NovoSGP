using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery : IRequest<IEnumerable<DisciplinaNomeDto>>
    {
        public ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery(Usuario usuarioLogado, string turmaCodigo)
        {
            UsuarioLogado = usuarioLogado;
            TurmaCodigo = turmaCodigo;
        }

        public Usuario UsuarioLogado { get; set; }
        public string TurmaCodigo { get; set; }
    }

    public class ObterComponentesCurricularesPorUsuarioECodigoTurmaQueryValidator : AbstractValidator<ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery>
    {
        public ObterComponentesCurricularesPorUsuarioECodigoTurmaQueryValidator()
        {
            RuleFor(c => c.UsuarioLogado)
            .NotEmpty()
            .WithMessage("O usuario logado deve ser informado para consulta de componentes curriculares.");

            RuleFor(c => c.TurmaCodigo)
            .NotEmpty()
            .WithMessage("O código da turma deve ser informado para consulta de componentes curriculares.");

        }
    }
}
