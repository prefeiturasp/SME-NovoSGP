﻿using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Infra
{
    public class AlunoRegistroFrequenciaDto
    {
        public AlunoRegistroFrequenciaDto()
        {
            Aulas = new List<FrequenciaAulaDetalheDto>();
        }

        public string CodigoAluno { get; set; }
        public SituacaoMatriculaAluno CodigoSituacaoMatricula { get; set; }
        public string NomeAluno { get; set; }
        public int NumeroAlunoChamada { get; set; }
        public string SituacaoMatricula { get; set; }
        public bool PermiteAnotacao { get; set; }
        public bool PossuiAnotacao { get; set; }
        public DateTime DataSituacao { get; set; }
        public DateTime DataNascimento { get; set; }
        public MarcadorFrequenciaDto Marcador { get; set; }
        public IndicativoFrequenciaDto IndicativoFrequencia { get; set; }
        public string NomeResponsavel { get; set; }
        public string TipoResponsavel { get; set; }
        public string CelularResponsavel { get; set; }
        public DateTime? DataAtualizacaoContato { get; set; }
        public bool EhAtendidoAEE { get; set; }
        public bool EhMatriculadoTurmaPAP { get; set; }

        public IList<FrequenciaAulaDetalheDto> Aulas { get; set; }

        public void CarregarAulas(
            IEnumerable<Aula> aulas, 
            IEnumerable<RegistroFrequenciaAlunoPorAulaDto> registrosFrequenciaAlunos,
            IEnumerable<CompensacaoAusenciaAlunoAulaSimplificadoDto> compensacaoAusenciaAlunoAula,
            AlunoPorTurmaResposta aluno, 
            IEnumerable<AnotacaoAlunoAulaDto> anotacoesTurma, 
            FrequenciaPreDefinidaDto frequenciaPreDefinida, 
            DateTime PeriodoFim)
        {
            foreach (var aula in aulas.OrderBy(a => a.DataAula))
            {
                Aulas.Add(new FrequenciaAulaDetalheDto(aula, aluno, registrosFrequenciaAlunos, compensacaoAusenciaAlunoAula, anotacoesTurma, frequenciaPreDefinida, PeriodoFim));
            }
        }
    }
}
