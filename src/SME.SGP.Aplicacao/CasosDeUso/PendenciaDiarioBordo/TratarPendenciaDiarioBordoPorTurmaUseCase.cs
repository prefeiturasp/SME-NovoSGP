using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            var turmaComComponente = new TurmaComComponentesInfantilDto();
            var professoresEComponentes = new List<ProfessorEComponenteInfantilDto>();
            Guid perfilProfessorInfantil = Guid.Parse(PerfilUsuario.PROFESSOR_INFANTIL.ObterNome());

            var professoresDaTurma = await mediator.Send(new ObterProfessoresTitularesDaTurmaQuery(turmaId));

            if (professoresDaTurma != null)
            {
                string[] professoresSeparados = professoresDaTurma.FirstOrDefault().Split(',');

                var componentesDaTurma = new List<long>();

                foreach (var professor in professoresSeparados)
                {
                    var professorEComponente = new ProfessorEComponenteInfantilDto();
                    string codigoRfProfessor = professor.Trim();
                    if (!string.IsNullOrEmpty(codigoRfProfessor))
                    {
                        var componenteProfessorEol = await mediator.Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(turmaId, codigoRfProfessor, perfilProfessorInfantil));
                        componentesDaTurma.AddRange(componenteProfessorEol.Select(c => c.Codigo).ToList());

                        professorEComponente.ComponentesCurricularesId = componenteProfessorEol.Select(c => c.Codigo).ToArray();
                        professorEComponente.CodigoRf = codigoRfProfessor;
                        professoresEComponentes.Add(professorEComponente);
                    }
                }

                turmaComComponente.TurmaId = turmaId;
                turmaComComponente.ComponentesId = componentesDaTurma.Distinct().ToArray();

                if (turmaComComponente.TurmaId != null && turmaComComponente.ComponentesId.Length > 0)
                    await BuscaPendenciaESalva(turmaComComponente, professoresEComponentes);
            }

            return true;
        }

        public async Task BuscaPendenciaESalva(TurmaComComponentesInfantilDto turmaEComponente, List<ProfessorEComponenteInfantilDto> professoresEComponentes)
        {
            var aulas = await mediator.Send(new ObterPendenciasDiarioBordoQuery(turmaEComponente.TurmaId, turmaEComponente.ComponentesId.ToArray()));

            if (aulas != null && aulas.Any())
                await RegistraPendencia(aulas, professoresEComponentes);
        }
        private async Task RegistraPendencia(IEnumerable<AulaComComponenteDto> aulas, List<ProfessorEComponenteInfantilDto> professoresEComponentes)
         => await mediator.Send(new SalvarPendenciaDiarioBordoCommand(aulas, professoresEComponentes));
    }
}
