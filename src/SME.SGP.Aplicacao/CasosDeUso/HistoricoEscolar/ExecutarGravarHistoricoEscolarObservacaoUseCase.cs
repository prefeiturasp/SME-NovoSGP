using MediatR;
using SME.SGP.Aplicacao.Commands.HistoricoEscolar;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Queries.HistoricoEscolarObservacao;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarGravarHistoricoEscolarObservacaoUseCase : AbstractUseCase, IExecutarGravarHistoricoEscolarObservacaoUseCase
    {
        public ExecutarGravarHistoricoEscolarObservacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var historicoEscolarObservacaoDto = mensagemRabbit.ObterObjetoMensagem<HistoricoEscolarObservacaoDto>();

            var historicoEscolarObservacao = await mediator.Send(new ObterHistoricoEscolarObservacaoPorAlunoQuery(historicoEscolarObservacaoDto.CodigoAluno));

            var salvar = false;
            if (historicoEscolarObservacao == null)
            {
                historicoEscolarObservacao = new Dominio.HistoricoEscolarObservacao(historicoEscolarObservacaoDto.CodigoAluno, historicoEscolarObservacaoDto.Observacao);
                salvar = true;
            }
            else if (historicoEscolarObservacao.Observacao != historicoEscolarObservacaoDto.Observacao)
            {
                historicoEscolarObservacao.Alterar(historicoEscolarObservacaoDto.Observacao);
                salvar = true;
            }

            if (salvar)
            {
                await mediator.Send(new SalvarHistoricoEscolarObservacaoCommand(historicoEscolarObservacao));
            }

            return true;
        }
    }
}
