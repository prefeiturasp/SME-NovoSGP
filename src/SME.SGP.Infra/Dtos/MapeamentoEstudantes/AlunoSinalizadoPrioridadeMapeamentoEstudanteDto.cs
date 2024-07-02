namespace SME.SGP.Infra
{
    public class AlunoSinalizadoPrioridadeMapeamentoEstudanteDto
    {
        public AlunoSinalizadoPrioridadeMapeamentoEstudanteDto(string codigoAluno, bool alertaLaranja = false, bool alertaVermelho = false, bool possuiMapeamentoEstudante = false)
        {
            CodigoAluno = codigoAluno;
            PossuiMapeamentoEstudante = possuiMapeamentoEstudante;
            AlertaLaranja = alertaLaranja;
            AlertaVermelho = alertaVermelho;
        }
        public string CodigoAluno { get; set; }
        public bool PossuiMapeamentoEstudante { get; set; }
        public bool AlertaLaranja { get; set; }
        public bool AlertaVermelho { get; set; }
    }
}
