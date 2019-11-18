using System;

namespace SME.SGP.Aplicacao.Integracoes.Respostas
{
    public class AlunoPorTurmaResposta
    {
        public string CodigoAluno { get; set; }
        public int CodigoSituacaoMatricula { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataSituacao { get; set; }
        public string NomeAluno { get; set; }
        public string NomeSocialAluno { get; set; }
        public int NumeroAlunoChamada { get; set; }
        public bool PossuiDeficiencia { get; set; }
        public string SituacaoMatricula { get; set; }
    }
}