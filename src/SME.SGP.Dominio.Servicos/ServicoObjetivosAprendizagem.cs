﻿using Sentry;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoObjetivosAprendizagem : IServicoObjetivosAprendizagem
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioObjetivoAprendizagem repositorioObjetivoAprendizagem;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IServicoJurema servicoJurema;

        public ServicoObjetivosAprendizagem(IServicoJurema servicoJurema,
                                            IRepositorioObjetivoAprendizagem repositorioObjetivoAprendizagem,
                                            IRepositorioParametrosSistema repositorioParametrosSistema,
                                            IRepositorioCache repositorioCache)
        {
            this.servicoJurema = servicoJurema ?? throw new ArgumentNullException(nameof(servicoJurema));
            this.repositorioObjetivoAprendizagem = repositorioObjetivoAprendizagem ?? throw new ArgumentNullException(nameof(repositorioObjetivoAprendizagem));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task SincronizarObjetivosComJurema()
        {
            var parametrosDataUltimaAtualizacao = await repositorioParametrosSistema.ObterUnicoChaveEValorPorTipo(TipoParametroSistema.DataUltimaAtualizacaoObjetivosJurema);
            if (parametrosDataUltimaAtualizacao.HasValue)
            {
                var dataUltimaAtualizacao = DateTime.Parse(parametrosDataUltimaAtualizacao.Value.Value);

                var objetivosJuremaRespostaApi = await servicoJurema.ObterListaObjetivosAprendizagem();
                if (objetivosJuremaRespostaApi != null && objetivosJuremaRespostaApi.Any())
                {
                    var objetivosBase = await repositorioObjetivoAprendizagem.ListarAsync();

                    var objetivosJuremaResposta = objetivosJuremaRespostaApi.Where(c => c.Codigo.Length <= 20);

                    var objetivosAIncluir = objetivosJuremaResposta?.Where(c => !objetivosBase.Any(b => b.CodigoCompleto == c.Codigo));
                    var objetivosADesativar = objetivosBase?.Where(c => !c.Excluido)?.Where(c => !objetivosJuremaResposta.Any(b => b.Codigo == c.CodigoCompleto));
                    var objetivosAReativar = objetivosJuremaResposta?.Where(c => objetivosBase.Any(b => b.CodigoCompleto == c.Codigo && b.Excluido));
                    var objetivosAAtualizar = objetivosJuremaResposta?.Where(c => c.AtualizadoEm > dataUltimaAtualizacao);

                    var atualizarUltimaDataAtualizacao = false;
                    var houveAlteracaoNosDados = false;

                    if (objetivosAAtualizar != null && objetivosAAtualizar.Any())
                    {
                        foreach (var objetivo in objetivosAAtualizar)
                        {
                            await AtualizarObjetivoBase(objetivo);
                        }
                        atualizarUltimaDataAtualizacao = true;
                        houveAlteracaoNosDados = true;
                    }

                    if (objetivosAIncluir != null && objetivosAIncluir.Any())
                    {
                        foreach (var objetivo in objetivosAIncluir)
                        {
                            await repositorioObjetivoAprendizagem.SalvarAsync(MapearObjetivoRespostaParaDominio(objetivo));
                        }
                        houveAlteracaoNosDados = true;
                    }

                    if (objetivosAReativar != null && objetivosAReativar.Any())
                    {
                        foreach (var objetivo in objetivosAReativar)
                        {
                            await repositorioObjetivoAprendizagem.ReativarAsync(objetivo.Id);
                        }
                        houveAlteracaoNosDados = true;
                    }

                    if (objetivosADesativar != null && objetivosADesativar.Any())
                    {
                        foreach (var objetivo in objetivosADesativar)
                        {
                            objetivo.Desativar();
                            await repositorioObjetivoAprendizagem.AtualizarAsync(objetivo);
                        }
                        houveAlteracaoNosDados = true;
                    }

                    if (atualizarUltimaDataAtualizacao)
                    {
                        dataUltimaAtualizacao = objetivosJuremaResposta.Max(c => c.AtualizadoEm);
                        await repositorioParametrosSistema.AtualizarValorPorTipoAsync(TipoParametroSistema.DataUltimaAtualizacaoObjetivosJurema, dataUltimaAtualizacao.ToString("yyyy-MM-dd HH:mm:ss.fff tt"));
                    }

                    if (houveAlteracaoNosDados)
                        await repositorioCache.RemoverAsync("ObjetivosAprendizagem");
                }
            }
            else
                SentrySdk.CaptureException(new NegocioException("Parâmetro 'DataUltimaAtualizacaoObjetivosJurema' não encontrado na base de dados, os objetivos de aprendizagem não serão atualizados."));
        }

        private static void MapearParaObjetivoDominio(ObjetivoAprendizagemResposta objetivo, ObjetivoAprendizagem objetivoBase)
        {
            objetivoBase.AnoTurma = objetivo.Ano;
            objetivoBase.AtualizadoEm = objetivo.AtualizadoEm;
            objetivoBase.CodigoCompleto = objetivo.Codigo;
            objetivoBase.ComponenteCurricularId = objetivo.ComponenteCurricularId;
            objetivoBase.CriadoEm = objetivo.CriadoEm;
            objetivoBase.Descricao = objetivo.Descricao;
        }

        private async Task AtualizarObjetivoBase(ObjetivoAprendizagemResposta objetivo)
        {
            var objetivoBase = await repositorioObjetivoAprendizagem.ObterPorIdAsync(objetivo.Id);
            if (objetivoBase != null)
            {
                MapearParaObjetivoDominio(objetivo, objetivoBase);
                await repositorioObjetivoAprendizagem.AtualizarAsync(objetivoBase);
            }
        }

        private ObjetivoAprendizagem MapearObjetivoRespostaParaDominio(ObjetivoAprendizagemResposta objetivo)
        {
            return new ObjetivoAprendizagem
            {
                AnoTurma = objetivo.Ano,
                AtualizadoEm = objetivo.AtualizadoEm,
                CodigoCompleto = objetivo.Codigo,
                ComponenteCurricularId = objetivo.ComponenteCurricularId,
                CriadoEm = objetivo.CriadoEm,
                Descricao = objetivo.Descricao,
                Id = objetivo.Id
            };
        }
    }
}