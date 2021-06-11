using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterInformacoesEscolaresDoAlunoUseCase : AbstractUseCase, IObterInformacoesEscolaresDoAlunoUseCase
    {
        public ObterInformacoesEscolaresDoAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<InformacoesEscolaresAlunoDto> Executar(string codigoAluno, string turmaId)
        {
            var informacoesEscolaresAluno = await mediator.Send(new ObterNecessidadesEspeciaisAlunoQuery(codigoAluno, turmaId));

            return informacoesEscolaresAluno;
        }
    }
}
