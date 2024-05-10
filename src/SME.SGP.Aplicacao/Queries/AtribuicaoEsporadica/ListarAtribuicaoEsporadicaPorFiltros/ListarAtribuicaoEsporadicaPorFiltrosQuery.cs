using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ListarAtribuicaoEsporadicaPorFiltrosQuery : IRequest<PaginacaoResultadoDto<AtribuicaoEsporadica>>
    {
        public ListarAtribuicaoEsporadicaPorFiltrosQuery(Paginacao paginacao, int anoLetivo, string dreId, string ueId, string professorRF)
        {
            Paginacao = paginacao;
            AnoLetivo = anoLetivo;
            DreId = dreId;
            UeId = ueId;
            ProfessorRF = professorRF;
        }

        public Paginacao Paginacao { get; internal set; }
        public int AnoLetivo { get; internal set; }
        public string DreId { get; internal set; }
        public string UeId { get; internal set; }
        public string ProfessorRF { get; internal set; }
    }
}
