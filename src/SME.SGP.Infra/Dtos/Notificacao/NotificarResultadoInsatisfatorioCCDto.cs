using FluentValidation;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class NotificarResultadoInsatisfatorioCCDto
    {
        public string ComponenteCurricularNome { get; set; }
        public string Justificativa { get; set; }
        public string Professor { get; set; }        
    }
}
