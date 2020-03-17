﻿using System;

namespace SME.SGP.Dominio
{
    public class Ue
    {
        public string CodigoUe { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public Dre Dre { get; set; }
        public long DreId { get; set; }
        public long Id { get; set; }
        public string Nome { get; set; }
        public TipoEscola TipoEscola { get; set; }

        public void AdicionarDre(Dre dre)
        {
            if (dre != null)
            {
                Dre = dre;
                DreId = dre.Id;
            }
        }
    }
}