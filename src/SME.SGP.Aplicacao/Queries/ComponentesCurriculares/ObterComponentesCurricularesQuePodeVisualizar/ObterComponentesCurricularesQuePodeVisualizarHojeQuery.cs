using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesQuePodeVisualizarHojeQuery : IRequest<string[]>
    {
        public ObterComponentesCurricularesQuePodeVisualizarHojeQuery(string rfLogado, Guid perfilAtual, string turmaCodigo, bool realizarAgrupamentoComponente = false)
        {
            RfLogado = rfLogado;
            PerfilAtual = perfilAtual;
            TurmaCodigo = turmaCodigo;
            RealizarAgrupamentoComponente = realizarAgrupamentoComponente;
        }
        public string RfLogado { get; set; }
        public Guid PerfilAtual { get; set; }
        public string TurmaCodigo { get; set; }
        public bool RealizarAgrupamentoComponente { get; set; }
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
