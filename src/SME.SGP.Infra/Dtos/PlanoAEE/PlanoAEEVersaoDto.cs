using System;
using System.Collections.Generic;
using System.Text;
﻿using SME.SGP.Dominio;
namespace SME.SGP.Infra
{
    public class PlanoAEEVersaoDto
    {
        public long Id { get; set; }
        public int Numero { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
