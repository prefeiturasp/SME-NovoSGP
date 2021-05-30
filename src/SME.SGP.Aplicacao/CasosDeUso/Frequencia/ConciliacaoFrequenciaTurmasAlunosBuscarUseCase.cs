using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaTurmasAlunosBuscarUseCase : AbstractUseCase, IConciliacaoFrequenciaTurmasAlunosBuscarUseCase
    {
        public ConciliacaoFrequenciaTurmasAlunosBuscarUseCase(IMediator mediator) : base(mediator)
        {

        }
        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<TurmaComponentesParaCalculoFrequenciaDto>();

            if (filtro.ComponentesCurricularesId != null || filtro.ComponentesCurricularesId.Length > 0)
            {
                var alunosDaTurma = await mediator.Send(new ObterAlunosPorTurmaMapochoQuery(filtro.TurmaCodigo));
                if (alunosDaTurma == null || alunosDaTurma.Length == 0)
                    throw new NegocioException($"Não foi possível obter os alunos da turma {filtro.TurmaCodigo} para cálculo de frequência.");

                foreach (var disciplina in filtro.ComponentesCurricularesId.Distinct())
                {
                    foreach (var periodo in filtro.DataReferencia.Distinct())
                    {
                        await mediator.Send(new IncluirFilaCalcularFrequenciaPorTurmaCommand(alunosDaTurma, periodo, filtro.TurmaCodigo, disciplina));
                    }
                }
            }
            else throw new NegocioException($"A mensagem da turma {filtro.TurmaCodigo} deve possuir Componente Curricular pra cálculo de frequência");

            return true;
        }
    }


}
