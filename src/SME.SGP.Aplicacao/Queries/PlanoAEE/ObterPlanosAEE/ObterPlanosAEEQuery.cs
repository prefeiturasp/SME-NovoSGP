using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAEEQuery : IRequest<PaginacaoResultadoDto<PlanoAEEResumoDto>>
    {
        public ObterPlanosAEEQuery(FiltroPlanosAEEDto filtro)
        {
            DreId = filtro.DreId;
            UeId = filtro.UeId;
            TurmaId = filtro.TurmaId;
            AlunoCodigo = filtro.AlunoCodigo;
            Situacao = filtro.Situacao;
            ResponsavelRf = filtro.ResponsavelRf;
            PaaiReponsavelRf = filtro.PaaiReponsavelRf;
            ExibirEncerrados = filtro.ExibirEncerrados;
            AnoLetivo = filtro.AnoLetivo;
            ConsideraHistorico = filtro.ConsideraHistorico;
        }

        public long DreId { get; }
        public long UeId { get; }
        public long TurmaId { get; }
        public string AlunoCodigo { get; }
        public SituacaoPlanoAEE? Situacao { get; }
        public string ResponsavelRf { get; set; }
        public string PaaiReponsavelRf { get; set; }
        public bool ExibirEncerrados { get; set; }
        public int AnoLetivo { get; set; }
        public bool ConsideraHistorico { get; set; }
    }
}
