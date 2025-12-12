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

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.RegistrarNovoEncaminhamentoNAAPASecaoQuestaoResposta
{
    public class RegistrarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommandHandler : IRequestHandler<RegistrarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommand, long>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRespostaNovoEncaminhamentoNAAPA repositorioRespostaNovoEncaminhamentoNAAPA;

        public RegistrarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommandHandler(IMediator mediator, IRepositorioRespostaNovoEncaminhamentoNAAPA repositorioRespostaNovoEncaminhamentoNAAPA)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioRespostaNovoEncaminhamentoNAAPA = repositorioRespostaNovoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioRespostaNovoEncaminhamentoNAAPA));
        }

        public async Task<long> Handle(RegistrarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommand request, CancellationToken cancellationToken)
        {
            var resposta = await MapearParaEntidade(request);
            var id = await repositorioRespostaNovoEncaminhamentoNAAPA.SalvarAsync(resposta);
            return id;
        }

        private async Task<RespostaEncaminhamentoEscolar> MapearParaEntidade(RegistrarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommand request)
        {
            var resposta = new RespostaEncaminhamentoEscolar() { QuestaoEncaminhamentoId = request.QuestaoId };

            if (!String.IsNullOrEmpty(request.Resposta) && EnumExtension.EhUmDosValores(request.TipoQuestao, new Enum[] { TipoQuestao.Radio, TipoQuestao.Combo, TipoQuestao.Checkbox, TipoQuestao.ComboMultiplaEscolha, TipoQuestao.SuspeitaViolenciaNAAPA }))
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