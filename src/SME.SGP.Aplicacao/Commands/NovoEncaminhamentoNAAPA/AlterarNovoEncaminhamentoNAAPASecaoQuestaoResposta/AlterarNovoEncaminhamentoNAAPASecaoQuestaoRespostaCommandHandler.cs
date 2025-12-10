using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.AlterarNovoEncaminhamentoNAAPASecaoQuestaoResposta
{
    public class AlterarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommandHandler : IRequestHandler<AlterarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRespostaNovoEncaminhamentoNAAPA repositorioRespostaNovoEncaminhamentoNAAPA;

        public AlterarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommandHandler(IMediator mediator, IRepositorioRespostaNovoEncaminhamentoNAAPA repositorioRespostaNovoEncaminhamentoNAAPA)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioRespostaNovoEncaminhamentoNAAPA = repositorioRespostaNovoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioRespostaNovoEncaminhamentoNAAPA));
        }

        public async Task<bool> Handle(AlterarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommand request, CancellationToken cancellationToken)
        {
            var resposta = await MapearParaEntidade(request);
            await repositorioRespostaNovoEncaminhamentoNAAPA.SalvarAsync(resposta);

            return true;
        }

        private async Task<RespostaEncaminhamentoEscolar> MapearParaEntidade(AlterarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommand request)
        {
            var resposta = request.RespostaEncaminhamento;

            if (EnumExtension.EhUmDosValores(request.RespostaQuestaoDto.TipoQuestao, new Enum[] { TipoQuestao.Radio, TipoQuestao.Combo, TipoQuestao.Checkbox, TipoQuestao.ComboMultiplaEscolha, TipoQuestao.SuspeitaViolenciaNAAPA }))
            {
                resposta.RespostaId = !string.IsNullOrEmpty(request.RespostaQuestaoDto.Resposta) ? long.Parse(request.RespostaQuestaoDto.Resposta) : null;
            }

            if (EnumExtension.EhUmDosValores(request.RespostaQuestaoDto.TipoQuestao, new Enum[] { TipoQuestao.Frase, TipoQuestao.Texto, TipoQuestao.EditorTexto,
                                                                                                  TipoQuestao.Data, TipoQuestao.Numerico, TipoQuestao.Endereco,
                                                                                                  TipoQuestao.ContatoResponsaveis, TipoQuestao.AtividadesContraturno,
                                                                                                  TipoQuestao.TurmasPrograma, TipoQuestao.ProfissionaisEnvolvidos }))
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