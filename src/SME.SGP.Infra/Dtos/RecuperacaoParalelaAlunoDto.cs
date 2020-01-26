using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class RecuperacaoParalelaAlunoDto
    {
        private readonly Random rdm = new Random();

        public RecuperacaoParalelaStatus Concluido { get { return (RecuperacaoParalelaStatus)rdm.Next(1, 3); } }
        public long Id { get; set; }
        public string Nome { get; set; }
        public int NumeroChamada { get; set; }
        public List<RespostaDto> Respostas { get; set; }
        public string Turma { get; set; }
        public int TurmaId { get; set; }
    }
}