using System;
using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class PendenciaAulaFechamentoUseCase : AbstractUseCase, IPendenciaAulaFechamentoUseCase
    {
        public PendenciaAulaFechamentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var filtro = param.ObterObjetoMensagem<DreUeDto>();
                List<Aula> aulasComPendenciaDiarioClasse = new List<Aula>();
                await PreencherAulasComPendenciaDiarioClasse(filtro, aulasComPendenciaDiarioClasse);

                var agrupamentoTurmaDisciplina = aulasComPendenciaDiarioClasse.GroupBy(aula => new {TurmaCodigo = aula.TurmaId, aula.DisciplinaId, TurmaId = aula.Turma.Id, ModalidadeTipoCalendario = aula.Turma.ModalidadeTipoCalendario});
                foreach (var turmaDisciplina in agrupamentoTurmaDisciplina)
                {
                    
                  var periodoEscolarFechamentoEmAberto = (await mediator.Send(new ObterPeriodoEscolarFechamentoEmAbertoQuery(turmaDisciplina.Key.TurmaCodigo, turmaDisciplina.Key.ModalidadeTipoCalendario, DateTimeExtension.HorarioBrasilia().Date)));
                  if (periodoEscolarFechamentoEmAberto.Any())
                      foreach (var periodo in periodoEscolarFechamentoEmAberto)
                          await GerarPendenciasFechamento(turmaDisciplina.Key.TurmaCodigo, long.Parse(turmaDisciplina.Key.DisciplinaId), periodo);

                }

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao publicar mensagem para geração de pendências fechamento de aulas/diários de classe com pendência.", LogNivel.Critico, LogContexto.Pendencia, ex.Message, innerException: ex.InnerException.ToString(), rastreamento: ex.StackTrace));
                throw;
            }
        }

        private async Task GerarPendenciasFechamento(string turmaCodigo, long disciplinaId, PeriodoEscolar periodoEscolar)
        {
            var fechamentoTurmaDisciplina = await mediator.Send(new ObterFechamentoTurmaDisciplinaDTOQuery(turmaCodigo,
                disciplinaId,
                periodoEscolar.Bimestre));

            if (fechamentoTurmaDisciplina != null &&
                (fechamentoTurmaDisciplina.SituacaoFechamento == SituacaoFechamento.ProcessadoComSucesso ||
                 fechamentoTurmaDisciplina.SituacaoFechamento == SituacaoFechamento.ProcessadoComPendencias))
            {
                var disciplinasEol = await mediator.Send(new ObterDisciplinasPorIdsQuery(new[] {fechamentoTurmaDisciplina.DisciplinaId}));

                var disciplina = disciplinasEol is null
                    ? throw new NegocioException("Não foi possível localizar o componente curricular no EOL.")
                    : disciplinasEol.FirstOrDefault();

                if (fechamentoTurmaDisciplina.TipoTurma != TipoTurma.Programa)
                    await PublicarMsgGeracaoPendenciasFechamento(fechamentoTurmaDisciplina.DisciplinaId,
                        fechamentoTurmaDisciplina.CodigoTurma,
                        fechamentoTurmaDisciplina.NomeTurma,
                        fechamentoTurmaDisciplina.PeriodoInicio,
                        fechamentoTurmaDisciplina.PeriodoFim,
                        fechamentoTurmaDisciplina.Bimestre,
                        new Usuario() {Id = fechamentoTurmaDisciplina.UsuarioId},
                        fechamentoTurmaDisciplina.Id,
                        fechamentoTurmaDisciplina.Justificativa,
                        fechamentoTurmaDisciplina.CriadoRF,
                        fechamentoTurmaDisciplina.TurmaId,
                        false,
                        disciplina.RegistraFrequencia);
            }
        }

        private async Task PreencherAulasComPendenciaDiarioClasse(DreUeDto filtro, List<Aula> retorno)
        {
            var aulas = (await mediator.Send(new ObterPendenciasAulasPorTipoQuery(TipoPendencia.PlanoAula,
                "plano_aula",new long[] {(int) Modalidade.Fundamental, (int) Modalidade.EJA, (int) Modalidade.Medio},
                filtro.DreId, filtro.UeId,false)));
            if (aulas != null)
                retorno.AddRange(aulas);

            aulas = await mediator.Send(new ObterPendenciasAtividadeAvaliativaQuery(filtro.DreId, filtro.UeId,false));
            if (aulas != null)
                retorno.AddRange(aulas);

            aulas = await mediator.Send(new ObterPendenciasAulasPorTipoQuery(TipoPendencia.Frequencia,
                "registro_frequencia", new long[] {(int) Modalidade.EducacaoInfantil, (int) Modalidade.Fundamental, (int) Modalidade.EJA, (int) Modalidade.Medio},
                filtro.DreId, filtro.UeId,false));
            if (aulas != null)
                retorno.AddRange(aulas);
        }

        private async Task PublicarMsgGeracaoPendenciasFechamento(long componenteCurricularId, string turmaCodigo, string turmaNome, DateTime periodoEscolarInicio, DateTime periodoEscolarFim, int bimestre, Usuario usuario, long fechamentoTurmaDisciplinaId, string justificativa, string criadoRF, long turmaId, bool componenteSemNota = false, bool registraFrequencia = true)
        {
            await mediator.Send(new IncluirFilaGeracaoPendenciasFechamentoCommand(
                componenteCurricularId,
                turmaCodigo,
                turmaNome,
                periodoEscolarInicio,
                periodoEscolarFim,
                bimestre,
                usuario,
                fechamentoTurmaDisciplinaId,
                justificativa,
                criadoRF,
                turmaId,
                componenteSemNota,
                registraFrequencia));
        }
    }
}