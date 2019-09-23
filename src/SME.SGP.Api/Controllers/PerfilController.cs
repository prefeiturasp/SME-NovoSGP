using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/perfis")]
    [ValidaDto]
    public class PerfilController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PerfilDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Get()
        {
            var perfilSelecionado = Guid.NewGuid();
            return Ok(new
            {
                perfilSelecionado,
                Perfis = new List<PerfilDto>
                {
                    new PerfilDto()
                    {
                        Descricao = "Diretor",
                        Id = Guid.NewGuid()
                    },
                    new PerfilDto()
                    {
                        Descricao = "Professor",
                        Id = perfilSelecionado
                    },
                    new PerfilDto()
                    {
                        Descricao = "Coordenador Pedagógico",
                        Id = Guid.NewGuid(),
                        Sigla="CP"
                    },
                    new PerfilDto()
                    {
                        Descricao = "Professor Orientador de Área",
                        Id = Guid.NewGuid(),
                        Sigla="POA"
                    }
                }
            });
        }
    }
}