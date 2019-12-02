using SME.SGP.Dominio;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class AtribuicaoCJPersistenciaDto
    {
        public AtribuicaoCJPersistenciaDto()
        {
            Disciplinas = new List<AtribuicaoCJPersistenciaItemDto>();
        }

        public List<AtribuicaoCJPersistenciaItemDto> Disciplinas { get; set; }

        [Required(ErrorMessage = "É necessário informar a Dre.")]
        public string DreId { get; set; }

        [EnumeradoRequirido(ErrorMessage = "É necessário informar a modalidade.")]
        public Modalidade Modalidade { get; set; }

        [Required(ErrorMessage = "É necessário informar a turma.")]
        public string TurmaId { get; set; }

        [Required(ErrorMessage = "É necessário informar a Ue.")]
        public string UeId { get; set; }

        [Required(ErrorMessage = "É necessário informar o professor substituto.")]
        public string UsuarioRf { get; set; }
    }
}