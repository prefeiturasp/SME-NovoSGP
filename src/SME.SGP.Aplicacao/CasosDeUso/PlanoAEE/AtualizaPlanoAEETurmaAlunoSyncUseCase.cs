using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizaPlanoAEETurmaAlunoSyncUseCase : AbstractUseCase, IAtualizaPlanoAEETurmaAlunoSyncUseCase
    {
        public AtualizaPlanoAEETurmaAlunoSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var planos = await mediator.Send(new ObterPlanosComSituacaoDiferenteDeEncerradoQuery(DateTime.Now.Year));

            if (planos.NaoEhNulo() && planos.Any())
                foreach (var plano in planos)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAEE.AtualizarTabelaPlanoAEETurmaAlunoTratar, new SalvarPlanoAEETurmaAlunoCommand(plano.Id, plano.AlunoCodigo)));

            return true;
        }
    }
}
