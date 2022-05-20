using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TratarPendenciaDiarioBordoPorTurmaAulaComponenteUseCase : AbstractUseCase, ITratarPendenciaDiarioBordoPorTurmaAulaComponenteUseCase
    {
        public TratarPendenciaDiarioBordoPorTurmaAulaComponenteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroPendenciaDiarioBordoTurmaAulaDto>();

            foreach (var item in filtro.ProfessoresComponentes)
            {
                await mediator.Send(new SalvarPendenciaDiarioBordoCommand()
                {
                    ProfessorComponente = item,
                    Aula = filtro.Aula,
                    CodigoTurma = filtro.CodigoTurma,
                    TurmaComModalidade = filtro.TurmaComModalidade,
                    NomeEscola = filtro.NomeEscola
                });
            }
            return true;
        }
    }
}
