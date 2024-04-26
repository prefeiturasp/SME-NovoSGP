namespace SME.SGP.Infra
{
    public class AlunoSinalizadoPrioridadeMapeamentoEstudanteDto
    {
        public AlunoSinalizadoPrioridadeMapeamentoEstudanteDto(string codigoAluno, bool possuiMapeamentoEstudante = false)
        {
            CodigoAluno = codigoAluno;
            PossuiMapeamentoEstudante = possuiMapeamentoEstudante;
        }
        public string CodigoAluno { get; set; }
        public bool PossuiMapeamentoEstudante { get; set; }
    }
}
