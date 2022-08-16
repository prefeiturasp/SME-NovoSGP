using System;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterCacheObjetoQuery<T>: IRequest<T>
    {
        public ObterCacheObjetoQuery(string nomeChave, Func<Task<T>> buscarDados, int minutosParaExpirar = 720, 
            bool utilizarGZip = false)
        {
            NomeCache = nomeChave;
            BuscarDados = buscarDados;
            MinutosParaExpirar = minutosParaExpirar;
            UtilizarGZip = utilizarGZip;
        }

        public string NomeCache { get; set; }
        public Func<Task<T>> BuscarDados { get; set; }
        public int MinutosParaExpirar { get; set; }
        public bool UtilizarGZip { get; set; }
    }

    public class ObterCacheObjetoQueryValidator<T> : AbstractValidator<ObterCacheObjetoQuery<T>>
    {
        public ObterCacheObjetoQueryValidator()
        {
            RuleFor(x => x.NomeCache).NotEmpty().WithMessage("Informe o nome da chave do cache");
            RuleFor(x => x.BuscarDados).NotNull().WithMessage("Informe os dados para busca do cache");
        }
    }
}