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
    public class TratarPendenciaDiarioBordoPorTurmaUseCase : AbstractUseCase, ITratarPendenciaDiarioBordoPorTurmaUseCase
    {
        public TratarPendenciaDiarioBordoPorTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var turmaId = param.ObterObjetoMensagem<string>();

            var turmasDreUe = await mediator.Send(new ObterTurmasDreUePorCodigosQuery(new string[] { turmaId }));

            var professoresEComponentes = new List<ProfessorEComponenteInfantilDto>();
            Guid perfilProfessorInfantil = Guid.Parse(PerfilUsuario.PROFESSOR_INFANTIL.ObterNome());

            var listaProfessoresDaTurma = await mediator.Send(new ObterProfessoresTitularesDisciplinasEolQuery(turmaId, realizaAgrupamento: false));
            var componentesSgp = await mediator.Send(ObterComponentesCurricularesQuery.Instance);

            if (listaProfessoresDaTurma?.Any(a => !string.IsNullOrEmpty(a.ProfessorRf)) == true)
            {
                foreach (var professorDaTurma in listaProfessoresDaTurma)
                {
                    string[] professoresSeparados = professorDaTurma.ProfessorRf.Split(',');
                    foreach (var professor in professoresSeparados)
                    {
                        var codigoRfProfessor = professor.Trim();
                        if (!string.IsNullOrEmpty(codigoRfProfessor))
                        {
                            professoresEComponentes.AddRange(professorDaTurma.DisciplinasId()
                                .Select(disciplinaId => new ProfessorEComponenteInfantilDto()
                            {
                                CodigoRf = codigoRfProfessor,
                                DisciplinaId = disciplinaId,
                                DescricaoComponenteCurricular = componentesSgp.FirstOrDefault(f => f.Codigo.Equals(disciplinaId.ToString()))?.Descricao,
                            }));
                        }
                    }
                }

                await BuscaPendenciaESalva(turmasDreUe.FirstOrDefault(), professoresEComponentes);
            }

            await BuscaPendenciaParaExcluirEPublica(turmaId);

            return true;
        }

        public async Task BuscaPendenciaESalva(Turma turmaComDreUe, List<ProfessorEComponenteInfantilDto> professoresEComponentes)
        {
            try
            {
                var aulas = await mediator.Send(new ObterPendenciasDiarioBordoQuery(turmaComDreUe.CodigoTurma, professoresEComponentes.Select(d => d.DisciplinaId).ToArray()));

                var professoresParaGerarPendencia = new List<ProfessorEComponenteInfantilDto>();

                var filtroPendenciaDiarioBordoTurmaAula = new FiltroPendenciaDiarioBordoTurmaAulaDto()
                {
                    CodigoTurma = turmaComDreUe.CodigoTurma,
                    TurmaComModalidade = turmaComDreUe.NomeComModalidade(),
                    NomeEscola = turmaComDreUe.ObterEscola(),
                    AulasProfessoresComponentesCurriculares = new List<AulaProfessorComponenteDto>()
                };

                foreach (var aula in aulas)
                {
                    if (aula.ComponenteId > 0)
                    {
                        professoresParaGerarPendencia = professoresEComponentes.Where(w => w.DisciplinaId != aula.ComponenteId).ToList();

                        var componentesComAula = aulas.Where(w => w.Id == aula.Id).Select(s => s.ComponenteId).ToList();

                        professoresParaGerarPendencia = professoresParaGerarPendencia.Where(w => !componentesComAula.Contains(w.DisciplinaId)).ToList();
                    }
                    else
                        professoresParaGerarPendencia = professoresEComponentes;

                    if (professoresParaGerarPendencia.Any())
                    {
                        filtroPendenciaDiarioBordoTurmaAula.AulasProfessoresComponentesCurriculares.AddRange(professoresParaGerarPendencia.Select(s=> new AulaProfessorComponenteDto()
                        {
                            AulaId = aula.Id,
                            PeriodoEscolarId = aula.PeriodoEscolarId,
                            ComponenteCurricularId = s.DisciplinaId,
                            DescricaoComponenteCurricular = s.DescricaoComponenteCurricular,
                            ProfessorRf = s.CodigoRf
                        }));
                    }
                }
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaExecutaPendenciasAulaDiarioBordoTurmaAulaComponente, filtroPendenciaDiarioBordoTurmaAula));
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Gerar pendências de diário de bordo por turma", LogNivel.Critico, LogContexto.Pendencia, ex.Message));
            }
        }

        private async Task BuscaPendenciaParaExcluirEPublica(string turmaId)
        {
            var pendenciasDiarioDeBordoParaExcluir = await mediator.Send(new ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommand(turmaId));

            if (pendenciasDiarioDeBordoParaExcluir.Any())
            {
                var filtro = new FiltroListaAulaIdComponenteCurricularIdDto(pendenciasDiarioDeBordoParaExcluir);
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaExcluirPendenciasDiarioBordo, filtro));
            }
        }
    }
}
