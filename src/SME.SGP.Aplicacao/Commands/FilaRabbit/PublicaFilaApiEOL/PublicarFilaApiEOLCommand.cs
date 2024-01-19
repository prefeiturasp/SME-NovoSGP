using MediatR;
using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class PublicarFilaApiEOLCommand : IRequest<bool>
    {
        public PublicarFilaApiEOLCommand(string rota, object filtros, string exchange = null)
        {
            Filtros = filtros;
            Rota = rota;
            Exchange = exchange;
        }

        public string Rota { get; set; }
        public object Filtros { get; set; }
        public string Exchange { get; set; }
    }
    
    public class PublicarFilaApiEOLCommandValidator : AbstractValidator<PublicarFilaApiEOLCommand>
    {
        public PublicarFilaApiEOLCommandValidator()
        {
            RuleFor(a => a.Filtros)
                .NotEmpty()
                .WithMessage("O payload da mensagem deve ser informado para a execução da fila");
            
            RuleFor(a => a.Rota)
                .NotEmpty()
                .WithMessage("A rota deve ser informado para a execução da fila");
        }
    }
    
    
}
