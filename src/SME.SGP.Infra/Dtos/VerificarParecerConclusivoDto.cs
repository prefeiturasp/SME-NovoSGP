﻿using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class VerificarParecerConclusivoDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public bool EmAprovacao { get; set; }
    }
}
