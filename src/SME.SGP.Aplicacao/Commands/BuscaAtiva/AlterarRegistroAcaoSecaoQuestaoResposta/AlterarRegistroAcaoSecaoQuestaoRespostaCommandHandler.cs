using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarRegistroAcaoSecaoQuestaoRespostaCommandHandler : IRequestHandler<AlterarRegistroAcaoSecaoQuestaoRespostaCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRespostaRegistroAcaoBuscaAtiva repositorioResposta;

        public AlterarRegistroAcaoSecaoQuestaoRespostaCommandHandler(IMediator mediator, IRepositorioRespostaRegistroAcaoBuscaAtiva repositorioResposta)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioResposta = repositorioResposta ?? throw new ArgumentNullException(nameof(repositorioResposta));
        }

        public async Task<bool> Handle(AlterarRegistroAcaoSecaoQuestaoRespostaCommand request, CancellationToken cancellationToken)
        {
            var resposta = await MapearParaEntidade(request);
            await repositorioResposta.SalvarAsync(resposta);

            return true;
        }

        private async Task<RespostaRegistroAcaoBuscaAtiva> MapearParaEntidade(AlterarRegistroAcaoSecaoQuestaoRespostaCommand request)
        {
            var resposta = request.RespostaRegistroAcao;

            if (EnumExtension.EhUmDosValores(request.RespostaQuestaoDto.TipoQuestao, new Enum[] { TipoQuestao.Radio, TipoQuestao.Combo, TipoQuestao.Checkbox , TipoQuestao.ComboMultiplaEscolha }))
            {
                resposta.RespostaId = !string.IsNullOrEmpty(request.RespostaQuestaoDto.Resposta) ? long.Parse(request.RespostaQuestaoDto.Resposta) : null;
            }

            if (EnumExtension.EhUmDosValores(request.RespostaQuestaoDto.TipoQuestao, new Enum[] { TipoQuestao.Frase, TipoQuestao.Texto, TipoQuestao.EditorTexto, 
                                                                                                  TipoQuestao.Data, TipoQuestao.Numerico }))
            {
                resposta.Texto = request.RespostaQuestaoDto.Resposta;
            }

            if (!string.IsNullOrEmpty(request.RespostaQuestaoDto.Resposta) && EnumExtension.EhUmDosValores(request.RespostaQuestaoDto.TipoQuestao, new Enum[] { TipoQuestao.Upload }))
            {
                var arquivoCodigo = Guid.Parse(request.RespostaQuestaoDto.Resposta);
                resposta.ArquivoId = await mediator.Send(new ObterArquivoIdPorCodigoQuery(arquivoCodigo));
            }

            return resposta;
        }
        
    }
}
