﻿using System;

namespace SME.SGP.Dominio.Entidades
{
    public class NotaConceitoCicloParametro : EntidadeBase
    {
        public bool Ativo { get; set; }
        public long CicloId { get; set; }
        public DateTime FimVigencia { get; set; }
        public DateTime InicioVigencia { get; set; }
        public int PercentualAlerta { get; set; }
        public int QtdMinimaAvalicoes { get; set; }
        public long TipoNotaId { get; set; }
    }
}