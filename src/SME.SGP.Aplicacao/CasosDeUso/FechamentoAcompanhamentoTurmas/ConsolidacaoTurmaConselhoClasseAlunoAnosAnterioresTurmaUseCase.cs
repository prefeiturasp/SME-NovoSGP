using System;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresTurmaUseCase : AbstractUseCase, IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresTurmaUseCase
    {
        private readonly IRepositorioConselhoClasseConsolidadoConsulta repositorioConselhoClasseConsolidadoConsulta;

        public ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresTurmaUseCase(IMediator mediator, IRepositorioConselhoClasseConsolidadoConsulta repositorioConselhoClasseConsolidadoConsulta) : base(mediator)
        {
            this.repositorioConselhoClasseConsolidadoConsulta = repositorioConselhoClasseConsolidadoConsulta ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseConsolidadoConsulta));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<MensagemConsolidarTurmaConselhoClasseAlunoPorTurmaDto>();

            try
            {                
                var alunoNotas = await repositorioConselhoClasseConsolidadoConsulta.ObterFechamentoNotaAlunoOuConselhoClasseAsync(filtro.TurmaId);

                var agrupamentoPorAluno = alunoNotas.GroupBy(g => new { g.AlunoCodigo }, (key, group) =>
                new { key.AlunoCodigo, Result = group.Select(s => s).ToList() });

                foreach (var alunoNota in agrupamentoPorAluno)
                {
                    var parecerConclusivo = RetornarParecerConclusivo(alunoNota.Result);

                    var mensagemPorAluno = new MensagemConsolidarTurmaConselhoClasseAlunoPorAlunoDto(alunoNota.AlunoCodigo, filtro.TurmaId, alunoNota.Result, parecerConclusivo);

                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresAlunoTratar, JsonConvert.SerializeObject(mensagemPorAluno), mensagemRabbit.CodigoCorrelacao, null));
                }
                return true;
            }
            catch (System.Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível executar a consolidacao turma conselho classe aluno anos anteriores por turma.", LogNivel.Critico, LogContexto.ConselhoClasse, ex.Message));
                return false;
            }
        }

        private long? RetornarParecerConclusivo(List<ConsolidacaoConselhoClasseAlunoMigracaoDto> alunoNotas)
        {
            var alunoNota = alunoNotas.FirstOrDefault(f => f.ParecerConclusivoId is not null && f.ParecerConclusivoId > 0);
            return alunoNota?.ParecerConclusivoId;
        }
    }
}
