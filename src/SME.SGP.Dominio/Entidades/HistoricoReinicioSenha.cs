﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class HistoricoReinicioSenha: EntidadeBase
    {
        public string UsuarioRf { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
    }
}
