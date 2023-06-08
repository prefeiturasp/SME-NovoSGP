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
               return await AtualizarTurmaDoEncaminhamento(plano, alunoTurma);
            
            return false;
        }
        
        private async Task<bool> AtualizarTurmaDoEncaminhamento(PlanoAEETurmaDto plano, TurmasDoAlunoDto alunoTurma)
        {
            var turmaId = await mediator.Send(new ObterTurmaIdPorCodigoQuery(alunoTurma.CodigoTurma.ToString()));
            if (turmaId != 0)
                if (turmaId != plano.TurmaId)
                    return await AtualizarPlano(plano.Id, turmaId);
            return false;

        }

        private async Task<bool> AtualizarPlano(long planoAEEId, long turmaId)
        {
            var planoAEE = await mediator.Send(new ObterPlanoAEEComTurmaPorIdQuery(planoAEEId));
            if (planoAEE.Situacao == SituacaoPlanoAEE.Encerrado || planoAEE.Situacao == SituacaoPlanoAEE.EncerradoAutomaticamente)
                return false;

            planoAEE.TurmaId = turmaId;
            return await mediator.Send(new SalvarPlanoAeeSimplificadoCommand(planoAEE));
        }
    }
}
