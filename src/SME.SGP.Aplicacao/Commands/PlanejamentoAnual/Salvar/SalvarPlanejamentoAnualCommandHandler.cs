using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanejamentoAnualCommandHandler : AbstractUseCase, IRequestHandler<SalvarPlanejamentoAnualCommand, PlanejamentoAnualAuditoriaDto>
    {
        private readonly IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual;
        private readonly IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar;
        private readonly IRepositorioPlanejamentoAnualComponente repositorioPlanejamentoAnualComponente;
        private readonly IRepositorioPlanejamentoAnualObjetivosAprendizagem repositorioPlanejamentoAnualObjetivosAprendizagem;
        private readonly IUnitOfWork unitOfWork;
        private readonly IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions;

        public SalvarPlanejamentoAnualCommandHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual,
                                                     IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar,
                                                     IRepositorioPlanejamentoAnualComponente repositorioPlanejamentoAnualComponente,
                                                     IRepositorioPlanejamentoAnualObjetivosAprendizagem repositorioPlanejamentoAnualObjetivosAprendizagem,
                                                     IMediator mediator,
                                                     IUnitOfWork unitOfWork,
                                                     IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions) : base(mediator)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnual));
            this.repositorioPlanejamentoAnualPeriodoEscolar = repositorioPlanejamentoAnualPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualPeriodoEscolar));
            this.repositorioPlanejamentoAnualComponente = repositorioPlanejamentoAnualComponente ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualComponente));
            this.repositorioPlanejamentoAnualObjetivosAprendizagem = repositorioPlanejamentoAnualObjetivosAprendizagem ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualObjetivosAprendizagem));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));
        }
        public async Task<PlanejamentoAnualAuditoriaDto> Handle(SalvarPlanejamentoAnualCommand comando, CancellationToken cancellationToken)
        {

            PlanejamentoAnualAuditoriaDto auditorias = new PlanejamentoAnualAuditoriaDto();
            try
            {
                unitOfWork.IniciarTransacao();
                var listaDescricao = new List<PlanejamentoAnualComponenteResumidoDto>();
                var planejamentoAnual = await repositorioPlanejamentoAnual.ObterPlanejamentoSimplificadoPorTurmaEComponenteCurricular(comando.TurmaId, comando.ComponenteCurricularId);
                if (planejamentoAnual.EhNulo())
                {

                    planejamentoAnual = new PlanejamentoAnual(comando.TurmaId, comando.ComponenteCurricularId);
                    var id = await repositorioPlanejamentoAnual.SalvarAsync(planejamentoAnual);
                    auditorias.Id = id;
                }

                List<string> excessoes = new List<string>();
                var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
                var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(comando.TurmaId));
                if (turma.EhNulo())
                    throw new NegocioException($"Turma de id [{turma.Id}] não localizada!");

                var regenteAtual = await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(comando.ComponenteCurricularId, turma.CodigoTurma, DateTime.Now.Date, usuario));

                foreach (var periodoEscolar in comando.PeriodosEscolares)
                {
                    var periodo = await mediator.Send(new ObterPeriodoEscolarePorIdQuery(periodoEscolar.PeriodoEscolarId));
                    if (usuario.EhProfessor())
                    {
                        var temAtribuicao = await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQuery(comando.ComponenteCurricularId, turma.CodigoTurma, usuario.CodigoRf, periodo.PeriodoInicio.Date, periodo.PeriodoFim.Date));
                        if (!temAtribuicao && !regenteAtual)
                            excessoes.Add($"Você não possui atribuição na turma {turma.Nome} - {periodo.Bimestre}° Bimestre.");
                    }

                    var periodoEmAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, periodo.Bimestre, turma.AnoLetivo == DateTime.Today.Year));
                    if (!periodoEmAberto)
                        excessoes.Add($"O {periodo.Bimestre}° Bimestre não está aberto.");
                }

                if (!excessoes.Any())
                {

                    foreach (var periodo in comando.PeriodosEscolares)
                    {

                        var planejamentoAnualPeriodoEscolar = await repositorioPlanejamentoAnualPeriodoEscolar.ObterPorPlanejamentoAnualIdEPeriodoId(planejamentoAnual.Id, periodo.PeriodoEscolarId);
                        if (planejamentoAnualPeriodoEscolar.EhNulo())
                        {
                            planejamentoAnualPeriodoEscolar = new PlanejamentoAnualPeriodoEscolar(periodo.PeriodoEscolarId)
                            {
                                PlanejamentoAnualId = planejamentoAnual.Id
                            };

                            await repositorioPlanejamentoAnualPeriodoEscolar.SalvarAsync(planejamentoAnualPeriodoEscolar);
                        }
                        else
                        {
                            foreach (var componente in periodo.Componentes)
                            {
                                await repositorioPlanejamentoAnualObjetivosAprendizagem.RemoverTodosPorPlanejamentoAnualPeriodoEscolarIdEComponenteCurricularId(planejamentoAnualPeriodoEscolar.Id, componente.ComponenteCurricularId);
                            }
                        }

                        var auditoria = new PlanejamentoAnualPeriodoEscolarDto
                        {
                            PeriodoEscolarId = periodo.PeriodoEscolarId,
                        };

                        var componentes = periodo.Componentes.Select(c => new PlanejamentoAnualComponente
                        {
                            ComponenteCurricularId = c.ComponenteCurricularId,
                            Descricao = c.Descricao,
                            PlanejamentoAnualPeriodoEscolarId = planejamentoAnualPeriodoEscolar.Id,
                            ObjetivosAprendizagem = c.ObjetivosAprendizagemId.Select(o => new PlanejamentoAnualObjetivoAprendizagem
                            {
                                ObjetivoAprendizagemId = o
                            })?.ToList()
                        })?.ToList();

                        if (componentes.NaoEhNulo())
                        {

                            foreach (var componente in componentes)
                            {
                                var planejamentoAnualComponente = await repositorioPlanejamentoAnualComponente.ObterPorPlanejamentoAnualPeriodoEscolarId(componente.ComponenteCurricularId, planejamentoAnualPeriodoEscolar.Id);
                                if (planejamentoAnualComponente.EhNulo())
                                {
                                    planejamentoAnualComponente = new PlanejamentoAnualComponente
                                    {
                                        ComponenteCurricularId = componente.ComponenteCurricularId,
                                        PlanejamentoAnualPeriodoEscolarId = planejamentoAnualPeriodoEscolar.Id,
                                    };
                                }
                                listaDescricao.Add(new PlanejamentoAnualComponenteResumidoDto() { DescricaoNovo = componente.Descricao,DescricaoAtual = planejamentoAnualComponente.Descricao });
                                planejamentoAnualComponente.Descricao = componente.Descricao = componente.Descricao.Replace(configuracaoArmazenamentoOptions.Value.BucketTemp, configuracaoArmazenamentoOptions.Value.BucketArquivos);
                                await repositorioPlanejamentoAnualComponente.SalvarAsync(planejamentoAnualComponente);
                                auditoria.Componentes.Add(new PlanejamentoAnualComponenteDto
                                {
                                    Auditoria = (AuditoriaDto)planejamentoAnualComponente,
                                    Descricao = planejamentoAnualComponente.Descricao,
                                    ComponenteCurricularId = componente.ComponenteCurricularId,
                                });

                                await Task.Run(() => repositorioPlanejamentoAnualObjetivosAprendizagem.SalvarVarios(componente.ObjetivosAprendizagem, planejamentoAnualComponente.Id));
                            }
                        }
                        auditorias.PeriodosEscolares.Add(auditoria);
                    }
                }

                if (excessoes.Any())
                {
                    var str = new StringBuilder();
                    str.AppendLine($"Os seguintes erros foram encontrados: ");
                    foreach (var t in excessoes)
                    {
                        str.AppendLine($"- {t}");
                    }

                    throw new NegocioException(str.ToString());
                }

                unitOfWork.PersistirTransacao();
                foreach (var item in listaDescricao)
                {
                    await MoverRemoverExcluidos(item.DescricaoNovo, item.DescricaoAtual);
                }
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                throw;
            }

            return auditorias;
        }

        private async Task MoverRemoverExcluidos(string novo, string atual)
        {
            if (!string.IsNullOrEmpty(novo))
            {
                var moverArquivo = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.PlanejamentoAnual, atual, novo));
            }
            if (!string.IsNullOrEmpty(atual))
            {
                var deletarArquivosNaoUtilziados = await mediator.Send(new RemoverArquivosExcluidosCommand(atual, novo, TipoArquivo.PlanejamentoAnual.Name()));
            }
        }
    }
}
