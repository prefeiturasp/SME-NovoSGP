﻿using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultasAtribuicaoEsporadica : ConsultasBase, IConsultasAtribuicaoEsporadica
    {
        private readonly IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica;
        private readonly IMediator mediator;

        public ConsultasAtribuicaoEsporadica(IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica, IContextoAplicacao contextoAplicacao,IMediator mediator) : base(contextoAplicacao)
        {
            this.repositorioAtribuicaoEsporadica = repositorioAtribuicaoEsporadica ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoEsporadica));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<AtribuicaoEsporadicaDto>> Listar(FiltroAtribuicaoEsporadicaDto filtro)
        {
            var retornoConsultaPaginada = await repositorioAtribuicaoEsporadica.ListarPaginada(Paginacao, filtro.AnoLetivo, filtro.DreId, filtro.UeId, filtro.ProfessorRF);

            var retorno = new PaginacaoResultadoDto<AtribuicaoEsporadicaDto>
            {
                TotalPaginas = retornoConsultaPaginada.TotalPaginas,
                TotalRegistros = retornoConsultaPaginada.TotalRegistros
            };

            bool nenhumItemEncontrado = retornoConsultaPaginada.Items.EhNulo() ||
               !retornoConsultaPaginada.Items.Any() ||
               retornoConsultaPaginada.Items.ElementAt(0).Id == 0;

            retorno.Items = !nenhumItemEncontrado ? await ListaEntidadeParaListaDto(retornoConsultaPaginada.Items) : null;

            return retorno;
        }

        public async Task<AtribuicaoEsporadicaCompletaDto> ObterPorId(long id)
        {
            var atribuicaoEsporadica = await repositorioAtribuicaoEsporadica.ObterPorIdAsync(id);

            if (atribuicaoEsporadica is null)
                return null;

            return await EntidadeParaDtoCompleto(atribuicaoEsporadica);
        }

        private async Task<AtribuicaoEsporadicaDto> EntidadeParaDto(AtribuicaoEsporadica entidade, bool buscarNome = true, string nomeProfessor = "")
        {
            if (buscarNome)
            {
                var professorResumo = await mediator.Send(new ObterResumoProfessorPorRFAnoLetivoQuery(entidade.ProfessorRf, entidade.DataInicio.Year));
                nomeProfessor = professorResumo.NaoEhNulo() ? professorResumo.Nome : "Professor não encontrado";
            }

            return new AtribuicaoEsporadicaDto
            {
                AnoLetivo = entidade.DataInicio.Year,
                DataFim = entidade.DataFim,
                DataInicio = entidade.DataInicio,
                DreId = entidade.DreId,
                Excluido = entidade.Excluido,
                Id = entidade.Id,
                Migrado = entidade.Migrado,
                ProfessorNome = nomeProfessor,
                ProfessorRf = entidade.ProfessorRf,
                UeId = entidade.UeId
            };
        }

        private async Task<AtribuicaoEsporadicaCompletaDto> EntidadeParaDtoCompleto(AtribuicaoEsporadica entidade)
        {
            var professorResumo = await mediator.Send(new ObterResumoProfessorPorRFAnoLetivoQuery(entidade.ProfessorRf, entidade.DataInicio.Year));

            return new AtribuicaoEsporadicaCompletaDto
            {
                AnoLetivo = entidade.DataInicio.Year,
                DataFim = entidade.DataFim,
                DataInicio = entidade.DataInicio,
                DreId = entidade.DreId,
                Excluido = entidade.Excluido,
                Id = entidade.Id,
                Migrado = entidade.Migrado,
                ProfessorNome = professorResumo.NaoEhNulo() ? professorResumo.Nome : "Professor não encontrado",
                ProfessorRf = entidade.ProfessorRf,
                UeId = entidade.UeId,
                AlteradoEm = entidade.AlteradoEm,
                AlteradoPor = entidade.AlteradoPor,
                AlteradoRF = entidade.AlteradoRF,
                CriadoEm = entidade.CriadoEm,
                CriadoPor = entidade.CriadoPor,
                CriadoRF = entidade.CriadoRF
            };
        }

        private async Task<IEnumerable<AtribuicaoEsporadicaDto>> ListaEntidadeParaListaDto(IEnumerable<AtribuicaoEsporadica> entidades)
        {
            var professores = await mediator.Send(new ObterFuncionariosPorRFsQuery(entidades.Select(x => x.ProfessorRf)));

            return entidades.Select(async x => await EntidadeParaDto(x, false, ObterNomeProfessor(professores, x.ProfessorRf))).Select(_task => _task.Result);
        }

        private string ObterNomeProfessor(IEnumerable<ProfessorResumoDto> professores, string codigoRF)
        {
            var professor = professores.FirstOrDefault(p => p.CodigoRF == codigoRF);

            if (professor.EhNulo())
                return "Professor não encontrado";

            return professor.Nome;
        }
    }
}