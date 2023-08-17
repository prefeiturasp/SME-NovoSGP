using System;
using SME.SGP.Dominio;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class FiltroPlanoAee
    {
        public string Perfil { get; set; }
        public Modalidade Modalidade { get; set; }
        public ModalidadeTipoCalendario TipoCalendario { get; set; }
        public bool TurmasMesmaUe { get; set; } 
        public bool TurmaHistorica { get; set; }
    }
} 