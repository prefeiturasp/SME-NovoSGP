using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasAtribuicaoCJ : IConsultasAtribuicaoCJ
    {
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;

        public ConsultasAtribuicaoCJ(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
        }

        public async Task<IEnumerable<AtribuicaoCJListaRetornoDto>> Listar(AtribuicaoCJListaFiltroDto filtroDto)
        {
            IEnumerable<AtribuicaoCJ> listaRetorno;
            string[] listaRfs = new string[0];

            if (string.IsNullOrEmpty(filtroDto.UsuarioNome))
            {
                listaRfs = new string[0];
            }

            if (string.IsNullOrEmpty(filtroDto.UsuarioRf))
            {
                listaRfs = new[] { filtroDto.UsuarioRf };
            }

            listaRetorno = await repositorioAtribuicaoCJ.ObterPorFiltros(null, null, filtroDto.UeId, string.Empty, listaRfs);

            return TransformaEntidadesEmDtosListaRetorno(listaRetorno);
        }

        private AtribuicaoCJListaRetornoDto TransformaEntidadeEmDto(AtribuicaoCJ dto)
        {
            var disciplinas = new List<AtribuicaoCJDisciplinaRetornoDto>();
            //var nomeDisciplinas = servicoEol.
            //dto.DisciplinaId

            return new AtribuicaoCJListaRetornoDto()
            {
                Modalidade = dto.Modalidade.GetAttribute<DisplayAttribute>().Name,
                Turma = dto.TurmaId,
                Disciplinas = disciplinas
            };
        }

        private IEnumerable<AtribuicaoCJListaRetornoDto> TransformaEntidadesEmDtosListaRetorno(IEnumerable<AtribuicaoCJ> listaRetorno)
        {
            var professoresDisciplinas = listaRetorno.GroupBy(a => new { a.ProfessorRf, a.Modalidade, a.TurmaId, a.UeId }).ToList();

            //var turmas =

            //professoresDisciplinas.ForEach(a =>
            //{
            //    var atribuicao = new AtribuicaoCJListaRetornoDto()
            //    {
            //        Modalidade = a.Key.Modalidade.GetAttribute<DisplayAttribute>().Name,
            //        Turma = a.Key.tu
            //    }
            //});

            foreach (var dto in listaRetorno)
            {
                yield return TransformaEntidadeEmDto(dto);
            }
        }
    }
}