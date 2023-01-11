﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarEncaminhamentoNAAPASecaoQuestaoRespostaCommandHandler : IRequestHandler<AlterarEncaminhamentoNAAPASecaoQuestaoRespostaCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRespostaEncaminhamentoNAAPA repositorioRespostaEncaminhamentoNAAPA;

        public AlterarEncaminhamentoNAAPASecaoQuestaoRespostaCommandHandler(IMediator mediator, IRepositorioRespostaEncaminhamentoNAAPA repositorioRespostaEncaminhamentoNAAPA)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioRespostaEncaminhamentoNAAPA = repositorioRespostaEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioRespostaEncaminhamentoNAAPA));
        }

        public async Task<bool> Handle(AlterarEncaminhamentoNAAPASecaoQuestaoRespostaCommand request, CancellationToken cancellationToken)
        {
            var resposta = await MapearParaEntidade(request);
            await repositorioRespostaEncaminhamentoNAAPA.SalvarAsync(resposta);

            return true;
        }

        private async Task<RespostaEncaminhamentoNAAPA> MapearParaEntidade(AlterarEncaminhamentoNAAPASecaoQuestaoRespostaCommand request)
        {
            var resposta = request.RespostaEncaminhamento;

            if (EnumExtension.EhUmDosValores(request.RespostaQuestaoDto.TipoQuestao, new Enum[] { TipoQuestao.Radio, TipoQuestao.Combo, TipoQuestao.Checkbox , TipoQuestao.ComboMultiplaEscolha }))
            {
                resposta.RespostaId = !string.IsNullOrEmpty(request.RespostaQuestaoDto.Resposta) ? long.Parse(request.RespostaQuestaoDto.Resposta) : null;
            }

            if (EnumExtension.EhUmDosValores(request.RespostaQuestaoDto.TipoQuestao, new Enum[] { TipoQuestao.Frase, TipoQuestao.Texto, TipoQuestao.EditorTexto, 
                                                                                                  TipoQuestao.Data, TipoQuestao.Numerico, TipoQuestao.Endereco,
                                                                                                  TipoQuestao.ContatoResponsaveis, TipoQuestao.AtividadesContraturno}))
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
