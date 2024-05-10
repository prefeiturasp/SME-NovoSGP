﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class PeriodoEscolarDto
    {
        [Range(1, 4, ErrorMessage = "É necessário informar o bimestre")]
        public int Bimestre { get; set; }

        public long Id { get; set; }
        public bool Migrado { get; set; }

        [Required(ErrorMessage = "É necessário informar o fim do período escolar")]
        public DateTime PeriodoFim { get; set; }

        [Required(ErrorMessage = "É necessário informar o início do período escolar")]
        public DateTime PeriodoInicio { get; set; }

        public string Descricao
        {
            get
            {
                if (Bimestre > 0)
                    return $"{Bimestre}° Bimestre";
                else
                    return "";
            }
        }

        public IEnumerable<int> MesesDoPeriodo()
        {
            for (int mes = PeriodoInicio.Month; mes <= PeriodoFim.Month; mes++)
                yield return mes;
        }
    }
}