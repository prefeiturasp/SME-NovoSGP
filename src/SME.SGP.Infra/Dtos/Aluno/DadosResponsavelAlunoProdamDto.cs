using Newtonsoft.Json;

namespace SME.SGP.Infra
{
    public class DadosResponsavelAlunoProdamDto
    {
        public DadosResponsavelAlunoProdamDto(DadosResponsavelAlunoEolDto dadosResponsavel, AtualizarDadosUsuarioDto atualizarDados)
        {
            AutorizaEnvioSMS = dadosResponsavel.AutorizaEnvioSMS;
            CodigoAluno = dadosResponsavel.CodigoAluno;
            CPF = dadosResponsavel.Cpf;
            CPFConfere = dadosResponsavel.CPFConfere;
            DataNascimentoMae = dadosResponsavel.DataNascimentoMae?.ToString("yyyyMMdd");
            DigitoRG = dadosResponsavel.DigitoRG;
            Nome = dadosResponsavel.Nome;
            NomeMae = dadosResponsavel.NomeMae;
            NumeroRG = dadosResponsavel.NumeroRG;
            TipoPessoa = ((int)dadosResponsavel.TipoResponsavel).ToString();
            TipoTurnoCelular = dadosResponsavel.TipoTurnoCelular;
            TipoTurnoTelefoneComercial = dadosResponsavel.TipoTurnoTelefoneComercial;
            TipoTurnoTelefoneFixo = dadosResponsavel.TipoTurnoTelefoneFixo;
            UfRG = dadosResponsavel.UfRG;

            Email = atualizarDados.Email;
            DDDCelular = atualizarDados.DDDCelular;
            DDDTelefoneComercial = atualizarDados.DDDComercial;
            DDDTelefoneFixo = atualizarDados.DDDResidencial;
            NumeroCelular = atualizarDados.NumeroCelular;
            NumeroTelefoneComercial = atualizarDados.NumeroComercial;
            NumeroTelefoneFixo = atualizarDados.NumeroResidencial;
        }

        [JsonProperty("usuario")]
        public string Usuario { get => "webResp"; }

        [JsonProperty("senha")]
        public string Senha { get => "resp"; }

        [JsonProperty("cd_aluno")]
        public string CodigoAluno { get; set; }

        [JsonProperty("tp_pessoa_responsavel")]
        public string TipoPessoa { get; set; }

        [JsonProperty("nm_responsavel")]
        public string Nome { get; set; }

        [JsonProperty("nr_rg_responsavel")]
        public string NumeroRG { get; set; }

        [JsonProperty("cd_digito_rg_responsavel")]
        public string DigitoRG { get; set; }

        [JsonProperty("sg_uf_rg_responsavel")]
        public string UfRG { get; set; }

        [JsonProperty("cd_cpf_responsavel")]
        public string CPF { get; set; }

        [JsonProperty("in_cpf_responsavel_confere")]
        public string CPFConfere { get; set; }

        [JsonProperty("cd_ddd_celular_responsavel")]
        public string DDDCelular { get; set; }

        [JsonProperty("nr_celular_responsavel")]
        public string NumeroCelular { get; set; }

        [JsonProperty("cd_tipo_turno_celular")]
        public string TipoTurnoCelular { get; set; }

        [JsonProperty("cd_ddd_telefone_fixo_responsavel")]
        public string DDDTelefoneFixo { get; set; }

        [JsonProperty("nr_telefone_fixo_responsavel")]
        public string NumeroTelefoneFixo { get; set; }

        [JsonProperty("cd_tipo_turno_fixo")]
        public string TipoTurnoTelefoneFixo { get; set; }

        [JsonProperty("cd_ddd_telefone_comercial_responsavel")]
        public string DDDTelefoneComercial { get; set; }

        [JsonProperty("nr_telefone_comercial_responsavel")]
        public string NumeroTelefoneComercial { get; set; }

        [JsonProperty("cd_tipo_turno_comercial")]
        public string TipoTurnoTelefoneComercial { get; set; }

        [JsonProperty("in_autoriza_envio_sms_responsavel")]
        public string AutorizaEnvioSMS { get; set; }

        [JsonProperty("email_responsavel")]
        public string Email { get; set; }

        [JsonProperty("nm_mae_responsavel")]
        public string NomeMae { get; set; }

        [JsonProperty("dt_nascimento_responsavel")]
        public string DataNascimentoMae { get; set; }
    }
}
