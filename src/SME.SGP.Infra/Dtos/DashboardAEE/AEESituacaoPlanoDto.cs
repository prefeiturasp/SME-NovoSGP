using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra.Dtos
{
    public class AEESituacaoPlanoDto
    {
        public long Quantidade { get; set; }
        public SituacaoPlanoAEE Situacao { get; set; }
        public string DescricaoSituacao { get => Situacao.Name(); }
    }
}
