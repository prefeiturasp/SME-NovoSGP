using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarCachePorValorStringCommand : IRequest
    {
        public SalvarCachePorValorStringCommand(string nomeChave, string valor, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            NomeChave = nomeChave;
            Valor = valor;
            MinutosParaExpirar = minutosParaExpirar;
            UtilizarGZip = utilizarGZip;
        }

        public bool UtilizarGZip { get; set; }

        public int MinutosParaExpirar { get; set; }

        public string Valor { get; set; }

        public string NomeChave { get; set; }
    }

    public class SalvarCachePorValorStringQueryValidor : AbstractValidator<SalvarCachePorValorStringCommand>
    {
        public SalvarCachePorValorStringQueryValidor()
        {
            RuleFor(x => x.NomeChave).NotEmpty().WithMessage("Informe o nome da chave para salvar o cache");
            RuleFor(x => x.Valor).NotEmpty().WithMessage("Informe o valor para salvar o cache");
        }
    }
}