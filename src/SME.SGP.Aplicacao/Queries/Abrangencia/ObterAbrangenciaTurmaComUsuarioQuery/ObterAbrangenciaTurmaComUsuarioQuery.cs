using FluentValidation;
using MediatR;
using SME.SGP.Dto;
using System;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaTurmaComUsuarioQuery : IRequest<AbrangenciaFiltroRetorno>
    {
        public ObterAbrangenciaTurmaComUsuarioQuery(string turmaCodigo, bool ehAbrangenciaUeOuDreOuSme, bool consideraHistorico = false)
        {
            TurmaCodigo = turmaCodigo;
            EhAbrangenciaUeOuDreOuSme = ehAbrangenciaUeOuDreOuSme;
            ConsideraHistorico = consideraHistorico;
        }

        public string TurmaCodigo { get; set; }
        public bool EhAbrangenciaUeOuDreOuSme { get; set; }
        public bool ConsideraHistorico { get; set; }
    }
    public class ObterAbrangenciaTurmaComUsuarioQueryValidator : AbstractValidator<ObterAbrangenciaTurmaComUsuarioQuery>
    {
        public ObterAbrangenciaTurmaComUsuarioQueryValidator()
        {
            RuleFor(x => x.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para consultar a abrangência do usuário.");
        }
    }
}
