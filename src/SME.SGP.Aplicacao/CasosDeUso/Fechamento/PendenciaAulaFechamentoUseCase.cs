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
            var fechamentosTurmaDisciplinaReprocessados = new List<long>();
            try
            {
                var filtro = param.ObterObjetoMensagem<DreUeDto>();
                List<Aula> aulasComPendenciaDiarioClasse = new List<Aula>();
                await PreencherAulasComPendenciaDiarioClasse(filtro, aulasComPendenciaDiarioClasse);

                var agrupamentoTurmaDisciplina = aulasComPendenciaDiarioClasse.GroupBy(aula => new {TurmaCodigo = aula.TurmaId, aula.DisciplinaId, TurmaId = aula.Turma.Id, ModalidadeTipoCalendario = aula.Turma.ModalidadeTipoCalendario});
                var agrupamentoTurma = agrupamentoTurmaDisciplina.GroupBy(turmaDisciplina => turmaDisciplina.Key.TurmaCodigo);
                foreach (var turma in agrupamentoTurma.Where(a => a.Key == "2514097"))
                    await mediator.Send(new ExcluirNotificacaoPendenciasFechamentoCommand(turma.Key, DateTimeExtension.HorarioBrasilia().Year));

                foreach (var turmaDisciplina in agrupamentoTurmaDisciplina.Where(a => a.Key.TurmaCodigo == "2514097"))
                {
                    
                  var periodoEscolarFechamentoEmAberto = (await mediator.Send(new ObterPeriodoEscolarFechamentoEmAbertoQuery(turmaDisciplina.Key.TurmaCodigo, turmaDisciplina.Key.ModalidadeTipoCalendario, DateTimeExtension.HorarioBrasilia().Date)));
                  if (periodoEscolarFechamentoEmAberto.Any())
                      foreach (var periodo in periodoEscolarFechamentoEmAberto)
                        {
                            var id = await GerarPendenciasFechamento(turmaDisciplina.Key.TurmaCodigo, long.Parse(turmaDisciplina.Key.DisciplinaId), periodo);
                            if (id != 0)
                                fechamentosTurmaDisciplinaReprocessados.Add(id);
                        }      
                            

                }

                await RegerarPendenciasFechamento(filtro.UeId, fechamentosTurmaDisciplinaReprocessados.Distinct().ToArray());
                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao publicar mensagem para geração de pendências fechamento de aulas/diários de classe com pendência.", LogNivel.Critico, LogContexto.Pendencia, ex.Message, innerException: ex.InnerException.ToString(), rastreamento: ex.StackTrace));
                throw;
            }
        }

        private async Task RegerarPendenciasFechamento(long idUe, long[]  idsFechamentosTurmaDisciplinaJaReprocessados)
        {
            var situacoesFechamento = new SituacaoFechamento[] { SituacaoFechamento.ProcessadoComPendencias, SituacaoFechamento.ProcessadoComErro };
            var fechamentoTurmaDisciplina = await mediator.Send(new ObterFechamentoTurmaDisciplinaAnoAtualDTOPorUeSituacaoQuery(idUe, situacoesFechamento, idsFechamentosTurmaDisciplinaJaReprocessados));
            foreach (var fechamento in fechamentoTurmaDisciplina)
                await GerarPendenciasFechamento(fechamento);
        }

        private async Task<long> GerarPendenciasFechamento(string turmaCodigo, long disciplinaId, PeriodoEscolar periodoEscolar)
        {
            var situacoesFechamento = new SituacaoFechamento[] { SituacaoFechamento.ProcessadoComSucesso, SituacaoFechamento.ProcessadoComPendencias };
            var fechamentoTurmaDisciplina = await mediator.Send(new ObterFechamentoTurmaDisciplinaDTOQuery(turmaCodigo,
                disciplinaId,
                periodoEscolar.Bimestre,
                situacoesFechamento));

            if (fechamentoTurmaDisciplina.NaoEhNulo())
            {
                var disciplinaEol = await mediator.Send(new ObterComponenteCurricularPorIdQuery(fechamentoTurmaDisciplina.DisciplinaId));

                var disciplina = disciplinaEol ?? throw new NegocioException("Não foi possível localizar o componente curricular no EOL.");

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

                return fechamentoTurmaDisciplina.Id;
            }
            return 0;
        }

        private async Task<long> GerarPendenciasFechamento(FechamentoTurmaDisciplinaPendenciaDto fechamentoTurmaDisciplina)
        {
            if (fechamentoTurmaDisciplina.NaoEhNulo())
            {
                var disciplinasEol = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new[] { fechamentoTurmaDisciplina.DisciplinaId }));

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
                        new Usuario() { Id = fechamentoTurmaDisciplina.UsuarioId },
                        fechamentoTurmaDisciplina.Id,
                        fechamentoTurmaDisciplina.Justificativa,
                        fechamentoTurmaDisciplina.CriadoRF,
                        fechamentoTurmaDisciplina.TurmaId,
                        false,
                        disciplina.RegistraFrequencia);

                return fechamentoTurmaDisciplina.Id;
            }
            return 0;
        }

        private async Task PreencherAulasComPendenciaDiarioClasse(DreUeDto filtro, List<Aula> retorno)
        {
            var aulas = (await mediator.Send(new ObterPendenciasAulasPorTipoQuery(TipoPendencia.PlanoAula,
                "plano_aula",new long[] {(int) Modalidade.Fundamental, (int) Modalidade.EJA, (int) Modalidade.Medio},
                filtro.DreId, filtro.UeId,false)));
            if (aulas.NaoEhNulo())
                retorno.AddRange(aulas);

            aulas = await mediator.Send(new ObterPendenciasAtividadeAvaliativaQuery(filtro.DreId, filtro.UeId,false));
            if (aulas.NaoEhNulo())
                retorno.AddRange(aulas);

            aulas = await mediator.Send(new ObterPendenciasAulasPorTipoQuery(TipoPendencia.Frequencia,
                "registro_frequencia", new long[] {(int) Modalidade.EducacaoInfantil, (int) Modalidade.Fundamental, (int) Modalidade.EJA, (int) Modalidade.Medio},
                filtro.DreId, filtro.UeId,false));
            if (aulas.NaoEhNulo())
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