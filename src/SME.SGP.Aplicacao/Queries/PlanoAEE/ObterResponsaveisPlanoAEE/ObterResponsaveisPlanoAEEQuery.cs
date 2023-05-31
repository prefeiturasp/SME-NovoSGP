using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsaveisPlanoAEEQuery : IRequest<IEnumerable<UsuarioEolRetornoDto>>
    {
        public ObterResponsaveisPlanoAEEQuery(FiltroPlanosAEEDto filtro)
        {
            DreId = filtro.DreId;
            UeId = filtro.UeId;
            TurmaId = filtro.TurmaId;
            AlunoCodigo = filtro.AlunoCodigo;
            Situacao = filtro.Situacao;
            ExibirEncerrados = filtro.ExibirEncerrados;
        }

        public long DreId { get; }
        public long UeId { get; }
        public long TurmaId { get; }
        public string AlunoCodigo { get; }
        public SituacaoPlanoAEE? Situacao { get; }
        public bool ExibirEncerrados { get; set; }
    }

    public class ObterResponsaveisPlanoAEEQueryValidator : AbstractValidator<ObterResponsaveisPlanoAEEQuery>
    {
        public ObterResponsaveisPlanoAEEQueryValidator()
        {
            RuleFor(c => c.DreId)
            .NotEmpty()
            .WithMessage("A DRE deve ser informada para pesquisa de responsáveis dos Planos AEE");
        }
    }
}
