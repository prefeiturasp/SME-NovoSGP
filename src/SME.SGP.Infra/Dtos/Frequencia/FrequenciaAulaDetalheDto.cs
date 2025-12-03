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
                                        IEnumerable<CompensacaoAusenciaAlunoAulaSimplificadoDto> compensacaoAusenciaAlunos,
                                        IEnumerable<AnotacaoAlunoAulaDto> anotacoesTurma,
                                        FrequenciaPreDefinidaDto frequenciaPreDefinida,
                                        Dictionary<DateTime, TipoFrequencia?> frequenciaSugeridaPorData = null)
        {
            DetalheFrequencia = new List<FrequenciaDetalheAulaDto>();

            AulaId = aula.Id;
            Desabilitado = !aluno.EstaAtivo(aula.DataAula) || aula.EhDataSelecionadaFutura;
            PossuiAnotacao = anotacoesTurma.Any(a => a.AulaId == AulaId);
            EhReposicao = TipoAula.Reposicao == aula.TipoAula ? true : false;

            var frequenciaSugerida = ObterFrequenciaSugeridaDataAula(aula, frequenciaSugeridaPorData);
            TipoFrequenciaSugerida = frequenciaSugerida?.ShortName();

            var registrosFrequenciaAula = registrosFrequenciaAlunos.Where(a => a.AulaId == AulaId);
            if(registrosFrequenciaAula?.Any() ?? false)
                CarregarDetalheFrequencia(aula, registrosFrequenciaAula, compensacaoAusenciaAlunos, frequenciaPreDefinida, frequenciaSugerida);

            Tipo = ObterTipoFrequenciaDaAula();
        }

        private static TipoFrequencia? ObterFrequenciaSugeridaDataAula(Aula aula, Dictionary<DateTime, TipoFrequencia?> frequenciaSugeridaPorData)
        {
            return frequenciaSugeridaPorData != null && frequenciaSugeridaPorData.TryGetValue(aula.DataAula, out var tipoFrequencia) ?
                                             tipoFrequencia : null;
        }

        public long AulaId { get; set; }
        public bool Desabilitado { get; set; }
        public bool PossuiAnotacao { get; set; }
        private TipoFrequencia? Tipo { get; set; }
        public string TipoFrequencia  { get => TipoFrequenciaSugerida ?? Tipo?.ShortName() ?? ""; }
        public string TipoFrequenciaSugerida  { get; set; }
        public IList<FrequenciaDetalheAulaDto> DetalheFrequencia { get; set; }
        public bool EhReposicao { get; set; }

        private void CarregarDetalheFrequencia(
            Aula aula, 
            IEnumerable<RegistroFrequenciaAlunoPorAulaDto> registrosFrequenciaAula,
            IEnumerable<CompensacaoAusenciaAlunoAulaSimplificadoDto> compensacaoAusenciaAlunos,
            FrequenciaPreDefinidaDto frequenciaPreDefinida,
            TipoFrequencia? frequenciaSugerida)
        {
            for (int numeroAula = 1; numeroAula <= aula.Quantidade; numeroAula++)
            {
                DetalheFrequencia.Add(new FrequenciaDetalheAulaDto()
                {
                    NumeroAula = numeroAula,
                    Tipo = frequenciaSugerida ?? ObterTipoFrequencia(numeroAula, registrosFrequenciaAula, frequenciaPreDefinida),
                    PossuiCompensacao = compensacaoAusenciaAlunos.Any(t => t.AulaId == aula.Id && t.NumeroAula == numeroAula)
                });
            }
        }

        private TipoFrequencia ObterTipoFrequencia(int numeroAula, IEnumerable<RegistroFrequenciaAlunoPorAulaDto> registrosFrequenciaAula, FrequenciaPreDefinidaDto frequenciaPreDefinida)
            => registrosFrequenciaAula.FirstOrDefault(a => a.NumeroAula == numeroAula)?.TipoFrequencia ??
                frequenciaPreDefinida?.Tipo ?? 
                    Dominio.TipoFrequencia.C;

        private TipoFrequencia? ObterTipoFrequenciaDaAula()
        {
            var tiposFrequencia = DetalheFrequencia.GroupBy(a => a.Tipo);

            return tiposFrequencia.Count() == 1 ?
                (TipoFrequencia?)tiposFrequencia.First().Key :
                null;
        }
    }
}
