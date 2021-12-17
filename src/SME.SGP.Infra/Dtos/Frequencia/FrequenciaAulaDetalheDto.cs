using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Infra
{
    public class FrequenciaAulaDetalheDto
    {
        public FrequenciaAulaDetalheDto(Aula aula,
                                        AlunoPorTurmaResposta aluno,
                                        IEnumerable<RegistroFrequenciaAlunoPorAulaDto> registrosFrequenciaAlunos,
                                        IEnumerable<AnotacaoAlunoAulaDto> anotacoesTurma,
                                        FrequenciaPreDefinidaDto frequenciaPreDefinida)
        {
            DetalheFrequencia = new List<FrequenciaDetalheAulaDto>();

            AulaId = aula.Id;
            Desabilitado = aluno.EstaInativo(aula.DataAula) || aula.EhDataSelecionadaFutura;
            PermiteAnotacao = aluno.EstaAtivo(aula.DataAula);
            PossuiAnotacao = anotacoesTurma.Any(a => a.AulaId == AulaId);

            var registrosFrequenciaAula = registrosFrequenciaAlunos.Where(a => a.AulaId == AulaId);
            CarregarDetalheFrequencia(aula, registrosFrequenciaAula, frequenciaPreDefinida);
            TipoFrequencia = ObterTipoFrequenciaDaAula();
        }

        public long AulaId { get; set; }
        public bool Desabilitado { get; set; }
        public bool PermiteAnotacao { get; set; }
        public bool PossuiAnotacao { get; set; }
        public TipoFrequencia? TipoFrequencia { get; set; }
        public IList<FrequenciaDetalheAulaDto> DetalheFrequencia { get; set; }

        private void CarregarDetalheFrequencia(Aula aula, IEnumerable<RegistroFrequenciaAlunoPorAulaDto> registrosFrequenciaAula, FrequenciaPreDefinidaDto frequenciaPreDefinida)
        {
            for (int numeroAula = 1; numeroAula <= aula.Quantidade; numeroAula++)
            {
                DetalheFrequencia.Add(new FrequenciaDetalheAulaDto()
                {
                    NumeroAula = numeroAula,
                    Tipo = ObterTipoFrequencia(numeroAula, registrosFrequenciaAula, frequenciaPreDefinida)
                });
            }
        }

        private TipoFrequencia ObterTipoFrequencia(int numeroAula, IEnumerable<RegistroFrequenciaAlunoPorAulaDto> registrosFrequenciaAula, FrequenciaPreDefinidaDto frequenciaPreDefinida)
            => registrosFrequenciaAula.FirstOrDefault(a => a.NumeroAula == numeroAula)?.TipoFrequencia ??
                frequenciaPreDefinida.Tipo;

        private TipoFrequencia? ObterTipoFrequenciaDaAula()
        {
            var tiposFrequencia = DetalheFrequencia.GroupBy(a => a.Tipo);

            return tiposFrequencia.Count() == 1 ?
                (TipoFrequencia?)tiposFrequencia.First().Key :
                null;
        }
    }
}
