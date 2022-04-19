using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using SME.SGP.Api.Controllers;
using Xunit;

namespace SME.SGP.TesteIntegracao.Nota
{
    public class LancarNotaBimestre : TesteBase
    {
        public LancarNotaBimestre(TestFixture testFixture) : base(testFixture)
        {
        }

        [Fact]
        public async Task Lancar_Conceito_Para_Componentrse_Regencia_de_Classe_Eja()
        {
           // var command = ServiceProvider.GetService<IComandosNotasConceitos>();

            //var listaDeNotas = new List<NotaConceitoDto>()
            //{
            // new NotaConceitoDto()
            //     {
            //         AlunoId = "7128291",
            //         AtividadeAvaliativaId = 13143296,
            //         Conceito = 2,
            //         Nota=8
            //     },
            //};
            //var dto = new NotaConceitoListaDto
            //{
            //    DisciplinaId = "1114",
            //    TurmaId = "2366531",
            //    NotasConceitos = listaDeNotas
            //};
            
            var controller = ServiceProvider.GetService<NotasConceitosController>();
            //var retorno = (await controller.Post(dto,command)) as JsonResult;
            var retorno = (await controller.Post2()) as JsonResult;

            retorno.StatusCode.ShouldBe(200);
            var valorRetornadoPelaController = (IEnumerable<PeriodosParaConsultaNotasDto>) retorno.Value;
            valorRetornadoPelaController.ShouldNotBeEmpty();
        }
    }
}
