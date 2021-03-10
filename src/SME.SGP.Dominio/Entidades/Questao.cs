﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class Questao : EntidadeBase
    {
        public Questao()
        {
            OpcoesRespostas = new List<OpcaoResposta>();
        }

        public Questionario Questionario { get; set; }
        public long QuestionarioId { get; set; }

        public int Ordem { get; set; }
        public string Nome { get; set; }
        public string Observacao { get; set; }
        public bool Obrigatorio { get; set; }
        public TipoQuestao Tipo { get; set; }
        public string Opcionais { get; set; }

        public List<OpcaoResposta> OpcoesRespostas { get; set; }
    }
}
