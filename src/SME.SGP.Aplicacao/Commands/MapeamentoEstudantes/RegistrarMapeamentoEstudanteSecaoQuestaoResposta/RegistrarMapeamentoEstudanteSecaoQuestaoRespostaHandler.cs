using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistrarMapeamentoEstudanteSecaoQuestaoRespostaCommandHandler : IRequestHandler<RegistrarMapeamentoEstudanteSecaoQuestaoRespostaCommand, long>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRespostaMapeamentoEstudante repositorioResposta;

        public RegistrarMapeamentoEstudanteSecaoQuestaoRespostaCommandHandler(IMediator mediator, IRepositorioRespostaMapeamentoEstudante repositorioResposta)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioResposta = repositorioResposta ?? throw new ArgumentNullException(nameof(repositorioResposta));
        }

        public async Task<long> Handle(RegistrarMapeamentoEstudanteSecaoQuestaoRespostaCommand request, CancellationToken cancellationToken)
        {
            var resposta = await MapearParaEntidade(request);
            var id = await repositorioResposta.SalvarAsync(resposta);
            return id;
        }

        private async Task<RespostaMapeamentoEstudante> MapearParaEntidade(RegistrarMapeamentoEstudanteSecaoQuestaoRespostaCommand request)
        {
            var resposta = new RespostaMapeamentoEstudante() { QuestaoMapeamentoEstudanteId = request.QuestaoId };

            if (!String.IsNullOrEmpty(request.Resposta) && EnumExtension.EhUmDosValores(request.TipoQuestao, new Enum[] { TipoQuestao.Radio, TipoQuestao.Combo, 
                                                                                        TipoQuestao.Checkbox, TipoQuestao.ComboMultiplaEscolha }))
                resposta.RespostaId = long.Parse(request.Resposta);

            if (EnumExtension.EhUmDosValores(request.TipoQuestao, new Enum[] { TipoQuestao.Frase, TipoQuestao.Texto, TipoQuestao.EditorTexto, 
                                                                               TipoQuestao.Data, TipoQuestao.Numerico, TipoQuestao.ComboDinamico, TipoQuestao.ComboMultiplaEscolhaDinamico,
                                                                               TipoQuestao.AvaliacoesExternasProvaSP }))
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
