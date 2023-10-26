using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterRelatorioSemestralPorIdAlunoPAPQuery : IRequest<RelatorioSemestralPAPAluno>
    {
        public ObterRelatorioSemestralPorIdAlunoPAPQuery(long idAlunoPAP)
        {
            IdAlunoPAP = idAlunoPAP;
        }
        public long IdAlunoPAP { get; set; }
    }
}
