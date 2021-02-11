using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommandHandler : IRequestHandler<RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommand, long>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRespostaEncaminhamentoAEE repositorioRespostaEncaminhamentoAEE;

        public RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommandHandler(IMediator mediator, IRepositorioRespostaEncaminhamentoAEE repositorioRespostaEncaminhamentoAEE)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioRespostaEncaminhamentoAEE = repositorioRespostaEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioRespostaEncaminhamentoAEE));
        }

        public async Task<long> Handle(RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommand request, CancellationToken cancellationToken)
        {
            var resposta = await MapearParaEntidade(request);
            var id = await repositorioRespostaEncaminhamentoAEE.SalvarAsync(resposta);
            return id;
        }

        private async Task<RespostaEncaminhamentoAEE> MapearParaEntidade(RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommand request)
        {
            var resposta = new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = request.QuestaoId
            };

            if (!String.IsNullOrEmpty(request.Resposta) && EnumExtension.EhUmDosValores(request.TipoQuestao, new Enum[] { TipoQuestao.Radio, TipoQuestao.Combo, TipoQuestao.Checkbox, TipoQuestao.ComboMultiplaEscolha }))
            {
                resposta.RespostaId = long.Parse(request.Resposta);
            }

            if (EnumExtension.EhUmDosValores(request.TipoQuestao, new Enum[] { TipoQuestao.Frase, TipoQuestao.Texto, TipoQuestao.AtendimentoClinico }))
            {
                resposta.Texto = request.Resposta;
            }

            if (!String.IsNullOrEmpty(request.Resposta) && EnumExtension.EhUmDosValores(request.TipoQuestao, new Enum[] { TipoQuestao.Upload }))
            {
                var arquivoCodigo = Guid.Parse(request.Resposta);
                resposta.ArquivoId = await mediator.Send(new ObterArquivoIdPorCodigoQuery(arquivoCodigo));
            }

            return resposta;
        }
    }
}
