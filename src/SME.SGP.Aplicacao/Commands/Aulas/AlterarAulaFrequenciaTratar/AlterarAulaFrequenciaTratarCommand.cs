using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class AlterarAulaFrequenciaTratarCommand : IRequest<bool>
    {
        public AlterarAulaFrequenciaTratarCommand(Aula aula, int quantidadeAulasOriginal)
        {
            Aula = aula;
            QuantidadeAulasOriginal = quantidadeAulasOriginal;
        }

        public Aula Aula { get; set; }
        public int QuantidadeAulasOriginal { get; set; }
    }
}
