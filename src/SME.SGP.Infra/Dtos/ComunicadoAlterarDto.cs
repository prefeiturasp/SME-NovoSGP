using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dto
{
    public class ComunicadoAlterarDto
    {
        public ComunicadoAlterarDto()
        {
            Alunos = new List<string>();
            Turmas = new List<string>();
        }

        [DataRequerida(ErrorMessage = "A data de envio é obrigatória.")]
        public DateTime DataEnvio { get; set; }

        [DataMaiorAtual(ErrorMessage = "A data de expiração deve ser igual ou maior que a data atual.")]
        public DateTime? DataExpiracao { get; set; }

        [Required(ErrorMessage = "É necessário informar a descrição.")]
        [MinLength(5, ErrorMessage = "A descrição deve conter no mínimo 5 caracteres.")]
        public string Descricao { get; set; }
        public long Id { get; set; }

        [Required(ErrorMessage = "É necessário informar o título.")]
        [MinLength(10, ErrorMessage = "O título deve conter no mínimo 10 caracteres.")]
        [MaxLength(50, ErrorMessage = "O título deve conter no máximo 50 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "É necessario informar o ano letivo")]
        [Range(0, int.MaxValue, ErrorMessage = "É necessário informar o ano letivo")]
        public int AnoLetivo { get; set; }

        [Required(ErrorMessage = "É necessário informar o codigo da DRE")]
        public string CodigoDre { get; set; }

        [Required(ErrorMessage = "É necessário informar o codigo da UE")]
        public string CodigoUe { get; set; }

        public IEnumerable<string> Turmas { get; set; }

        public bool AlunosEspecificados { get; set; }

        [Required(ErrorMessage = "A modalidade do comunicado deve ser informada.")]
        [ListaTemElementos(ErrorMessage = "É necessário informar ao menos uma modalidade")]
        public int[] Modalidades { get; set; }

        public int Semestre { get; set; }

        public string SeriesResumidas { get; set; }

        public IEnumerable<string> Alunos { get; set; }
        public long? TipoCalendarioId { get; set; }
        public long? EventoId { get; set; }
    }

}