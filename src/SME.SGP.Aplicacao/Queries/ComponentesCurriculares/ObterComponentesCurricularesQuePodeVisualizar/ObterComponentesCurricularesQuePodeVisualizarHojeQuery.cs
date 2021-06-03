using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesQuePodeVisualizarHojeQuery : IRequest<string[]>
    {
        public ObterComponentesCurricularesQuePodeVisualizarHojeQuery(string rfLogado, Guid perfilAtual, string turmaCodigo)
        {
            RfLogado = rfLogado;
            PerfilAtual = perfilAtual;
            TurmaCodigo = turmaCodigo;
        }
        public string RfLogado { get; set; }
        public Guid PerfilAtual { get; set; }
        public string TurmaCodigo { get; set; }
    }
    public class ObterComponentesCurricularesQuePodeVisualizarHojeQueryValidator : AbstractValidator<ObterComponentesCurricularesQuePodeVisualizarHojeQuery>
    {
        public ObterComponentesCurricularesQuePodeVisualizarHojeQueryValidator()
        {
            RuleFor(c => c.RfLogado)
            .NotEmpty()
            .WithMessage("O usuario logado deve ser informado para consulta de componentes curriculares.");

            RuleFor(c => c.PerfilAtual)
            .NotEmpty()
            .WithMessage("O perfil atual deve ser informado para consulta de componentes curriculares.");

            RuleFor(c => c.TurmaCodigo)
            .NotEmpty()
            .WithMessage("O código da turma deve ser informado para consulta de componentes curriculares.");

        }
    }
}
