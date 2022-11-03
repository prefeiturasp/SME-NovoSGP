﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaAulasPorTipoCommandHandler : AsyncRequestHandler<SalvarPendenciaAulasPorTipoCommand>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public SalvarPendenciaAulasPorTipoCommandHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected override async Task Handle(SalvarPendenciaAulasPorTipoCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var aulasAgrupadas = request.Aulas.GroupBy(x => new { x.TurmaId, x.DisciplinaId });

                var componentesCurriculares = await mediator.Send(new ObterDescricaoComponentesCurricularesPorIdsQuery(request.Aulas.Select(s => long.Parse(s.DisciplinaId)).Distinct().ToArray()));

                var turmasDreUe = await mediator.Send(new ObterTurmasDreUePorCodigosQuery(request.Aulas.Select(s => s.TurmaId).Distinct().ToArray()));

                foreach (var item in aulasAgrupadas)
                {
                    var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorCalendarioEDataQuery(item.First().TipoCalendarioId, item.First().DataAula));

                    if (periodoEscolar != null)
                    {
                        var turmaComDreUe = turmasDreUe.FirstOrDefault(f => f.CodigoTurma.Equals(item.Key.TurmaId));

                        var componente = componentesCurriculares.FirstOrDefault(f => f.Id == long.Parse(item.Key.DisciplinaId));

                        var descricaoComponenteCurricular = !string.IsNullOrEmpty(componente.DescricaoInfantil) ? componente.DescricaoInfantil : componente.Descricao;

                        var turmaAnoComModalidade = turmaComDreUe.NomeComModalidade();

                        var descricaoUeDre = turmaComDreUe.ObterEscola();

                        if (periodoEscolar != null)
                        {
                            var aulasNormais = item.Where(w => !w.AulaCJ);

                            var aulasCJ = item.Where(w => w.AulaCJ);

                            if (aulasNormais.Any())
                            {
                                var modalidadeTurma = await mediator.Send(new ObterModalidadeTurmaPorCodigoQuery(item.First().TurmaId));

                                if (modalidadeTurma != Modalidade.EducacaoInfantil || request.TipoPendenciaAula != TipoPendencia.Frequencia)
                                {
                                    var professorTitularTurma = await mediator.Send(new ObterProfessorTitularPorTurmaEComponenteCurricularQuery(item.First().TurmaId, item.First().DisciplinaId));

                                    if (professorTitularTurma != null)
                                    {
                                        if (periodoEscolar != null && !string.IsNullOrEmpty(professorTitularTurma.ProfessorRf))
                                            await SalvarPendenciaAulaUsuario(item.First().DisciplinaId, professorTitularTurma.ProfessorRf, periodoEscolar.Id, request.TipoPendenciaAula, aulasNormais.Select(x => x.Id), descricaoComponenteCurricular, turmaAnoComModalidade, descricaoUeDre, turmaComDreUe);
                                    }
                                }
                                else
                                {
                                    var listaProfessoresTitularesDaTurma = await mediator.Send(new ObterProfessoresTitularesDisciplinasEolQuery(item.First().TurmaId));
                                
                                    var professoresTitularesDaTurma =
                                        listaProfessoresTitularesDaTurma?.Select(x => x.ProfessorRf);
                                
                                    if (professoresTitularesDaTurma != null)
                                    {
                                        string[] professoresSeparados = professoresTitularesDaTurma.FirstOrDefault().Split(',');

                                        foreach (var professor in professoresSeparados)
                                        {
                                            string codigoRfProfessor = professor.Trim();

                                            if (!string.IsNullOrEmpty(codigoRfProfessor))
                                                await SalvarPendenciaAulaUsuario(item.First().DisciplinaId, codigoRfProfessor, periodoEscolar.Id, request.TipoPendenciaAula, aulasNormais.Select(x => x.Id), descricaoComponenteCurricular, turmaAnoComModalidade, descricaoUeDre, turmaComDreUe);
                                        }
                                    }
                                }

                            }

                            if (aulasCJ.Any())
                            {
                                var agrupamentoAulasCJ = aulasCJ.Where(w => !string.IsNullOrEmpty(w.ProfessorRf)).GroupBy(x => new { x.ProfessorRf });

                                foreach (var aulaCJ in agrupamentoAulasCJ)
                                    await SalvarPendenciaAulaUsuario(item.First().DisciplinaId, aulaCJ.FirstOrDefault().ProfessorRf, periodoEscolar.Id, request.TipoPendenciaAula, aulaCJ.Select(x => x.Id), descricaoComponenteCurricular, turmaAnoComModalidade, descricaoUeDre, turmaComDreUe);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao Salvar Pendencia Aulas Por Tipo.", LogNivel.Critico, LogContexto.Aula, ex.Message,innerException: ex.InnerException.ToString(),rastreamento:ex.StackTrace), cancellationToken);
                throw;
            }
        }

        private async Task SalvarPendenciaAulaUsuario(
                                string disciplinaId, 
                                string codigoRfProfessor, 
                                long periodoEscolarId, 
                                TipoPendencia tipoPendencia, 
                                IEnumerable<long> aulasIds, 
                                string descricaoComponenteCurricular, 
                                string turmaAnoComModalidade, 
                                string descricaoUeDre, 
                                Turma turma)
        {
            var pendenciaAulaProfessor = await mediator.Send(new ObterPendenciaIdPorComponenteProfessorBimestreQuery(long.Parse(disciplinaId), codigoRfProfessor, periodoEscolarId, tipoPendencia, turma.CodigoTurma, turma.UeId));

            var pendenciaIdExistente = pendenciaAulaProfessor != null && pendenciaAulaProfessor.Any() ? pendenciaAulaProfessor.FirstOrDefault().PendenciaId : 0;

            var aulasParaInserir = aulasIds.Except(pendenciaAulaProfessor.Select(s => s.AulaId));

            try
            {
                if (aulasParaInserir.Any())
                {
                    unitOfWork.IniciarTransacao();

                    var pendenciaId = pendenciaIdExistente > 0
                        ? pendenciaIdExistente
                        : await mediator.Send(MapearPendencia(tipoPendencia, descricaoComponenteCurricular, turmaAnoComModalidade, descricaoUeDre, turma.Id));

                    await mediator.Send(new SalvarPendenciasAulasCommand(pendenciaId, aulasIds));

                    await SalvarPendenciaUsuario(pendenciaId, codigoRfProfessor);

                    unitOfWork.PersistirTransacao();
                }
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao Salvar Pendencia Aulas Por Tipo.",  LogNivel.Critico, LogContexto.Aula, ex.Message,innerException: ex.InnerException.ToString(),rastreamento:ex.StackTrace));
                unitOfWork.Rollback();
            }
        }

        private SalvarPendenciaCommand MapearPendencia(TipoPendencia tipoPendencia, string descricaoComponenteCurricular, string turmaAnoComModalidade, string descricaoUeDre, long turmaId)
        {
            return new SalvarPendenciaCommand
            {
                TipoPendencia = tipoPendencia,
                DescricaoComponenteCurricular = descricaoComponenteCurricular,
                TurmaAnoComModalidade = turmaAnoComModalidade,
                DescricaoUeDre = descricaoUeDre,
                TurmaId = turmaId
            };
        }

        private async Task SalvarPendenciaUsuario(long pendenciaId, string professorRf)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(professorRf));
            await mediator.Send(new SalvarPendenciaUsuarioCommand(pendenciaId, usuarioId));
        }
    }
}
