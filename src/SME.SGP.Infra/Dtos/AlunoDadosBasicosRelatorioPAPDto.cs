using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Infra
{
    public class AlunoDadosBasicosRelatorioPAPDto 
    {
        public string Nome { get; set; }
        public int NumeroChamada { get; set; }
        public DateTime DataNascimento { get; set; }
        public string CodigoEOL { get; set; }
        public SituacaoMatriculaAluno SituacaoCodigo { get; set; }
        public string Situacao { get; set; }
        public DateTime DataSituacao { get; set; }
        public double Frequencia { get; set; }
        public MarcadorFrequenciaDto Marcador { get; set; }
        public bool RelatorioPreenchido { get; set; }
        public bool Desabilitado { get => EstaInativo() || TemMarcadorInativo(); }

        public bool TemMarcadorInativo()
            => Marcador != null &&
                (new[] { TipoMarcadorFrequencia.Transferido,
                        TipoMarcadorFrequencia.Remanejado,
                        TipoMarcadorFrequencia.Inativo}).Contains(Marcador.Tipo);

        public bool EstaInativo()
            => !(new[] { SituacaoMatriculaAluno.Ativo,
                        SituacaoMatriculaAluno.Rematriculado,
                        SituacaoMatriculaAluno.PendenteRematricula,
                        SituacaoMatriculaAluno.SemContinuidade
                    }).Contains(SituacaoCodigo);

        public static explicit operator AlunoDadosBasicosRelatorioPAPDto(AlunoDadosBasicosDto aluno)
            => aluno == null ? null :
            new AlunoDadosBasicosRelatorioPAPDto()
            {
                Nome = aluno.Nome,
                NumeroChamada = aluno.NumeroChamada,
                DataNascimento = aluno.DataNascimento,
                CodigoEOL = aluno.CodigoEOL,
                SituacaoCodigo = aluno.SituacaoCodigo,
                Situacao = aluno.Situacao,
                DataSituacao = aluno.DataSituacao,
                Frequencia = aluno.Frequencia,
                Marcador = aluno.Marcador,
            };
    }
}
