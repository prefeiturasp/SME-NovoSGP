using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
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

            var listaProfessoresDaTurma = await mediator.Send(new ObterProfessoresTitularesDisciplinasEolQuery(turmaId));
            var professoresDaTurma = listaProfessoresDaTurma?.Select(x => x.ProfessorRf);
            var componentesSgp = await mediator.Send(new ObterComponentesCurricularesQuery());

            if (professoresDaTurma != null && professoresDaTurma.Any(a => !string.IsNullOrEmpty(a)))
            {
                string[] professoresSeparados = professoresDaTurma.FirstOrDefault().Split(',');

                var componentesDaTurma = new List<long>();

                foreach (var professor in professoresSeparados)
                {
                    var codigoRfProfessor = professor.Trim();
                    if (!string.IsNullOrEmpty(codigoRfProfessor))
                    {
                        var componentesCurricularesEolProfessor = await mediator.Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(turmaId, codigoRfProfessor, perfilProfessorInfantil));

                        professoresEComponentes.AddRange(componentesCurricularesEolProfessor.Select(s => new ProfessorEComponenteInfantilDto()
                        {
                            CodigoRf = codigoRfProfessor,
                            DisciplinaId = s.Codigo,
                            DescricaoComponenteCurricular = componentesSgp.FirstOrDefault(f=> f.Codigo.Equals(s.Codigo.ToString())).Descricao,
                        }));
                    }
                }
                await BuscaPendenciaESalva(turmasDreUe.FirstOrDefault(), professoresEComponentes);
            }

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
    }
}
