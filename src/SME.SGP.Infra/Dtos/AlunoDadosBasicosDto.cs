using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AlunoDadosBasicosDto
    {
        public string Nome { get; set; }
        public int NumeroChamada { get; set; }
        public DateTime DataNascimento { get; set; }
        public string CodigoEOL { get; set; }
        public string Situacao { get; set; }
        public DateTime DataSituacao { get; set; }
        public double Frequencia { get; set; }

        public static explicit operator AlunoDadosBasicosDto(AlunoPorTurmaResposta dadosAluno)
            => dadosAluno == null ? null : new AlunoDadosBasicosDto()
            {
                Nome = dadosAluno.NomeAluno,
                NumeroChamada = dadosAluno.NumeroAlunoChamada,
                DataNascimento = dadosAluno.DataNascimento,
                CodigoEOL = dadosAluno.CodigoAluno,
                Situacao = dadosAluno.SituacaoMatricula,
                DataSituacao = dadosAluno.DataSituacao
            };
    }
}
