using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RegistrarEncaminhamentoAEEUseCase : IRegistrarEncaminhamentoAEEUseCase
    {
        private readonly IMediator mediator;

        public RegistrarEncaminhamentoAEEUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<ResultadoEncaminhamentoAeeDto> Executar(EncaminhamentoAeeDto encaminhamentoDto)
        {
            // turma

            // encaminhamento_aee_secao

            // encaminhamento aee
            var resultadoEncaminhamento = await mediator.Send(new RegistrarEncaminhamentoAeeCommand());

            // questao_encaminhamento_aee

            // resposta_encaminhamento_aee

            // TODO: Atualiza a situação da seção no encaminhamento, quando todas as questões obrigatórias forem respondidas

            // TODO: Retornar para o front a situação da seção salva

            return resultadoEncaminhamento;
        }
    }
}


