using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaPorDescricaoTipoQuery : IRequest<long>
    {
        public string Descricao { get; set; }
        public TipoPendencia TipoPendencia { get; set; }

        public ObterPendenciaPorDescricaoTipoQuery(string descricao, TipoPendencia tipo)
        {
            Descricao = descricao;
            TipoPendencia = tipo;
        }
    }

    public class ObterPendenciaPorDescricaoTipoQueryValidator : AbstractValidator<ObterPendenciaPorDescricaoTipoQuery>
    {
        public ObterPendenciaPorDescricaoTipoQueryValidator()
        {
            RuleFor(a => a.Descricao)
                .NotEmpty()
                .WithMessage("É necessário informar a descrição da pendência para verificar se já existe pendência do tipo.");
        }
    }
}
