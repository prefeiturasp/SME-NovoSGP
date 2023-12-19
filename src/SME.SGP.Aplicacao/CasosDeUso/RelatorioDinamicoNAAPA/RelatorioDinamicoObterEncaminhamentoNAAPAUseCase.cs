using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioDinamicoObterEncaminhamentoNAAPAUseCase : AbstractUseCase, IRelatorioDinamicoObterEncaminhamentoNAAPAUseCase
    {
        private const string OPCAO_TODOS = "-99";

        public RelatorioDinamicoObterEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<RelatorioDinamicoNAAPADto> Executar(FiltroRelatorioDinamicoNAAPADto param)
        {
            if (param.Modalidades.PossuiRegistros(x => x.ToString().Equals(OPCAO_TODOS)))
                param.Modalidades = new Modalidade[] { };
            return mediator.Send(new ObterEncaminhamentoNAAPAParaRelatorioDinamicoQuery(param));
        }
    }
}
