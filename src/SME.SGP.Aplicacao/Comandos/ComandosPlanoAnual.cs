﻿using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Aplicacao
{
    public class ComandosPlanoAnual : IComandosPlanoAnual
    {
        private readonly IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem;
        private readonly IConsultasTurma consultasTurma;
        private readonly IConsultasPlanoAnual consultasPlanoAnual;
        private readonly IRepositorioComponenteCurricularJurema repositorioComponenteCurricular;
        private readonly IRepositorioObjetivoAprendizagemPlano repositorioObjetivoAprendizagemPlano;
        private readonly IRepositorioPlanoAnual repositorioPlanoAnual;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;
        private readonly IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions;

        public ComandosPlanoAnual(IRepositorioPlanoAnual repositorioPlanoAnual,
                                  IRepositorioObjetivoAprendizagemPlano repositorioObjetivoAprendizagemPlano,
                                  IRepositorioComponenteCurricularJurema repositorioComponenteCurricular,
                                  IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem,
                                  IConsultasTurma consultasTurma,
                                  IConsultasPlanoAnual consultasPlanoAnual,
                                  IUnitOfWork unitOfWork,
                                  IServicoUsuario servicoUsuario, 
                                  IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions)
        {
            this.repositorioPlanoAnual = repositorioPlanoAnual ?? throw new ArgumentNullException(nameof(repositorioPlanoAnual));
            this.repositorioObjetivoAprendizagemPlano = repositorioObjetivoAprendizagemPlano ?? throw new ArgumentNullException(nameof(repositorioObjetivoAprendizagemPlano));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.consultasObjetivoAprendizagem = consultasObjetivoAprendizagem ?? throw new ArgumentNullException(nameof(consultasObjetivoAprendizagem));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
            this.consultasPlanoAnual = consultasPlanoAnual ?? throw new ArgumentNullException(nameof(consultasPlanoAnual));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));
        }

        public async Task Migrar(MigrarPlanoAnualDto migrarPlanoAnualDto)
        {
            var planoAnualDto = migrarPlanoAnualDto.PlanoAnual;
            var planoCopia = new PlanoAnualDto(
                planoAnualDto.AnoLetivo,
                planoAnualDto.Bimestres,
                planoAnualDto.EscolaId,
                planoAnualDto.Id,
                planoAnualDto.TurmaId,
                planoAnualDto.ComponenteCurricularEolId);

            unitOfWork.IniciarTransacao();
            try 
            { 
                foreach (var bimestrePlanoAnual in migrarPlanoAnualDto.BimestresDestino.OrderBy(c => c))
                {
                    var planoAnualOrigem = ObterPlanoAnualSimplificado(planoAnualDto, bimestrePlanoAnual);

                if (planoAnualOrigem.EhNulo())
                    throw new NegocioException("Plano anual de origem não encontrado");

                    var bimestreAtual = planoAnualDto.Bimestres.FirstOrDefault(c => c.Bimestre == bimestrePlanoAnual);
                    foreach (var turmaId in migrarPlanoAnualDto.IdsTurmasDestino)
                    {
                        planoCopia.TurmaId = turmaId;

                        var planoAnual = ObterPlanoAnualSimplificado(planoCopia, bimestrePlanoAnual);

                    if (planoAnual.EhNulo())
                        planoAnual = MapearParaDominio(planoCopia, planoAnual, bimestrePlanoAnual, bimestreAtual.Descricao, bimestreAtual.ObjetivosAprendizagemOpcionais);

                        planoAnual.Descricao = planoAnualOrigem.Descricao;
                        await Salvar(planoCopia, planoAnual, bimestreAtual);
                    }
                }
                unitOfWork.PersistirTransacao();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<IEnumerable<PlanoAnualCompletoDto>> Salvar(PlanoAnualDto planoAnualDto)
        {
            var usuarioAtual = await servicoUsuario.ObterUsuarioLogado();
            if (string.IsNullOrWhiteSpace(usuarioAtual.CodigoRf))
            {
                throw new NegocioException("Não foi possível obter o RF do usuário.");
            }

            if (planoAnualDto.TurmaId.HasValue && !await consultasTurma.TurmaEmPeriodoAberto(planoAnualDto.TurmaId.Value.ToString(), DateTime.Today))
                throw new NegocioException("Turma não esta em período aberto");

            unitOfWork.IniciarTransacao();
            try 
            { 
                foreach (var bimestrePlanoAnual in planoAnualDto.Bimestres)
                {
                    PlanoAnual planoAnual = ObterPlanoAnualSimplificado(planoAnualDto, bimestrePlanoAnual.Bimestre.Value);
                    if (planoAnual.NaoEhNulo())
                    {
                        var podePersistir = await servicoUsuario.PodePersistirTurmaDisciplina(usuarioAtual.CodigoRf, planoAnualDto.TurmaId.ToString(), planoAnualDto.ComponenteCurricularEolId.ToString(), DateTime.Now);
                        if (usuarioAtual.PerfilAtual == Perfis.PERFIL_PROFESSOR && !podePersistir)
                            throw new NegocioException(MensagemNegocioComuns.Voce_nao_pode_fazer_alteracoes_ou_inclusoes_nesta_turma_componente_e_data);
                    }
                    planoAnual = MapearParaDominio(planoAnualDto, planoAnual, bimestrePlanoAnual.Bimestre.Value, bimestrePlanoAnual.Descricao, bimestrePlanoAnual.ObjetivosAprendizagemOpcionais);
                    await Salvar(planoAnualDto, planoAnual, bimestrePlanoAnual);
                }
                unitOfWork.PersistirTransacao();

                var resposta = await consultasPlanoAnual.ObterPorUETurmaAnoEComponenteCurricular(planoAnualDto.EscolaId, planoAnualDto.TurmaId.ToString(), planoAnualDto.AnoLetivo.Value, planoAnualDto.ComponenteCurricularEolId);
                return resposta;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        private static void ValidarObjetivoPertenceAoComponenteCurricular(IEnumerable<ObjetivoAprendizagemDto> objetivosAprendizagem,
                                                                          ObjetivoAprendizagemSimplificadoDto objetivo,
                                                                          ComponenteCurricularJurema componenteEol)
        {
            var objetivoAprendizagem = objetivosAprendizagem.FirstOrDefault(c => c.Id == objetivo.Id);
            if (objetivoAprendizagem.IdComponenteCurricular != componenteEol.CodigoJurema)
            {
                throw new NegocioException($"O objetivo de aprendizagem: '{objetivoAprendizagem.Codigo}' não pertence ao componente curricular: {componenteEol.DescricaoEOL}");
            }
        }

        private async Task AjustarObjetivosAprendizagem(PlanoAnualDto planoAnualDto, BimestrePlanoAnualDto bimestrePlanoAnualDto)
        {
            var objetivosAprendizagemPlanoAnual = repositorioObjetivoAprendizagemPlano.ObterObjetivosAprendizagemPorIdPlano(planoAnualDto.Id);

            if (objetivosAprendizagemPlanoAnual.NaoEhNulo())
            {
                RemoverObjetivos(objetivosAprendizagemPlanoAnual, bimestrePlanoAnualDto);
                await InserirObjetivos(planoAnualDto, objetivosAprendizagemPlanoAnual, bimestrePlanoAnualDto);
            }
        }

        private async Task InserirObjetivos(PlanoAnualDto planoAnualDto, IEnumerable<ObjetivoAprendizagemPlano> objetivosAprendizagemPlanoAnual, BimestrePlanoAnualDto bimestrePlanoAnualDto)
        {
            if (bimestrePlanoAnualDto.ObjetivosAprendizagem.NaoEhNulo() && bimestrePlanoAnualDto.ObjetivosAprendizagem.Any())
            {
                var idsObjetivos = objetivosAprendizagemPlanoAnual?.Select(c => c.ObjetivoAprendizagemJuremaId);
                IEnumerable<ComponenteCurricularJurema> componentesCurriculares = await ObterComponentesCurriculares();
                IEnumerable<ObjetivoAprendizagemDto> objetivosAprendizagem = await ObterObjetivosDeAprendizagem();

                foreach (var objetivo in bimestrePlanoAnualDto.ObjetivosAprendizagem)
                {
                    if (idsObjetivos.NaoEhNulo() && !idsObjetivos.Contains(objetivo.Id))
                    {
                        SalvarObjetivoAprendizagem(planoAnualDto, componentesCurriculares, objetivosAprendizagem, objetivo);
                    }
                }
            }
        }

        private PlanoAnual MapearParaDominio(PlanoAnualDto planoAnualDto, PlanoAnual planoAnual, int bimestre, string descricao, bool objetivosAprendizagemOpcionais)
        {
            if (planoAnual.EhNulo())
            {
                planoAnual = new PlanoAnual();
            }
            planoAnual.Ano = planoAnualDto.AnoLetivo.Value;
            planoAnual.Bimestre = bimestre;
            planoAnual.Descricao = descricao.Replace(configuracaoArmazenamentoOptions.Value.BucketTemp, configuracaoArmazenamentoOptions.Value.BucketArquivos);
            planoAnual.EscolaId = planoAnualDto.EscolaId;
            planoAnual.TurmaId = planoAnualDto.TurmaId.Value;
            planoAnual.ComponenteCurricularEolId = planoAnualDto.ComponenteCurricularEolId;
            planoAnual.ObjetivosAprendizagemOpcionais = objetivosAprendizagemOpcionais;
            return planoAnual;
        }

        private async Task<IEnumerable<ComponenteCurricularJurema>> ObterComponentesCurriculares()
        {
            var componentesCurriculares = await repositorioComponenteCurricular.ListarAsync();
            if (componentesCurriculares.EhNulo())
            {
                throw new NegocioException("Não foi possível recuperar a lista de componentes curriculares.");
            }

            return componentesCurriculares;
        }

        private async Task <IEnumerable<ObjetivoAprendizagemDto>> ObterObjetivosDeAprendizagem()
        {
            var objetivosAprendizagem = await consultasObjetivoAprendizagem.Listar();
            if (objetivosAprendizagem.EhNulo())
            {
                throw new NegocioException("Não foi possível recuperar a lista de objetivos de aprendizagem.");
            }

            return objetivosAprendizagem;
        }

        private PlanoAnual ObterPlanoAnualSimplificado(PlanoAnualDto planoAnualDto, int bimestre)
        {
            return repositorioPlanoAnual.ObterPlanoAnualSimplificadoPorAnoEscolaBimestreETurma(planoAnualDto.AnoLetivo.Value,
                                                                                                      planoAnualDto.EscolaId,
                                                                                                      planoAnualDto.TurmaId.Value,
                                                                                                      bimestre,
                                                                                                      planoAnualDto.ComponenteCurricularEolId);
        }

        private void RemoverObjetivos(IEnumerable<ObjetivoAprendizagemPlano> objetivosAprendizagemPlanoAnual, BimestrePlanoAnualDto bimestrePlanoAnualDto)
        {
            if (objetivosAprendizagemPlanoAnual.NaoEhNulo())
            {
                var idsObjetivos = bimestrePlanoAnualDto.ObjetivosAprendizagem.Select(x => x.Id);
                var objetivosRemover = objetivosAprendizagemPlanoAnual.Where(c => !idsObjetivos.Contains(c.ObjetivoAprendizagemJuremaId));

                foreach (var objetivo in objetivosRemover)
                {
                    repositorioObjetivoAprendizagemPlano.Remover(objetivo.Id);
                }
            }
        }

        private async Task Salvar(PlanoAnualDto planoAnualDto, PlanoAnual planoAnual, BimestrePlanoAnualDto bimestrePlanoAnualDto)
        {
            planoAnualDto.Id = repositorioPlanoAnual.Salvar(planoAnual);
            if (!planoAnual.Migrado)
                await AjustarObjetivosAprendizagem(planoAnualDto, bimestrePlanoAnualDto);
        }

        private void SalvarObjetivoAprendizagem(PlanoAnualDto planoAnualDto,
                                                IEnumerable<ComponenteCurricularJurema> componentesCurriculares,
                                                IEnumerable<ObjetivoAprendizagemDto> objetivosAprendizagem,
                                                ObjetivoAprendizagemSimplificadoDto objetivo)
        {
            var componenteEol = componentesCurriculares.FirstOrDefault(c => c.CodigoJurema == objetivo.IdComponenteCurricular && c.CodigoEOL == objetivo.ComponenteCurricularEolId);

            ValidarObjetivoPertenceAoComponenteCurricular(objetivosAprendizagem, objetivo, componenteEol);

            repositorioObjetivoAprendizagemPlano.Salvar(new ObjetivoAprendizagemPlano()
            {
                ObjetivoAprendizagemJuremaId = objetivo.Id,
                ComponenteCurricularId = componenteEol.Id,
                PlanoId = planoAnualDto.Id
            });
        }
    }
}