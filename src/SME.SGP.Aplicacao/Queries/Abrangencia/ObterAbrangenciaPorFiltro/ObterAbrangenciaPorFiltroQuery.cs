using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaPorFiltroQuery : IRequest<IEnumerable<AbrangenciaFiltroRetorno>>
    {
        public ObterAbrangenciaPorFiltroQuery(string texto, bool consideraHistorico, Usuario usuario)
        {
            Texto = texto;
            ConsideraHistorico = consideraHistorico;
            Usuario = usuario;
        }

        public string Texto { get; set; }
        public bool ConsideraHistorico { get; set; }
        public Usuario Usuario { get; set; }
    }

    public class ObterAbrangenciaPorFiltroQueryValidator : AbstractValidator<ObterAbrangenciaPorFiltroQuery>
    {
        public ObterAbrangenciaPorFiltroQueryValidator()
        {
            RuleFor(x => x.ConsideraHistorico).NotNull().WithMessage("Considera Histórico deve ser informado");
            RuleFor(x => x.Usuario).NotNull().WithMessage("Usuario deve ser informado");
        }
    }
}
