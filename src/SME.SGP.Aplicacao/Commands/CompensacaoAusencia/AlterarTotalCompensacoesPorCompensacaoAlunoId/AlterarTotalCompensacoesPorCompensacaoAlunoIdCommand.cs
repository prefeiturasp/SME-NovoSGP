using MediatR;

namespace SME.SGP.Aplicacao
{
    public class AlterarTotalCompensacoesPorCompensacaoAlunoIdCommand : IRequest<bool>
    {
        public long CompensacaoAlunoId { get; set; }
        public int Quantidade { get; set; }
    }
}
