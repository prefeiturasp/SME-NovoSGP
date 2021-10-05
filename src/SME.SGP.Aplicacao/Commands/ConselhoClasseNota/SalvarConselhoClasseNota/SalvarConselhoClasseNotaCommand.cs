using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarConselhoClasseNotaCommand : IRequest
    {
        public SalvarConselhoClasseNotaCommand(ConselhoClasseNota conselhoClasseNota)
        {
            ConselhoClasseNota = conselhoClasseNota;
        }

        public ConselhoClasseNota ConselhoClasseNota { get; }
    }
}
