﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Aplicacao
{
    public class ComandosPlanoAnualTerritorioSaber : IComandosPlanoAnualTerritorioSaber
    {
        private readonly IRepositorioPlanoAnualTerritorioSaber repositorioPlanoAnualTerritorioSaber;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;
        private readonly IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions;

        public ComandosPlanoAnualTerritorioSaber(IRepositorioPlanoAnualTerritorioSaber repositorioPlanoAnualTerritorioSaber,
                                  IUnitOfWork unitOfWork,
                                  IServicoUsuario servicoUsuario, IMediator mediator,
                                  IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions)
        {
            this.repositorioPlanoAnualTerritorioSaber = repositorioPlanoAnualTerritorioSaber ?? throw new ArgumentNullException(nameof(repositorioPlanoAnualTerritorioSaber));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));
        }

        public async Task<IEnumerable<EntidadeBase>> Salvar(PlanoAnualTerritorioSaberDto planoAnualTerritorioSaberDto)
        {
            Validar(planoAnualTerritorioSaberDto);

            var listaAuditoria = new List<EntidadeBase>();

            unitOfWork.IniciarTransacao();
            var listaDescricao = new List<PlanoAnualTerritorioSaberResumidoDto>();
            var usuarioAtual = servicoUsuario.ObterUsuarioLogado().Result;
            if (string.IsNullOrWhiteSpace(usuarioAtual.CodigoRf))
            {
                throw new NegocioException("Não foi possível obter o RF do usuário.");
            }
            foreach (var bimestrePlanoAnual in planoAnualTerritorioSaberDto.Bimestres)
            {
                PlanoAnualTerritorioSaber planoAnualTerritorioSaber = await ObterPlanoAnualTerritorioSaberSimplificado(planoAnualTerritorioSaberDto, bimestrePlanoAnual.Bimestre.Value);
                if (planoAnualTerritorioSaber != null)
                {
                    if (usuarioAtual.PerfilAtual == Perfis.PERFIL_PROFESSOR && !servicoUsuario.PodePersistirTurmaDisciplina(usuarioAtual.CodigoRf, planoAnualTerritorioSaberDto.TurmaId.ToString(), planoAnualTerritorioSaberDto.TerritorioExperienciaId.ToString(), DateTime.Now).Result)
                        throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma, componente curricular e data.");
                }
                listaDescricao.Add(new PlanoAnualTerritorioSaberResumidoDto() { DesenvolvimentoNovo = bimestrePlanoAnual.Desenvolvimento,
                                                                                DesenvolvimentoAtual = planoAnualTerritorioSaber!= null ? planoAnualTerritorioSaber.Desenvolvimento : string.Empty,
                                                                                ReflexaoAtual = planoAnualTerritorioSaber != null ? planoAnualTerritorioSaber.Reflexao : string.Empty,
                                                                                ReflexaoNovo = bimestrePlanoAnual.Reflexao});

                planoAnualTerritorioSaber = MapearParaDominio(planoAnualTerritorioSaberDto, planoAnualTerritorioSaber, bimestrePlanoAnual.Bimestre.Value, bimestrePlanoAnual.Desenvolvimento, bimestrePlanoAnual.Reflexao);
                repositorioPlanoAnualTerritorioSaber.Salvar(planoAnualTerritorioSaber);

                listaAuditoria.Add(planoAnualTerritorioSaber);
            }

            unitOfWork.PersistirTransacao();
            foreach (var item in listaDescricao)
            {
                await MoverRemoverExcluidos(item.DesenvolvimentoNovo, item.DesenvolvimentoAtual);
                await MoverRemoverExcluidos(item.ReflexaoNovo, item.ReflexaoAtual);
            }
            return listaAuditoria;
        }
        private async Task MoverRemoverExcluidos(string novo, string atual)
        {
            if (!string.IsNullOrEmpty(novo))
            {
                await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.TerritorioSaber, atual, novo));
            }
            if (!string.IsNullOrEmpty(atual))
            {
                await mediator.Send(new RemoverArquivosExcluidosCommand(atual, novo, TipoArquivo.TerritorioSaber.Name()));
            }
        }
        private void Validar(PlanoAnualTerritorioSaberDto planoAnualTerritorioSaberDto)
        {
            var bimestresDescricaoVazia = planoAnualTerritorioSaberDto.Bimestres.Where(b =>
                   string.IsNullOrEmpty(b.Desenvolvimento) && string.IsNullOrEmpty(b.Reflexao));

            if (bimestresDescricaoVazia.Any())
                throw new NegocioException($@"É necessário preencher o desenvolvimento e/ou reflexão do 
                                            {string.Join(", ", bimestresDescricaoVazia.Select(b => $"{b.Bimestre}º"))} bimestre");
        }

        private async Task<PlanoAnualTerritorioSaber> ObterPlanoAnualTerritorioSaberSimplificado(PlanoAnualTerritorioSaberDto planoAnualTerritorioSaberDto, int bimestre)
        {
            return await repositorioPlanoAnualTerritorioSaber.ObterPlanoAnualTerritorioSaberSimplificadoPorAnoEscolaBimestreETurma(planoAnualTerritorioSaberDto.AnoLetivo.Value,
                                                                                                      planoAnualTerritorioSaberDto.EscolaId,
                                                                                                      planoAnualTerritorioSaberDto.TurmaId.Value,
                                                                                                      bimestre,
                                                                                                      planoAnualTerritorioSaberDto.TerritorioExperienciaId);
        }
        private PlanoAnualTerritorioSaber MapearParaDominio(PlanoAnualTerritorioSaberDto planoAnualTerritorioSaberDto, PlanoAnualTerritorioSaber planoAnualTerritorioSaber, int bimestre, string desenvolvimento, string reflexao)
        {
            if (planoAnualTerritorioSaber == null)
            {
                planoAnualTerritorioSaber = new PlanoAnualTerritorioSaber();
            }
            planoAnualTerritorioSaber.Ano = planoAnualTerritorioSaberDto.AnoLetivo.Value;
            planoAnualTerritorioSaber.Bimestre = bimestre;
            planoAnualTerritorioSaber.Reflexao = reflexao.Replace(configuracaoArmazenamentoOptions.Value.BucketTemp, configuracaoArmazenamentoOptions.Value.BucketArquivos) ?? string.Empty;
            planoAnualTerritorioSaber.Desenvolvimento = desenvolvimento.Replace(configuracaoArmazenamentoOptions.Value.BucketTemp, configuracaoArmazenamentoOptions.Value.BucketArquivos) ?? string.Empty;
            planoAnualTerritorioSaber.EscolaId = planoAnualTerritorioSaberDto.EscolaId;
            planoAnualTerritorioSaber.TurmaId = planoAnualTerritorioSaberDto.TurmaId.Value;
            planoAnualTerritorioSaber.TerritorioExperienciaId = planoAnualTerritorioSaberDto.TerritorioExperienciaId;
            return planoAnualTerritorioSaber;
        }
    }
}
