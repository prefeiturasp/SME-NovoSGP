using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class NotaFrequenciaDto
    {
        public NotaFrequenciaDto(
                                long componenteCurricularCodigo, 
                                FrequenciaAluno frequenciaAluno, 
                                PeriodoEscolar periodoEscolar,
                                Turma turma, 
                                IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno,
                                IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno, 
                                bool componenteLancaNota, 
                                bool visualizaNotas, 
                                string[] codigosTurma, 
                                FrequenciaAluno frequenciaAlunoRegenciaPai)
        {
            ComponenteCurricularCodigo = componenteCurricularCodigo;
            FrequenciaAluno = frequenciaAluno;
            PeriodoEscolar = periodoEscolar;
            Turma = turma;
            NotasConselhoClasseAluno = notasConselhoClasseAluno;
            NotasFechamentoAluno = notasFechamentoAluno;
            ComponenteLancaNota = componenteLancaNota;
            VisualizaNotas = visualizaNotas;
            CodigosTurma = codigosTurma;
            FrequenciaAlunoRegenciaPai = frequenciaAlunoRegenciaPai;
        }

        public long ComponenteCurricularCodigo { get; set; }
        public FrequenciaAluno FrequenciaAluno { get; set; }
        public PeriodoEscolar PeriodoEscolar { get; set; }
        public Turma Turma { get; set; }
        public IEnumerable<NotaConceitoBimestreComponenteDto> NotasConselhoClasseAluno { get; set; }
        public IEnumerable<NotaConceitoBimestreComponenteDto> NotasFechamentoAluno { get; set; }
        public bool ComponenteLancaNota { get; set; }
        public bool VisualizaNotas { get; set; }
        public string[] CodigosTurma { get; set; }
        public FrequenciaAluno FrequenciaAlunoRegenciaPai { get; set; }
    }
}
