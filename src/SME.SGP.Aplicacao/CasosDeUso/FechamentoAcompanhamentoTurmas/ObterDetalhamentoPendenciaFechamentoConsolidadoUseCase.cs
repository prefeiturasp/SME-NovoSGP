using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDetalhamentoPendenciaFechamentoConsolidadoUseCase : AbstractUseCase, IObterDetalhamentoPendenciaFechamentoConsolidadoUseCase
    {
        public ObterDetalhamentoPendenciaFechamentoConsolidadoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<DetalhamentoPendenciaFechamentoRetornoDto> Executar(long param)
        {
            var detalhamentoPendencia = await mediator.Send(new ObterDetalhamentoPendenciaFechamentoConsolidadoQuery(param));

            if (detalhamentoPendencia == null)
                return null;

            var descricao = UtilRegex.RemoverTagsHtml(detalhamentoPendencia.Descricao);

            var justificativa = detalhamentoPendencia.Justificativa == null ?
                "" :
                $" Justificativa : {UtilRegex.RemoverTagsHtml(detalhamentoPendencia.Justificativa)}";

            var descricaoTexto = $"{descricao}{justificativa}";

            var descricaoHtml = detalhamentoPendencia.DescricaoHtml;

            return new DetalhamentoPendenciaFechamentoRetornoDto
            {
                PendenciaId = detalhamentoPendencia.PendenciaId,
                DescricaoHtml = string.IsNullOrEmpty(descricaoHtml) ?
                descricaoTexto :
                descricaoHtml
            };            
        }
    }
}
