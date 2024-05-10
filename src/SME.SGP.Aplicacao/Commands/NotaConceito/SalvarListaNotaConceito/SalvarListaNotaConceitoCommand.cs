using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarListaNotaConceitoCommand : IRequest<bool>
    {
        public SalvarListaNotaConceitoCommand(IEnumerable<NotaConceito> listaNotasConceitos)
        {
            ListaNotasConceitos = listaNotasConceitos;
        }

        public IEnumerable<NotaConceito> ListaNotasConceitos { get; set; }
    }

    public class SalvarListaNotaConceitoCommandValidator : AbstractValidator<SalvarListaNotaConceitoCommand>
    {
        public SalvarListaNotaConceitoCommandValidator()
        {
            RuleFor(c => c.ListaNotasConceitos)
                .Must(a => a.Any())
                .WithMessage("Informe uma Nota Conceito para Salvar Lista Nota Conceito");
        }
    }
}