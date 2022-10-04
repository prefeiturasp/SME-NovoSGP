using SME.SGP.Dominio;

namespace SME.SGP.TesteIntegracao.RegistroIndividual
{
    public class FiltroRegistroIndividualDto
    {
        public string Perfil { get; set; }
        public Modalidade Modalidade { get; set; }
        public ModalidadeTipoCalendario TipoCalendario { get; set; }
    }
} 