using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarTurmaDoEncaminhamentoNAAPAUseCase : AbstractUseCase, IAtualizarTurmaDoEncaminhamentoNAAPAUseCase
    {
        public AtualizarTurmaDoEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var situacaoMatriculaAtiva = SituacaoMatriculaAluno.Ativo;
            var encaminhamento = param.ObterObjetoMensagem<AtendimentoNAAPADto>();
            var alunosEol = await mediator.Send(new ObterAlunosEolPorCodigosQuery(long.Parse(encaminhamento.AlunoCodigo), true));
            var alunoTurma = alunosEol?.FirstOrDefault(turma => turma.CodigoTipoTurma == (int)TipoTurma.Regular 
                                                      && turma.AnoLetivo <= DateTimeExtension.HorarioBrasilia().Year
                                                      && turma.DataSituacao.Date <= DateTimeExtension.HorarioBrasilia().Date
                                                      && (int)situacaoMatriculaAtiva == turma.CodigoSituacaoMatricula);

            if (alunoTurma.NaoEhNulo()) 
               await AtualizarTurmaDoEncaminhamento(encaminhamento, alunoTurma);

            return true;
        }

        private async Task AtualizarTurmaDoEncaminhamento(AtendimentoNAAPADto encaminhamento, TurmasDoAlunoDto alunoTurma)
        {
            var turmaId = await mediator.Send(new ObterTurmaIdPorCodigoQuery(alunoTurma.CodigoTurma.ToString()));

            if (turmaId != 0)
            {
                if (turmaId != encaminhamento.TurmaId)
                {
                    await AtualizarEncaminhamento(encaminhamento.Id.GetValueOrDefault(), turmaId);
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarNotificacaoTransferenciaUeDreDoEncaminhamentoNAAPA, encaminhamento, Guid.NewGuid(), null));
                }
                if (alunoTurma.CodigoSituacaoMatricula != (int)(encaminhamento.SituacaoMatriculaAluno ?? 0))
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarNotificacaoAtualizacaoSituacaoAlunoDoEncaminhamentoNAAPA, encaminhamento, Guid.NewGuid(), null));
            }
        }

        private async Task AtualizarEncaminhamento(long encaminhamentoNAAPAId, long turmaId)
        {
            var encaminhamentoNAAPA = await mediator.Send(new ObterEncaminhamentoNAAPAComTurmaPorIdQuery(encaminhamentoNAAPAId));
            if(encaminhamentoNAAPA.NaoEhNulo())
            {
                var turma = await mediator.Send(new ObterTurmaPorIdQuery(turmaId));

                encaminhamentoNAAPA.TurmaId = turmaId;
                encaminhamentoNAAPA.Turma = turma;

                await mediator.Send(new SalvarEncaminhamentoNAAPACommand(encaminhamentoNAAPA));
            }
        }
    }
}
