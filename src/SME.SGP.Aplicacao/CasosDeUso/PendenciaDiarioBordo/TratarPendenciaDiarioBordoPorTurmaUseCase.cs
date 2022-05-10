using MediatR;
using SME.SGP.Dominio;
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

            var professoresDaTurma = await mediator.Send(new ObterProfessoresTitularesDaTurmaQuery(turmaId));

            if (professoresDaTurma != null)
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
                            DescricaoComponenteCurricular = s.Descricao
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

                foreach (var aula in aulas)
                {
                    professoresParaGerarPendencia.AddRange(aula.ComponenteId > 0
                                                           ? professoresEComponentes.Where(w => w.DisciplinaId != aula.ComponenteId)
                                                           : professoresEComponentes);

                    await mediator.Send(new SalvarPendenciaDiarioBordoCommand()
                    {
                        TurmaComModalidade = turmaComDreUe.NomeComModalidade(),
                        DescricaoUeDre = $"{ObterEscola(turmaComDreUe)}",
                        ProfessoresComponentes = professoresEComponentes,
                        Aula = aula
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string ObterEscola(Turma turmaDreUe)
        {
            var ueTipo = turmaDreUe.Ue.TipoEscola;

            var dreAbreviacao = turmaDreUe.Ue.Dre.Abreviacao.Replace("-", "");

            var ueNome = turmaDreUe.Ue.Nome;

            return ueTipo != TipoEscola.Nenhum ? $"{ueTipo.ShortName()} {ueNome} ({dreAbreviacao})" : $"{ueNome} ({dreAbreviacao})";
        }
    }
}
