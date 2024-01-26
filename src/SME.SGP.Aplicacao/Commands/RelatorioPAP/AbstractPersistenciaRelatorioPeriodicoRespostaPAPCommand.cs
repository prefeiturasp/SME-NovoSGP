using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public abstract class AbstractPersistenciaRelatorioPeriodicoRespostaPAPCommand
    {
        protected readonly IMediator mediator;
        protected readonly IRepositorioRelatorioPeriodicoPAPResposta repositorio;

        protected AbstractPersistenciaRelatorioPeriodicoRespostaPAPCommand(IMediator mediator, IRepositorioRelatorioPeriodicoPAPResposta repositorio)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        protected async Task<long> ExecutePersistencia(RelatorioPeriodicoPAPResposta relatorioResposta, TipoQuestao tipoQuestao, string resposta)
        {
            var entidade = await MapearParaEntidade(relatorioResposta, tipoQuestao, resposta);

            return await repositorio.SalvarAsync(entidade);
        }

        private async Task<RelatorioPeriodicoPAPResposta> MapearParaEntidade(RelatorioPeriodicoPAPResposta relatorioResposta, TipoQuestao tipoQuestao, string resposta)
        {
            if (!String.IsNullOrEmpty(resposta) && EnumExtension.EhUmDosValores(tipoQuestao, new Enum[] { TipoQuestao.Radio, TipoQuestao.Combo, TipoQuestao.Checkbox, TipoQuestao.ComboMultiplaEscolha }))
                relatorioResposta.RespostaId = long.Parse(resposta);

            if (EnumExtension.EhUmDosValores(tipoQuestao, new Enum[] { TipoQuestao.Frase, TipoQuestao.Texto, TipoQuestao.EditorTexto,
                                                                               TipoQuestao.Data, TipoQuestao.Numerico, TipoQuestao.Endereco,
                                                                               TipoQuestao.ContatoResponsaveis, TipoQuestao.AtividadesContraturno, TipoQuestao.TurmasPrograma }))
                relatorioResposta.Texto = resposta;

            if (!String.IsNullOrEmpty(resposta) && EnumExtension.EhUmDosValores(tipoQuestao, new Enum[] { TipoQuestao.Upload }))
            {
                var arquivoCodigo = Guid.Parse(resposta);
                relatorioResposta.ArquivoId = await mediator.Send(new ObterArquivoIdPorCodigoQuery(arquivoCodigo));
            }

            return relatorioResposta;
        }
    }
}
