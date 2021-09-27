using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarNotificacaoAlteracaoLimiteDiasUseCase : AbstractUseCase, IGerarNotificacaoAlteracaoLimiteDias
    {
        public class GerarNotificacaoAlteracaoLimiteDiasParametros
        {
            public int Bimestre { get; set; }
            public Turma TurmaFechamento { get; set; }
            public Usuario UsuarioLogado { get; set; }
            public Ue Ue { get; set; }
            public string AlunosComNotaAlterada { get; set; }
        }

        private readonly IServicoFechamentoTurmaDisciplina servicoFechamentoTurmaDisciplina;

        public GerarNotificacaoAlteracaoLimiteDiasUseCase(IMediator mediator, IServicoFechamentoTurmaDisciplina servicoFechamentoTurmaDisciplina) : base(mediator)
        {
            this.servicoFechamentoTurmaDisciplina = servicoFechamentoTurmaDisciplina;
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var data = param.ObterObjetoMensagem<GerarNotificacaoAlteracaoLimiteDiasParametros>();
            servicoFechamentoTurmaDisciplina.GerarNotificacaoAlteracaoLimiteDias(data.TurmaFechamento, data.UsuarioLogado, data.Ue, data.Bimestre, data.AlunosComNotaAlterada);
            return true;
        }
    }
}
