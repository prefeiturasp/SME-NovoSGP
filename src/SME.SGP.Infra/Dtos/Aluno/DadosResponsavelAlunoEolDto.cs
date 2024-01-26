using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class DadosResponsavelAlunoEolDto
    {
        public int Id { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public TipoResponsavel TipoResponsavel { get; set; }
        public string NomeSocialAluno { get; set; }
        public DateTime DataNascimentoAluno { get; set; }
        public DateTime? DataNascimento { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string NomeMae { get; set; }
        public int TipoSigilo { get; set; }
        public string DDDCelular { get; set; }
        public string NumeroCelular { get; set; }
        public string NomeAluno { get; set; }
        public string CodigoAluno { get; set; }
        public string NumeroRG { get; set; }
        public string DigitoRG { get; set; }
        public string UfRG { get; set; }
        public string CPFConfere { get; set; }
        public string TipoTurnoCelular { get; set; }
        public string DDDTelefoneFixo { get; set; }
        public string NumeroTelefoneFixo { get; set; }
        public string TipoTurnoTelefoneFixo { get; set; }
        public string DDDTelefoneComercial { get; set; }
        public string NumeroTelefoneComercial { get; set; }
        public string TipoTurnoTelefoneComercial { get; set; }
        public string AutorizaEnvioSMS { get; set; }
        public DateTime? DataNascimentoMae { get; set; }
    }
}
