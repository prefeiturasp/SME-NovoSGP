using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistrarAtendimentoNAAPASecaoQuestaoRespostaCommandHandler : IRequestHandler<RegistrarAtendimentoNAAPASecaoQuestaoRespostaCommand, long>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRespostaAtendimentoNAAPA repositorioRespostaEncaminhamentoNAAPA;

        public RegistrarAtendimentoNAAPASecaoQuestaoRespostaCommandHandler(IMediator mediator, IRepositorioRespostaAtendimentoNAAPA repositorioRespostaEncaminhamentoNAAPA)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioRespostaEncaminhamentoNAAPA = repositorioRespostaEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioRespostaEncaminhamentoNAAPA));
        }

        public async Task<long> Handle(RegistrarAtendimentoNAAPASecaoQuestaoRespostaCommand request, CancellationToken cancellationToken)
        {
            var resposta = await MapearParaEntidade(request);
            var id = await repositorioRespostaEncaminhamentoNAAPA.SalvarAsync(resposta);
            return id;
        }

        private async Task<RespostaEncaminhamentoNAAPA> MapearParaEntidade(RegistrarAtendimentoNAAPASecaoQuestaoRespostaCommand request)
        {
            var resposta = new RespostaEncaminhamentoNAAPA() { QuestaoEncaminhamentoId = request.QuestaoId };

            if (!String.IsNullOrEmpty(request.Resposta) && EnumExtension.EhUmDosValores(request.TipoQuestao, new Enum[] { TipoQuestao.Radio, TipoQuestao.Combo, TipoQuestao.Checkbox, TipoQuestao.ComboMultiplaEscolha }))
                resposta.RespostaId = long.Parse(request.Resposta);

            if (EnumExtension.EhUmDosValores(request.TipoQuestao, new Enum[] { TipoQuestao.Frase, TipoQuestao.Texto, TipoQuestao.EditorTexto, 
                                                                               TipoQuestao.Data, TipoQuestao.Numerico, TipoQuestao.Endereco,
                                                                               TipoQuestao.ContatoResponsaveis, TipoQuestao.AtividadesContraturno, 
                                                                               TipoQuestao.TurmasPrograma, TipoQuestao.ProfissionaisEnvolvidos }))
                resposta.Texto = request.Resposta;

            if (!String.IsNullOrEmpty(request.Resposta) && EnumExtension.EhUmDosValores(request.TipoQuestao, new Enum[] { TipoQuestao.Upload }))
            {
                var arquivoCodigo = Guid.Parse(request.Resposta);
                resposta.ArquivoId = await mediator.Send(new ObterArquivoIdPorCodigoQuery(arquivoCodigo));
            }

            return resposta;
        }
    }
}
