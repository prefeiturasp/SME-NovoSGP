using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class MigrarPlanoAulaCommandHandler : IRequestHandler<MigrarPlanoAulaCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;
        private readonly IRepositorioPlanoAula repositorioPlanoAula;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;
        
        private readonly IRepositorioUeConsulta repositorioUe;

        public MigrarPlanoAulaCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IRepositorioPlanoAula repositorioPlanoAula,
            IConsultasAbrangencia consultasAbrangencia, IRepositorioTurmaConsulta repositorioTurmaConsulta, IRepositorioUeConsulta repositorioUe)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPlanoAula = repositorioPlanoAula ?? throw new ArgumentNullException(nameof(repositorioPlanoAula));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<bool> Handle(MigrarPlanoAulaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var usuario = request.Usuario;
                var planoAulaDto = repositorioPlanoAula.ObterPorId(request.PlanoAulaMigrar.PlanoAulaId);
                var aula = await mediator.Send(new ObterAulaPorIdQuery(planoAulaDto.AulaId));

                await ValidarMigracao(request.PlanoAulaMigrar, usuario.CodigoRf, usuario.EhProfessorCj(), aula.UeId, aula.TurmaId);

                unitOfWork.IniciarTransacao();

                foreach (var planoTurma in request.PlanoAulaMigrar.IdsPlanoTurmasDestino)
                {
                    var disciplinaId = aula.DisciplinaId;
                    if (aula.DisciplinaId.EhIdComponenteCurricularTerritorioSaberAgrupado())
                        disciplinaId = await ObterComponenteCurricularTerritorioAgrupadorOutraTurma(aula.DisciplinaId, planoTurma.TurmaId);

                    AulaConsultaDto aulaConsultaDto = await
                        mediator.Send(new ObterAulaDataTurmaDisciplinaQuery(
                            planoTurma.Data,
                            planoTurma.TurmaId,
                            disciplinaId
                        ));

                    if (aulaConsultaDto.EhNulo())
                        throw new NegocioException($"Não há aula cadastrada para a turma {planoTurma.TurmaId} para a data {planoTurma.Data.ToString("dd/MM/yyyy")} neste componente curricular!");

                    var planoCopia = new PlanoAulaDto()
                    {
                        Id = planoTurma.Sobreescrever ? request.PlanoAulaMigrar.PlanoAulaId : 0,
                        AulaId = aulaConsultaDto.Id,
                        Descricao = planoAulaDto.Descricao,
                        ComponenteCurricularId = Convert.ToInt64(aulaConsultaDto.DisciplinaId),
                        LicaoCasa = request.PlanoAulaMigrar.MigrarLicaoCasa ? planoAulaDto.LicaoCasa : string.Empty,
                        ObjetivosAprendizagemComponente = !usuario.EhProfessorCj() ||
                                                          request.PlanoAulaMigrar.MigrarObjetivos ?
                            (await mediator.Send(new ObterObjetivosComComponentePorPlanoAulaIdQuery(request.PlanoAulaMigrar.PlanoAulaId)))?.ToList() : null,
                        RecuperacaoAula = request.PlanoAulaMigrar.MigrarRecuperacaoAula ?
                            planoAulaDto.RecuperacaoAula : string.Empty
                    };

                    await mediator.Send(new SalvarPlanoAulaCommand(planoCopia));
                }

                unitOfWork.PersistirTransacao();
                return true;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw;
            }
        }
        private async Task<string> ObterComponenteCurricularTerritorioAgrupadorOutraTurma(string codigoComponenteCurricularAgrupadorTurmaOrigem, string codigoTurmaDestino)
        {
            var componenteCurricularTurmaOrigem = await mediator.Send(new ObterComponenteCurricularPorIdQuery(long.Parse(codigoComponenteCurricularAgrupadorTurmaOrigem)));
            var componentesCurricularesTurmaDestino = await mediator.Send(new ObterDisciplinasPorCodigoTurmaQuery(codigoTurmaDestino));
            var componenteCurricularTurmaDestino = componentesCurricularesTurmaDestino.FirstOrDefault(cc => (componenteCurricularTurmaOrigem?.TerritorioSaber ?? false) &&
                                                                                    cc.CodigoComponenteTerritorioSaber == componenteCurricularTurmaOrigem.CodigoComponenteCurricularTerritorioSaber);
            return componenteCurricularTurmaDestino?.CodigoComponenteCurricular.ToString() ?? codigoComponenteCurricularAgrupadorTurmaOrigem;
        }

        private async Task ValidarMigracao(MigrarPlanoAulaDto migrarPlanoAulaDto, string codigoRf, bool ehProfessorCj, string ueId, string turmaCodigo)
        {

            var turmaAula = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));
            Ue ue = repositorioUe.ObterPorId(turmaAula.UeId);
            turmaAula.AdicionarUe(ue);

            var turmasAbrangencia = await mediator.Send(new ObterTurmasRegularesPorUeModalidadePeriodoAnoLetivoQuery(turmaAula.Ue.CodigoUe, turmaAula.ModalidadeCodigo));

            var idsTurmasSelecionadas = migrarPlanoAulaDto.IdsPlanoTurmasDestino.Select(x => x.TurmaId).ToList();

            var turmasSelecionadas = await repositorioTurmaConsulta.ObterPorCodigosAsync(idsTurmasSelecionadas.ToArray());
            if (turmasSelecionadas.Any(t => t.TipoTurma == TipoTurma.Programa))
            {
                var turmasPrograma = await consultasAbrangencia.ObterTurmasPrograma(turmaAula.Ue.CodigoUe, turmaAula.ModalidadeCodigo);
                if (turmasPrograma.NaoEhNulo())
                    turmasAbrangencia = turmasAbrangencia.NaoEhNulo() ? turmasAbrangencia.Concat(turmasPrograma) : turmasPrograma;
            }

            var usuario = await mediator.Send(new ObterUsuarioPorCodigoRfLoginQuery(codigoRf, string.Empty));

            var turmasAtribuidasAoProfessor = await mediator.Send(new ObterTurmasPorProfessorRfQuery(codigoRf));
            var turmasAtribuidasAoProfessorPorAno = await mediator.Send(new ObterTurmasPorAnoEUsuarioIdQuery(usuario.Id, turmaAula.AnoLetivo));

            if (ehProfessorCj)
            {
                var turmasAtribuidasCJ = turmasAtribuidasAoProfessor.ToList();
                var professoresAbragenciaTurma = await mediator.Send(new ObterProfessoresTurmaAbrangenciaQuery(turmaCodigo));

                if(professoresAbragenciaTurma.Any(p=> p == codigoRf))
                {
                    turmasAtribuidasCJ.Add(new ProfessorTurmaDto()
                    {
                        CodTurma = Convert.ToInt32(turmaAula.CodigoTurma),
                        Ano = turmaAula.Ano
                    });
                }

                turmasAtribuidasAoProfessor = turmasAtribuidasCJ;
            }

            if (turmaAula.AnoLetivo == DateTimeExtension.HorarioBrasilia().Year)
            {
                if(turmasAtribuidasAoProfessorPorAno.Any())      
                    turmasAtribuidasAoProfessor = turmasAtribuidasAoProfessor.Concat(turmasAtribuidasAoProfessorPorAno.Select( a=> new ProfessorTurmaDto()
                    {
                        CodTurma = Convert.ToInt32(a.Codigo),
                        Ano = a.AnoTurma.ToString(),
                        NomeTurma = a.Nome
                    }));
                

                await ValidaTurmasProfessor(ehProfessorCj, ueId,
                                      migrarPlanoAulaDto.DisciplinaId,
                                      codigoRf,
                                      turmasAtribuidasAoProfessor,
                                      turmasAbrangencia,
                                      idsTurmasSelecionadas);

                ValidaTurmasAno(ehProfessorCj, migrarPlanoAulaDto.MigrarObjetivos,
                            turmasAtribuidasAoProfessor, turmasAbrangencia, idsTurmasSelecionadas);
            }
            else
            {
                await ValidaTurmasProfessorPorAno(ehProfessorCj, ueId,
                                      migrarPlanoAulaDto.DisciplinaId,
                                      codigoRf,
                                      turmasAtribuidasAoProfessorPorAno,
                                      turmasAbrangencia,
                                      idsTurmasSelecionadas);
                
                ValidaTurmasAnoHistorico(ehProfessorCj, migrarPlanoAulaDto.MigrarObjetivos,
                            turmasAtribuidasAoProfessorPorAno, turmasAbrangencia, idsTurmasSelecionadas);
            }

            
        }

        private void ValidaTurmasAno(bool ehProfessorCJ, bool migrarObjetivos,
                                     IEnumerable<ProfessorTurmaDto> turmasAtribuidasAoProfessor,
                                     IEnumerable<AbrangenciaTurmaRetorno> turmasAbrangencia,
                                     IEnumerable<string> idsTurmasSelecionadas)
        {
            if (!ehProfessorCJ || migrarObjetivos)
            {
                if (turmasAtribuidasAoProfessor.Any())
                {
                    var turmasAtribuidasSelecionadas = turmasAtribuidasAoProfessor.Where(t => idsTurmasSelecionadas.Contains(t.CodTurma.ToString()));
                    var anoTurma = turmasAtribuidasSelecionadas.First().Ano;
                    if (!turmasAtribuidasSelecionadas.All(x => x.Ano == anoTurma))
                        throw new NegocioException("Somente é possível migrar o plano de aula para turmas dentro do mesmo ano");
                }
                else
                {
                    var turmasAbrangenciaSelecionadas = turmasAbrangencia.Where(t => idsTurmasSelecionadas.Contains(t.Codigo));
                    var anoTurma = turmasAbrangenciaSelecionadas.First().Ano;
                    if (!turmasAbrangenciaSelecionadas.All(x => x.Ano == anoTurma))
                        throw new NegocioException("Somente é possível migrar o plano de aula para turmas dentro do mesmo ano");
                }
            }
        }

        private void ValidaTurmasAnoHistorico(bool ehProfessorCJ, bool migrarObjetivos,
                                     IEnumerable<TurmaNaoHistoricaDto> turmasAtribuidasAoProfessor,
                                     IEnumerable<AbrangenciaTurmaRetorno> turmasAbrangencia,
                                     IEnumerable<string> idsTurmasSelecionadas)
        {
            if (!ehProfessorCJ || migrarObjetivos)
            {
                if (turmasAtribuidasAoProfessor.Any())
                {
                    var turmasAtribuidasSelecionadas = turmasAtribuidasAoProfessor.Where(t => idsTurmasSelecionadas.Contains(t.Codigo.ToString()));
                    var anoTurma = turmasAtribuidasSelecionadas.First().AnoTurma;
                    if (!turmasAtribuidasSelecionadas.All(x => x.AnoTurma == anoTurma))
                        throw new NegocioException("Somente é possível migrar o plano de aula para turmas dentro do mesmo ano");
                }
                else
                {
                    var turmasAbrangenciaSelecionadas = turmasAbrangencia.Where(t => idsTurmasSelecionadas.Contains(t.Codigo));
                    var anoTurma = turmasAbrangenciaSelecionadas.First().Ano;
                    if (!turmasAbrangenciaSelecionadas.All(x => x.Ano == anoTurma))
                        throw new NegocioException("Somente é possível migrar o plano de aula para turmas dentro do mesmo ano");
                }
            }
        }

        private async Task ValidaTurmasProfessor(bool ehProfessorCJ, string ueId, string disciplinaId, string codigoRf,
                                                IEnumerable<ProfessorTurmaDto> turmasAtribuidasAoProfessor,
                                                IEnumerable<AbrangenciaTurmaRetorno> turmasAbrangencia,
                                                IEnumerable<string> idsTurmasSelecionadas)
        {
            var dto = new AtribuicoesPorTurmaEProfessorDto()
            {
                UeId = ueId,
                ComponenteCurricularId = Convert.ToInt64(disciplinaId),
                UsuarioRf = codigoRf
            };
            IEnumerable<AtribuicaoCJ> lstTurmasCJ = await mediator.Send(new ObterAtribuicoesPorTurmaEProfessorQuery(dto));

            if (turmasAtribuidasAoProfessor.Any())
            {
                var idsTurmasProfessor = turmasAtribuidasAoProfessor?.Select(c => c.CodTurma).ToList();
                if (
                        (
                            ehProfessorCJ &&
                            (
                                lstTurmasCJ.EhNulo() ||
                                idsTurmasSelecionadas.Any(c => !lstTurmasCJ.Select(tcj => tcj.TurmaId).Contains(c))
                            )
                        ) ||
                        (
                            idsTurmasProfessor.EhNulo() ||
                            idsTurmasSelecionadas.Any(c => !idsTurmasProfessor.Contains(Convert.ToInt32(c)))
                        )

                   )
                    throw new NegocioException("Somente é possível migrar o plano de aula para turmas atribuidas ao professor");
            }
            else
            {
                var idsTurmasAbrangencia = turmasAbrangencia?.Select(c => c.Codigo).ToList();
                if (
                        (
                            ehProfessorCJ &&
                            (
                                lstTurmasCJ.EhNulo() ||
                                idsTurmasSelecionadas.Any(c => !lstTurmasCJ.Select(tcj => tcj.TurmaId).Contains(c))
                            )
                        ) ||
                        (
                            idsTurmasAbrangencia.EhNulo() ||
                            idsTurmasSelecionadas.Any(c => !idsTurmasAbrangencia.Contains(c))
                        )

                   )
                    throw new NegocioException("Somente é possível migrar o plano de aula para turmas da abrangência do professor");
            }

        }
        private async Task ValidaTurmasProfessorPorAno(bool ehProfessorCJ, string ueId, string disciplinaId, string codigoRf,
                                                IEnumerable<TurmaNaoHistoricaDto> turmasAtribuidasAoProfessor,
                                                IEnumerable<AbrangenciaTurmaRetorno> turmasAbrangencia,
                                                IEnumerable<string> idsTurmasSelecionadas)
        {
            var dto = new AtribuicoesPorTurmaEProfessorDto()
            {
                UeId = ueId,
                ComponenteCurricularId = Convert.ToInt64(disciplinaId),
                UsuarioRf = codigoRf
            };
            IEnumerable<AtribuicaoCJ> lstTurmasCJ = await mediator.Send(new ObterAtribuicoesPorTurmaEProfessorQuery(dto));

            if (turmasAtribuidasAoProfessor.Any())
            {
                var idsTurmasProfessor = turmasAtribuidasAoProfessor?.Select(c => Convert.ToInt32(c.Codigo)).ToList();
                if (
                        (
                            ehProfessorCJ &&
                            (
                                lstTurmasCJ.EhNulo() ||
                                idsTurmasSelecionadas.Any(c => !lstTurmasCJ.Select(tcj => tcj.TurmaId).Contains(c))
                            )
                        ) ||
                        (
                            idsTurmasProfessor.EhNulo() ||
                            idsTurmasSelecionadas.Any(c => !idsTurmasProfessor.Contains(Convert.ToInt32(c)))
                        )

                   )
                    throw new NegocioException("Somente é possível migrar o plano de aula para turmas atribuidas ao professor");
            }
            else
            {
                var idsTurmasAbrangencia = turmasAbrangencia?.Select(c => c.Codigo).ToList();
                if (
                        (
                            ehProfessorCJ &&
                            (
                                lstTurmasCJ.EhNulo() ||
                                idsTurmasSelecionadas.Any(c => !lstTurmasCJ.Select(tcj => tcj.TurmaId).Contains(c))
                            )
                        ) ||
                        (
                            idsTurmasAbrangencia.EhNulo() ||
                            idsTurmasSelecionadas.Any(c => !idsTurmasAbrangencia.Contains(c))
                        )

                   )
                    throw new NegocioException("Somente é possível migrar o plano de aula para turmas da abrangência do professor");
            }

        }
    }
}
