using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoNAAPAParaRelatorioDinamicoQuery : IRequest<RelatorioDinamicoNAAPADto>
    {
        public ObterEncaminhamentoNAAPAParaRelatorioDinamicoQuery(FiltroRelatorioDinamicoNAAPADto filtro)
        {
            Filtro = filtro;
        }

        public FiltroRelatorioDinamicoNAAPADto Filtro {  get; set; }
    }
}
