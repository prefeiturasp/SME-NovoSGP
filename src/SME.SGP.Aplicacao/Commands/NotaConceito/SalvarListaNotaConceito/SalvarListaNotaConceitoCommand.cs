using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarListaNotaConceitoCommand : IRequest<bool>
    {
        public SalvarListaNotaConceitoCommand(List<NotaConceito> listaNotasConceitos, Usuario criadoPor)
        {
            ListaNotasConceitos = listaNotasConceitos;
            CriadoPor = criadoPor;
        }

        public List<NotaConceito> ListaNotasConceitos { get; set; }
        public Usuario CriadoPor { get; set; }
    }

    public class SalvarListaNotaConceitoCommandValidator : AbstractValidator<SalvarListaNotaConceitoCommand>
    {
        public SalvarListaNotaConceitoCommandValidator()
        {
            RuleFor(x => x.CriadoPor)
                .NotEmpty().WithMessage("Informe o Usuario criador para Salvar Lista Nota Conceito");
            RuleFor(c => c.ListaNotasConceitos)
                .Must(a => a.Any())
                .WithMessage("Informe uma Nota Conceito para Salvar Lista Nota Conceito");
        }
    }
}