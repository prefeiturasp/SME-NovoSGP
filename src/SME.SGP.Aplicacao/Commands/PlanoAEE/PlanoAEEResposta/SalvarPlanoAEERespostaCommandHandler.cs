using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class SalvarPlanoAEERespostaCommandHandler : IRequestHandler<SalvarPlanoAEERespostaCommand, long>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPlanoAEEResposta repositorioPlanoAEEResposta;

        public SalvarPlanoAEERespostaCommandHandler(IMediator mediator, IRepositorioPlanoAEEResposta repositorioPlanoAEEResposta)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPlanoAEEResposta = repositorioPlanoAEEResposta ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEResposta));
        }

        public async Task<long> Handle(SalvarPlanoAEERespostaCommand request, CancellationToken cancellationToken)
        {
            var planoAEEQuestao = await MapearParaEntidade(request);
            return await repositorioPlanoAEEResposta.SalvarAsync(planoAEEQuestao);
        }

        private async Task<PlanoAEEResposta> MapearParaEntidade(SalvarPlanoAEERespostaCommand request)
        {

            var resposta = new PlanoAEEResposta()
            {
                PlanoAEEQuestaoId = request.PlanoAEEQuestaoId
            };

            if (EnumExtension.EhUmDosValores(request.TipoQuestao, new Enum[] { TipoQuestao.Periodo }))
            {
                ConveterRespostaPeriodoEmDatas(request, resposta);

                await ValidarIntervaloDeDatas(resposta, request.PlanoId);
            }

            if (!String.IsNullOrEmpty(request.Resposta) && EnumExtension.EhUmDosValores(request.TipoQuestao, new Enum[] { TipoQuestao.Radio, TipoQuestao.Combo, TipoQuestao.Checkbox, TipoQuestao.ComboMultiplaEscolha }))
            {
                resposta.RespostaId = long.Parse(request.Resposta);
            }

            if (EnumExtension.EhUmDosValores(request.TipoQuestao, new Enum[] { TipoQuestao.Frase, TipoQuestao.Texto, TipoQuestao.AtendimentoClinico, TipoQuestao.FrequenciaEstudanteAEE, TipoQuestao.PeriodoEscolar }))
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

        private static void ConveterRespostaPeriodoEmDatas(SalvarPlanoAEERespostaCommand request, PlanoAEEResposta resposta)
        {
            var respostaRetorno = request.Resposta.Replace("\\", "").Replace("\"", "").Replace("[", "").Replace("]", "");
            string[] periodos = respostaRetorno.ToString().Split(',');
            resposta.PeriodoInicio = DateTime.Parse(periodos[0]).Date;
            resposta.PeriodoFim = DateTime.Parse(periodos[1]).Date;
        }
        private async Task ValidarIntervaloDeDatas(PlanoAEEResposta resposta, long planoId)
        {
            // Data inicial deve ser menor que a data final
            if (resposta.PeriodoInicio.Value > resposta.PeriodoFim.Value)
                throw new NegocioException("Período inicial deve ser menor que o período final");

            if (resposta.PeriodoInicio.Value.Year != DateTime.Now.Year || resposta.PeriodoFim.Value.Year != DateTime.Now.Year)
                throw new NegocioException("Não é permitido cadastrar plano AEE para outro Ano Letivo!");

            // Data inicial deve ser menor que a data final
            if (UtilData.ObterDiferencaDeMesesEntreDatas(resposta.PeriodoInicio.Value, resposta.PeriodoFim.Value) > 3)
                throw new NegocioException("Não é permitido cadastrar plano AEE com intervalo do período maior que 3 meses!");
        }
    }
}
