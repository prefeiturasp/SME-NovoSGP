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
    public class ObterEstudanteLocalAtividadeUseCase : AbstractUseCase, IObterEstudanteLocalAtividadeUseCase
    {
        public ObterEstudanteLocalAtividadeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AlunoLocalAtividadeDto>> Executar(string codigoAluno, int? anoLetivo,bool filtrarSituacaoMatricula)
        {
            var estudantes = await mediator.Send(new ObterTurmasAlunoPorFiltroQuery(codigoAluno, anoLetivo, filtrarSituacaoMatricula, false));

            var alunosLocaisAtividades = new List<AlunoLocalAtividadeDto>();

            Ue ue = null;
            Turma turma = null;
            IEnumerable<DisciplinaResposta> componentesCurriculares = null;
            
            foreach (var estudante in estudantes.Where(w=> w.CodigoTipoTurma == (int)TipoTurma.Programa))
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

                alunosLocaisAtividades.AddRange(componentesCurriculares.Select(componenteCurricular=> new AlunoLocalAtividadeDto()
                {
                    Local = $"{ue.Dre.Abreviacao} {local}",
                    Atividade = $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome} - {componenteCurricular.Nome}"
                }));
            }

            return alunosLocaisAtividades;
        }
    }
}
