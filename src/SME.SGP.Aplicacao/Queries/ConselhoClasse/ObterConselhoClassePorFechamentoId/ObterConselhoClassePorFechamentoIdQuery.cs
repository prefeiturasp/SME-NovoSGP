using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClassePorFechamentoIdQuery : IRequest<ConselhoClasse>
    {
        public ObterConselhoClassePorFechamentoIdQuery(long fechamentoId)
        {
            FechamentoId = fechamentoId;
        }

        public long FechamentoId { get; set; }
    }
}
