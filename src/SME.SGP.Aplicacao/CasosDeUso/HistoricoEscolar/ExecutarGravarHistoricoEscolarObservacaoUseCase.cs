using MediatR;
using SME.SGP.Aplicacao.Commands.HistoricoEscolar;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Queries.HistoricoEscolarObservacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
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
            var historicoEscolarObservacoes = mensagemRabbit.ObterObjetoMensagem<List<HistoricoEscolarObservacaoDto>>();

            if (historicoEscolarObservacoes != null && historicoEscolarObservacoes.Any())
            {
                foreach (var historicoEscolarObservacaoDto in historicoEscolarObservacoes)
                {
                    var historicoEscolarObservacao = await mediator.Send(new ObterHistoricoEscolarObservacaoPorAlunoQuery(historicoEscolarObservacaoDto.CodigoAluno));

                    if (historicoEscolarObservacao == null)
                    {
                        historicoEscolarObservacao = ObterNovoHistoricoObsevacao(historicoEscolarObservacaoDto);
                    }
                    else
                    {
                        historicoEscolarObservacao = await ObterAlteracaoHistoricoObservacao(historicoEscolarObservacao, historicoEscolarObservacaoDto);
                    }

                    if (historicoEscolarObservacao != null)
                    {
                        await mediator.Send(new SalvarHistoricoEscolarObservacaoCommand(historicoEscolarObservacao));
                    }
                }
            }

            return true;
        }

        private HistoricoEscolarObservacao ObterNovoHistoricoObsevacao(HistoricoEscolarObservacaoDto dto)
        {
            if (!string.IsNullOrEmpty(dto.Observacao))
                return new HistoricoEscolarObservacao(dto.CodigoAluno, dto.Observacao);

            return null;
        }

        private async Task<HistoricoEscolarObservacao> ObterAlteracaoHistoricoObservacao(HistoricoEscolarObservacao objeto, HistoricoEscolarObservacaoDto dto)
        {
            if (string.IsNullOrEmpty(dto.Observacao))
            {
                await mediator.Send(new RemoverHistoricoEscolarObservacaoCommad(objeto));

                return null;
            }

            objeto.Alterar(dto.Observacao);

            return objeto;
        }
    }
}
