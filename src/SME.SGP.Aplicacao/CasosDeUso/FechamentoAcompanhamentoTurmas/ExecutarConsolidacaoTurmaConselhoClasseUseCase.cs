using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoTurmaConselhoClasseUseCase : AbstractUseCase, IExecutarConsolidacaoTurmaConselhoClasseUseCase
    {
        public ExecutarConsolidacaoTurmaConselhoClasseUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var consolidacaoTurmaConselhoClasse = mensagemRabbit.ObterObjetoMensagem<ConsolidacaoTurmaDto>();

            if (consolidacaoTurmaConselhoClasse == null)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível iniciar a consolidação do conselho de clase da turma. O id da turma e o bimestre não foram informados", LogNivel.Critico, LogContexto.ConselhoClasse));                
                return false;
            }

            if (consolidacaoTurmaConselhoClasse.TurmaId == 0)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível iniciar a consolidação do conselho de clase da turma. O id da turma não foi informado", LogNivel.Critico, LogContexto.ConselhoClasse));                
                return false;
            }

            var turma = await mediator
                .Send(new ObterTurmaPorIdQuery(consolidacaoTurmaConselhoClasse.TurmaId));

            if (turma == null)
                throw new NegocioException("Turma não encontrada");
                        
            var alunos = await mediator
                .Send(new ObterAlunosPorTurmaQuery(turma.CodigoTurma));

            if (alunos == null || !alunos.Any())
                throw new NegocioException($"Não foram encontrados alunos para a turma {turma.CodigoTurma} no Eol");

            var tipoCalendarioId = await mediator
                .Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(turma.ModalidadeCodigo, turma.AnoLetivo, turma.Semestre));

            if (tipoCalendarioId == 0)
                throw new NegocioException("Não foi possível obter o tipo calendario.");

            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));

            if (periodosEscolares == null)
                throw new NegocioException("Não foi possivel obter o período escolar.");

            foreach (var aluno in alunos)
            {
                var ultimoBimestreAtivo = aluno.Inativo ?
                    periodosEscolares.FirstOrDefault(p => p.PeriodoInicio.Date <= aluno.DataSituacao && p.PeriodoFim.Date >= aluno.DataSituacao)?.Bimestre : 4;

                if (aluno.Inativo && consolidacaoTurmaConselhoClasse.Bimestre > ultimoBimestreAtivo)
                    continue;

                var matriculasAlunoTurma = await mediator.Send(new ObterMatriculasAlunoNaTurmaQuery(turma.CodigoTurma, aluno.CodigoAluno));

                var primeiroRegistroMatriculaAtiva = matriculasAlunoTurma
                    .Where(mat => mat.PossuiSituacaoAtiva())
                    .OrderBy(mat => mat.DataSituacao)
                    .FirstOrDefault();

                var dataSituacao = primeiroRegistroMatriculaAtiva != null && !primeiroRegistroMatriculaAtiva.DataSituacao.Equals(DateTime.MinValue) ? primeiroRegistroMatriculaAtiva.DataSituacao : aluno.DataSituacao;

                var matriculadoDepois = !aluno.Inativo ?
                    periodosEscolares.FirstOrDefault(p => dataSituacao > p.PeriodoFim.Date)?.Bimestre : null;

                if (!aluno.Inativo && matriculadoDepois != null && consolidacaoTurmaConselhoClasse.Bimestre > 0 && consolidacaoTurmaConselhoClasse.Bimestre < matriculadoDepois)
                    continue;

                try
                {
                    var mensagemConsolidacaoConselhoClasseAluno = new MensagemConsolidacaoConselhoClasseAlunoDto(aluno.CodigoAluno, consolidacaoTurmaConselhoClasse.TurmaId, consolidacaoTurmaConselhoClasse.Bimestre, aluno.Inativo);

                    var mensagemParaPublicar = JsonConvert.SerializeObject(mensagemConsolidacaoConselhoClasseAluno);

                    var publicarFilaConsolidacaoConselhoClasseAluno = await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarTurmaConselhoClasseAlunoTratar, mensagemParaPublicar, mensagemRabbit.CodigoCorrelacao, null));
                    if (!publicarFilaConsolidacaoConselhoClasseAluno)
                    {
                        var mensagem = $"Não foi possível inserir o aluno de codígo : {aluno.CodigoAluno} na fila de consolidação do conselho de classe.";
                        await mediator.Send(new SalvarLogViaRabbitCommand(mensagem, LogNivel.Critico, LogContexto.ConselhoClasse));                        
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível inserir o aluno de codígo : {aluno.CodigoAluno} na fila de consolidação do conselho de classe.", LogNivel.Critico, LogContexto.ConselhoClasse, ex.Message));
                    return false;
                }
            }
            return true;
        }
    }
}
