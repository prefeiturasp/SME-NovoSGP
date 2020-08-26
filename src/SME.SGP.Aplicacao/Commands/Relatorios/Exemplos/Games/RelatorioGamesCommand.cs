using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RelatorioGamesCommand : IRequest<bool>
    {
        public RelatorioGamesCommand(int ano)
        {
            Ano = ano;
        }

        public int Ano { get; set; }
    }
}
