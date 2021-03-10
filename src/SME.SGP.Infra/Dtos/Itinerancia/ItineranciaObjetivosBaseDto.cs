﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ItineranciaObjetivosBaseDto
    {
        public ItineranciaObjetivosBaseDto(long id, string nome, bool temDescricao, bool permiteVariasUes)
        {
            ItineranciaObjetivoBaseId = id;
            Nome = nome;
            TemDescricao = temDescricao;
            PermiteVariasUes = permiteVariasUes;
        }
        public long ItineranciaObjetivoBaseId { get; set; }
        public string Nome { get; set; }
        public bool TemDescricao { get; set; }
        public bool PermiteVariasUes { get; set; }
    }
}
