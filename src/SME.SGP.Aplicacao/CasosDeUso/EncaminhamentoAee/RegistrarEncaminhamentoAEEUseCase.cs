using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
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

        public async Task<ResultadoEncaminhamentoAEEDto> Executar(EncaminhamentoAEEDto encaminhamentoAEEDto)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(encaminhamentoAEEDto.TurmaId));
            if (turma == null)
                throw new NegocioException("A turma informada não foi encontrada");

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(encaminhamentoAEEDto.AlunoCodigo, DateTime.Now.Year));
            if (aluno == null)
                throw new NegocioException("O aluno informado não foi encontrado");

            if(encaminhamentoAEEDto.Id > 0)
            {
                var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEPorIdQuery(encaminhamentoAEEDto.Id));
                if (encaminhamentoAEE != null)
                {

                }
            }
            

            var resultadoEncaminhamento = new ResultadoEncaminhamentoAEEDto();

            

            // encaminhamento_aee_secao

            // encaminhamento aee
            //var resultadoEncaminhamento = await mediator.Send(new RegistrarEncaminhamentoAeeCommand());

            // questao_encaminhamento_aee

            // resposta_encaminhamento_aee

            // TODO: Atualiza a situação da seção no encaminhamento, quando todas as questões obrigatórias forem respondidas

            // TODO: Retornar para o front a situação da seção salva

            return resultadoEncaminhamento;
        }
    }
}


