using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidacaoAcompanhamentoAprendizagemAlunosPorUEUseCase : AbstractUseCase, IConsolidacaoAcompanhamentoAprendizagemAlunosPorUEUseCase
    {
        public ConsolidacaoAcompanhamentoAprendizagemAlunosPorUEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroUEDto>();
            
            int anoAtual = filtro.AnoLetivo > 0 ? filtro.AnoLetivo : DateTimeExtension.HorarioBrasilia().Year;

            var turmas = await ObterTurmasInfantil(filtro.UeCodigo, anoAtual);

            await PublicarMensagemConsolidar(turmas, anoAtual, filtro.UeCodigo);

            return true;
        }

        private async Task<IEnumerable<TurmaDTO>> ObterTurmasInfantil(string ueCodigo, int anoAtual)
        {
            var turmas = await mediator.Send(new ObterTurmasInfantilPorUEQuery(anoAtual, ueCodigo));

            if (turmas.EhNulo() && !turmas.Any())
                throw new NegocioException("Não foi possível localizar turmas para consolidar dados de Média de Registros Individuais");

            return turmas;
        }

        private async Task PublicarMensagemConsolidar(IEnumerable<TurmaDTO> turmas, int anoLetivo, string ueCodigo)
        {
            var quantidadesAlunosTurmas = await mediator.Send(new ObterQuantidadeAlunosPorTurmaNaUEQuery(ueCodigo));

            foreach (var turma in turmas)
            {
                var quantidadeAlunos = quantidadesAlunosTurmas.FirstOrDefault(c => c.TurmaCodigo == turma.TurmaCodigo);
                var quantidade = quantidadeAlunos?.Quantidade ?? 0;
                try
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarAcompanhamentoAprendizagemAlunoTratar, new FiltroAcompanhamentoAprendizagemAlunoTurmaDTO(turma.TurmaId, anoLetivo, 1, quantidade), Guid.NewGuid(), null));
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarAcompanhamentoAprendizagemAlunoTratar, new FiltroAcompanhamentoAprendizagemAlunoTurmaDTO(turma.TurmaId, anoLetivo, 2, quantidade), Guid.NewGuid(), null));
                }
                catch (Exception ex)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand("Consolidação Acompanhamento Aprendizagem", LogNivel.Critico, LogContexto.Aula, ex.Message));    
                }
            }
        }
    }
}
