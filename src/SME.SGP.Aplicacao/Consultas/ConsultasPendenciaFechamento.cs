using MediatR;
using Minio.DataModel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasPendenciaFechamento : ConsultasBase, IConsultasPendenciaFechamento
    {
        private readonly IRepositorioPendenciaFechamento repositorioPendenciaFechamento;
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        private readonly IMediator mediator;

        public ConsultasPendenciaFechamento(IContextoAplicacao contextoAplicacao
                                , IRepositorioPendenciaFechamento repositorioPendenciaFechamento,
                        IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular,
                                                        IMediator mediator) : base(contextoAplicacao)
        {
            this.repositorioPendenciaFechamento = repositorioPendenciaFechamento ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamento));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<PendenciaFechamentoResumoDto>> Listar(FiltroPendenciasFechamentosDto filtro)
        {
            var retornoConsultaPaginada = await repositorioPendenciaFechamento.ListarPaginada(Paginacao, filtro.TurmaCodigo, filtro.Bimestre, filtro.ComponenteCurricularId);

            if (retornoConsultaPaginada.Items != null && retornoConsultaPaginada.Items.Any())
            {
                // Atualiza nome da situacao
                retornoConsultaPaginada.Items.ToList()
                    .ForEach(i => i.SituacaoNome = Enum.GetName(typeof(SituacaoPendencia), i.Situacao));

                // Carrega nomes das disciplinas para o DTO de retorno
                var disciplinasEOL = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(retornoConsultaPaginada.Items.Select(a => a.DisciplinaId).Distinct().ToArray()));
                var componentesTurma = await mediator.Send(new ObterDisciplinasPorCodigoTurmaQuery(filtro.TurmaCodigo));

                foreach(var disciplinaEOL in disciplinasEOL)
                {
                    if (disciplinaEOL.TerritorioSaber)
                    {
                        disciplinaEOL.Nome = componentesTurma?.Where(b => b.TerritorioSaber == true && disciplinaEOL.CodigoComponenteCurricular == b.CodigoComponenteTerritorioSaber)
                                                             .Select(b => b.Nome).FirstOrDefault();                     
                    }

                    retornoConsultaPaginada.Items.Where(c => c.DisciplinaId == disciplinaEOL.CodigoComponenteCurricular).ToList()
                        .ForEach(d => d.ComponenteCurricular = disciplinaEOL.Nome);
                }
            }

            return retornoConsultaPaginada;
        }

        public async Task<PendenciaFechamentoCompletoDto> ObterPorPendenciaId(long pendenciaId)
        {
            var pendencia = await repositorioPendenciaFechamento.ObterPorPendenciaId(pendenciaId);
            if (pendencia == null)
                throw new NegocioException("Pendencia informada não localizada.");

            pendencia.SituacaoNome = Enum.GetName(typeof(SituacaoPendencia), pendencia.Situacao);

            var disciplinaEOL = await mediator.Send(new ObterComponenteCurricularPorIdQuery(pendencia.DisciplinaId));
            if (disciplinaEOL is null)
                throw new NegocioException("Componente curricular informado não localizado.");

            var componentesTurma = await mediator.Send(new ObterDisciplinasPorCodigoTurmaQuery(pendencia.CodigoTurma));

            if (disciplinaEOL.TerritorioSaber)
            {
                var nomeComponenteTerritorioSaber = componentesTurma?.Where(b => b.TerritorioSaber == true && disciplinaEOL.CodigoComponenteCurricular == b.CodigoComponenteTerritorioSaber)
                                     .Select(b => b.Nome).FirstOrDefault();

                if (!String.IsNullOrEmpty(nomeComponenteTerritorioSaber))
                {
                    pendencia.DescricaoHtml = pendencia.DescricaoHtml.Replace(disciplinaEOL.Nome, nomeComponenteTerritorioSaber);
                    disciplinaEOL.Nome = nomeComponenteTerritorioSaber;
                }
            }

            pendencia.ComponenteCurricular = disciplinaEOL.Nome;

            return pendencia;
        }
    }
}
