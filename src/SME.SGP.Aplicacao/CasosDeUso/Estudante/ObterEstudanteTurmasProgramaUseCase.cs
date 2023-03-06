using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterEstudanteTurmasProgramaUseCase : AbstractUseCase, IObterEstudanteTurmasProgramaUseCase
    {
        public ObterEstudanteTurmasProgramaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AlunoTurmaProgramaDto>> Executar(string codigoAluno, int? anoLetivo,bool filtrarSituacaoMatricula)
        {
            var turmasProgramaAlunos = new List<AlunoTurmaProgramaDto>();
            Ue ue = null;
            Turma turma = null;
            IEnumerable<DisciplinaResposta> componentesCurriculares = null;

            var estudantes = await mediator.Send(new ObterTurmasAlunoPorFiltroQuery(codigoAluno, anoLetivo, filtrarSituacaoMatricula, false));
            estudantes = estudantes.Where(w => w.CodigoTipoTurma == (int)TipoTurma.Programa);
                        
            foreach (var estudante in estudantes)
            {
                if (turma == null || !estudante.CodigoTurma.ToString().Equals(turma.CodigoTurma))
                {
                    turma = await mediator.Send(new ObterTurmaPorCodigoQuery(estudante.CodigoTurma.ToString()));
                    if (turma == null) continue;

                    componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorTurmaCodigoQuery(estudante.CodigoTurma.ToString()));
                }

                if (ue == null || !estudante.CodigoEscola.Equals(ue.CodigoUe))
                    ue = await mediator.Send(new ObterUeComDrePorCodigoQuery(estudante.CodigoEscola));

                var local = ue.TipoEscola != TipoEscola.Nenhum ? $"{ue.TipoEscola.ObterNomeCurto()} {ue.Nome}" : $"{ue.Nome}";

                turmasProgramaAlunos.AddRange(componentesCurriculares.Select(componenteCurricular=> new AlunoTurmaProgramaDto()
                {
                    DreUe = $"{ue.Dre.Abreviacao} {local}",
                    Turma = $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome} - { ((TipoTurnoEOL)turma.TipoTurno).Name()}",
                    ComponenteCurricular = componenteCurricular.Nome
                }));
            }

            return turmasProgramaAlunos.DistinctBy(turmaPrograma => new { turmaPrograma.DreUe, turmaPrograma.Turma, turmaPrograma.ComponenteCurricular });
        }
    }
}
