using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class RecuperacaoParalelaAlunoListagemDto
    {
        private readonly Random rdm = new Random();

        public long CodAluno { get; set; }
        public RecuperacaoParalelaStatus Concluido { get; set; }
        public long Id { get; set; }
        public string Nome { get; set; }
        public int NumeroChamada { get; set; }
        public List<ObjetivoRespostaDto> Respostas { get; set; }
        public string Turma { get; set; }
        public long TurmaId { get; set; }
    }
}