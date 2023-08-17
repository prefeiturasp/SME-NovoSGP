using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterValidacaoPercusoRAAUseCase : AbstractUseCase, IObterValidacaoPercusoRAAUseCase
    {
        public ObterValidacaoPercusoRAAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<InconsistenciaPercursoRAADto> Executar(FiltroInconsistenciaPercursoRAADto param)
        {
            var inconsistenciaPercurso = new InconsistenciaPercursoRAADto();

            inconsistenciaPercurso.MensagemInconsistenciaPercursoColetivo = await ObterInconsistenciaPercursoColetivo(param);
            inconsistenciaPercurso.InconsistenciaPercursoIndividual = await ObterInconsistenciaPercursoInconsistencia(param);

            return inconsistenciaPercurso;
        }

        private async Task<string> ObterInconsistenciaPercursoColetivo(FiltroInconsistenciaPercursoRAADto param)
        {
            var percursoColetivo = await mediator.Send(new ObterApanhadoGeralPorTurmaIdESemestreQuery(param.TurmaId, param.Semestre));

            return string.IsNullOrEmpty(percursoColetivo?.ApanhadoGeral) ? MensagemNegocioAcomponhamentoAluno.AUSENCIA_PREENCHIMENTO_PERCUSO_COLETIVO : string.Empty;
        } 

        private async Task<InconsistenciaPercursoIndividualRAADto> ObterInconsistenciaPercursoInconsistencia(FiltroInconsistenciaPercursoRAADto param)
        {
            var alunosComPercurso = (await mediator.Send(new ObterAlunosQueContemPercursoIndividalPreenchidoQuery(param.TurmaId, param.Semestre))).ToList();

            if (alunosComPercurso.Any())
            {
                var inconsistencias = await ObterInconsistenciaDeAlunosSemPercurso(alunosComPercurso, param);

                if (inconsistencias.Any())
                    return new InconsistenciaPercursoIndividualRAADto()
                    {
                        MensagemInsconsistencia = MensagemNegocioAcomponhamentoAluno.CRIANCAS_COM_AUSENCIA_PERCURSO_INDIVIDUAL,
                        AlunosComInconsistenciaPercursoIndividualRAA = inconsistencias
                    };

                return null;
            }

            return new InconsistenciaPercursoIndividualRAADto() { MensagemInsconsistencia = MensagemNegocioAcomponhamentoAluno.NENHUMA_CRIANCAO_POSSUI_PERCURSO_INDIVIDUAL }; 
        }

        private async Task<IEnumerable<AlunosComInconsistenciaPercursoIndividualRAADto>> ObterInconsistenciaDeAlunosSemPercurso(List<AcompanhamentoAluno> acompanhamentoAlunos, FiltroInconsistenciaPercursoRAADto param)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(param.TurmaId));
            var alunos = await mediator.Send(new ObterDadosAlunosFechamentoQuery(turma.CodigoTurma, turma.AnoLetivo, param.Semestre));
            var inconsistencias = new List<AlunosComInconsistenciaPercursoIndividualRAADto>();

            foreach (var aluno in alunos)
            {
                if (!acompanhamentoAlunos.Exists(alunoPercurso => alunoPercurso.AlunoCodigo == aluno.CodigoEOL))
                {
                    inconsistencias.Add(new AlunosComInconsistenciaPercursoIndividualRAADto()
                    {
                        AlunoCodigo = aluno.CodigoEOL,
                        AlunoNome = aluno.Nome,
                        NumeroChamada = aluno.NumeroChamada
                    });
                }
            }

            return inconsistencias;
        }
    }
}
