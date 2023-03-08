using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.HistoricoEscolar
{
    public class EnviarFilaGravarHistoricoEscolarObservacaoUseCase : AbstractUseCase, IEnviarFilaGravarHistoricoEscolarObservacaoUseCase
    {
        public EnviarFilaGravarHistoricoEscolarObservacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(string codigoAluno, SalvarObservacaoHistoricoEscolarDto salvarObservacaoHistoricoEscolarDto)
        {
            if (string.IsNullOrWhiteSpace(codigoAluno))
                throw new NegocioException("O Código do Aluno deve ser informado.");

            var historicoEscolarObservacao = new HistoricoEscolarObservacaoDto(codigoAluno, salvarObservacaoHistoricoEscolarDto.Observacao);
            return await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ExecutarGravarObservacaoHistorioEscolar, historicoEscolarObservacao, Guid.NewGuid()));
        }
    }
}
