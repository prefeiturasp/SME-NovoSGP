using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommandHandler : IRequestHandler<RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommand, long>
    {
        private readonly IRepositorioRespostaEncaminhamentoAEE repositorioRespostaEncaminhamentoAEE;

        public RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommandHandler(IRepositorioRespostaEncaminhamentoAEE repositorioRespostaEncaminhamentoAEE)
        {
            this.repositorioRespostaEncaminhamentoAEE = repositorioRespostaEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioRespostaEncaminhamentoAEE));
        }

        public async Task<long> Handle(RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommand request, CancellationToken cancellationToken)
        {
            var resposta = MapearParaEntidade(request);
            var id = await repositorioRespostaEncaminhamentoAEE.SalvarAsync(resposta);
            return id;
        }

        private RespostaEncaminhamentoAEE MapearParaEntidade(RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommand request)
        {
            var resposta = new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = request.QuestaoId
            };

            if (!String.IsNullOrEmpty(request.Resposta) && EnumExtension.EhUmDosValores(request.TipoQuestao, new Enum[] { TipoQuestao.Radio, TipoQuestao.Combo, TipoQuestao.Checkbox }))
            {
                resposta.RespostaId = long.Parse(request.Resposta);
            }

            if (EnumExtension.EhUmDosValores(request.TipoQuestao, new Enum[] { TipoQuestao.Frase, TipoQuestao.Texto }))
            {
                resposta.Texto = request.Resposta;
            }

            if (!String.IsNullOrEmpty(request.Resposta) && EnumExtension.EhUmDosValores(request.TipoQuestao, new Enum[] { TipoQuestao.Upload }))
            {
                resposta.ArquivoId = long.Parse(request.Resposta);
            }

            return resposta;
        }
    }
}
