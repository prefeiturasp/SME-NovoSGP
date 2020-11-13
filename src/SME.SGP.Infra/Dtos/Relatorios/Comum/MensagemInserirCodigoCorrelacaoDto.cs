using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class MensagemInserirCodigoCorrelacaoDto
    {
        public MensagemInserirCodigoCorrelacaoDto(TipoRelatorio tipoRelatorio)
        {
            TipoRelatorio = tipoRelatorio;
        }

        public TipoRelatorio TipoRelatorio { get; set; }
        public TipoFormatoRelatorio Formato { get; set; }
    }
}
