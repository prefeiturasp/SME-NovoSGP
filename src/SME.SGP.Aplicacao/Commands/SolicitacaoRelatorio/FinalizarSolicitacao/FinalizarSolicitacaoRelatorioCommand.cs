using MediatR;
using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Aplicacao
{
    public class FinalizarSolicitacaoRelatorioCommand : IRequest<long>
    {
        public FinalizarSolicitacaoRelatorioCommand(SolicitacaoRelatorio solicitacaoRelatorio)
        {
            SolicitacaoRelatorio = solicitacaoRelatorio;
        }

        public SolicitacaoRelatorio SolicitacaoRelatorio { get; set; }
    }
}



