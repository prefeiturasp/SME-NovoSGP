using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarTurmaDoPlanoAEEUseCase : AbstractUseCase, IAtualizarTurmaDoPlanoAEEUseCase
    {
        public AtualizarTurmaDoPlanoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var plano = param.ObterObjetoMensagem<PlanoAEETurmaDto>();
            var alunosEol = await mediator.Send(new ObterAlunosEolPorCodigosQuery(long.Parse(plano.AlunoCodigo), true));
            var alunoTurma = alunosEol.Where(turma => turma.CodigoTipoTurma == (int)TipoTurma.Regular 
                                                      && turma.AnoLetivo <= DateTimeExtension.HorarioBrasilia().Year
                                                      && turma.DataSituacao.Date <= DateTimeExtension.HorarioBrasilia().Date)
                                      .OrderByDescending(turma => turma.AnoLetivo)
                                      .ThenByDescending(turma => turma.DataSituacao)
                                      .FirstOrDefault(); 

            if (alunoTurma != null) 
               await AtualizarTurmaDoEncaminhamento(plano, alunoTurma);
            
            return true;
        }
        
        private async Task AtualizarTurmaDoEncaminhamento(PlanoAEETurmaDto plano, TurmasDoAlunoDto alunoTurma)
        {
            var turmaId = await mediator.Send(new ObterTurmaIdPorCodigoQuery(alunoTurma.CodigoTurma.ToString()));
            if (turmaId != 0)
                if (turmaId != plano.TurmaId)
                    await AtualizarPlano(plano.Id, turmaId);

        }

        private async Task AtualizarPlano(long planoAEEId, long turmaId)
        {
            var planoAEE = await mediator.Send(new ObterPlanoAEEComTurmaPorIdQuery(planoAEEId));
            planoAEE.TurmaId = turmaId;
            await mediator.Send(new SalvarPlanoAeeSimplificadoCommand(planoAEE));
        }
    }
}
