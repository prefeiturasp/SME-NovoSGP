using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarValorObjetQuery : IRequest
    {
        public SalvarValorObjetQuery(string nomeChave, object valor, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            NomeChave = nomeChave;
            Valor = valor;
            MinutosParaExpirar = minutosParaExpirar;
            UtilizarGZip = utilizarGZip;
        }

        public bool UtilizarGZip { get; set; }

        public int MinutosParaExpirar { get; set; }

        public object Valor { get; set; }

        public string NomeChave { get; set; }
    }

    public class SalvarValorObjetQueryValidator : AbstractValidator<SalvarValorObjetQuery>
    {
        public SalvarValorObjetQueryValidator()
        {
            RuleFor(x => x.NomeChave).NotEmpty().WithMessage("Informa o nome da chave para salvar o cache");
            RuleFor(x => x.Valor).NotNull().WithMessage("Informa o valor para salvar o cache");
        }
    }
}