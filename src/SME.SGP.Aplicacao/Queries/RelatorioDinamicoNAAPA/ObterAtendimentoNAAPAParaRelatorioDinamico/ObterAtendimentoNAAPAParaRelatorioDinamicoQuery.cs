using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAtendimentoNAAPAParaRelatorioDinamicoQuery : IRequest<RelatorioDinamicoNAAPADto>
    {
        public ObterAtendimentoNAAPAParaRelatorioDinamicoQuery(FiltroRelatorioDinamicoNAAPADto filtro)
        {
            Filtro = filtro;
        }

        public FiltroRelatorioDinamicoNAAPADto Filtro {  get; set; }
    }
}
