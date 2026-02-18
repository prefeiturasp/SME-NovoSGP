using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoObjetivosAprendizagem : IServicoObjetivosAprendizagem
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IMediator mediator;
        private readonly IRepositorioObjetivoAprendizagem repositorioObjetivoAprendizagem;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IServicoJurema servicoJurema;

        private readonly Dictionary<string, int> Anos = new Dictionary<string, int>
        {           
            {"first", 1},
            {"second", 2},
            {"third", 3},
            {"fourth", 4},
            {"fifth", 5},
            {"sixth", 6},
            {"seventh", 7},
            {"eighth", 8},
            {"nineth", 9},
            {"tenth", 10},
            {"eleventh", 11},
            {"twelfth", 12},
            {"thirteenth", 13},
            {"fourteenth", 14}
        };

        public ServicoObjetivosAprendizagem(IServicoJurema servicoJurema,
                                            IRepositorioObjetivoAprendizagem repositorioObjetivoAprendizagem,
                                            IRepositorioParametrosSistema repositorioParametrosSistema,
                                            IRepositorioCache repositorioCache,
                                            IMediator mediator)
        {
            this.servicoJurema = servicoJurema ?? throw new ArgumentNullException(nameof(servicoJurema));
            this.repositorioObjetivoAprendizagem = repositorioObjetivoAprendizagem ?? throw new ArgumentNullException(nameof(repositorioObjetivoAprendizagem));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task SincronizarObjetivosComJurema()
        {
            try
            {
                var parametrosDataUltimaAtualizacao = await mediator.Send(new ObterParametroSistemaUnicoChaveEValorPorTipoQuery(TipoParametroSistema.DataUltimaAtualizacaoObjetivosJurema));

                if (parametrosDataUltimaAtualizacao.HasValue)
                {
                    var dataUltimaAtualizacao = DateTime.Parse(parametrosDataUltimaAtualizacao.Value.Value);

                    var objetivosJuremaRespostaApi = await servicoJurema.ObterListaObjetivosAprendizagem();
                    if (objetivosJuremaRespostaApi != null && objetivosJuremaRespostaApi.Any())
                    {
                        var objetivosBase = await repositorioObjetivoAprendizagem.ListarAsync();

                        var objetivosJuremaResposta = objetivosJuremaRespostaApi.Where(c => c.Codigo.Length <= 20);

                        var objetivosAIncluir = objetivosJuremaResposta?
                            .Where(c => !objetivosBase.Any(b => b.Id == c.Id));

                        var objetivosADesativar = objetivosBase?
                            .Where(c => !c.Excluido)?.Where(c => !objetivosJuremaResposta.Any(b => b.Id == c.Id));

                        var objetivosAReativar = objetivosJuremaResposta?
                            .Where(c => objetivosBase.Any(b => b.Id == c.Id && b.Excluido));

                        var objetivosAAtualizar = objetivosJuremaResposta?
                            .Where(c => c.AtualizadoEm > dataUltimaAtualizacao);

                        var atualizarUltimaDataAtualizacao = false;
                        var houveAlteracaoNosDados = false;

                        if (objetivosAAtualizar != null && objetivosAAtualizar.Any())
                        {
                            try
                            {
                                foreach (var objetivo in objetivosAAtualizar)
                                {
                                    if (ValidarAno(objetivo.Ano))
                                        await AtualizarObjetivoBaseComVerificacaoCodigo(objetivo);
                                    else
                                        await mediator.Send(new SalvarLogViaRabbitCommand($"Objetivo ID '{objetivo.Id}' possui ano inválido '{objetivo.Ano}'. Atualização ignorada.", LogNivel.Negocio, LogContexto.ObjetivosAprendizagem));
                                }
                            }
                            catch (Exception ex)
                            {
                                await mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro ao atualizar os objetivos de aprendizagem: {ex.Message}", LogNivel.Critico, LogContexto.ObjetivosAprendizagem, ex.ToString()));
                            }

                            atualizarUltimaDataAtualizacao = true;
                            houveAlteracaoNosDados = true;
                        }

                        if (objetivosAIncluir != null && objetivosAIncluir.Any())
                        {
                            foreach (var objetivo in objetivosAIncluir)
                            {
                                if (!ValidarAno(objetivo.Ano))
                                {
                                    await mediator.Send(new SalvarLogViaRabbitCommand($"Objetivo ID '{objetivo.Id}' possui ano inválido '{objetivo.Ano}'. Inserção ignorada.", LogNivel.Negocio, LogContexto.ObjetivosAprendizagem));
                                    continue;
                                }

                                var codigoCompleto = NormalizarCodigoObjetivo(objetivo.Codigo);

                                if (string.IsNullOrEmpty(codigoCompleto))
                                {
                                    await mediator.Send(new SalvarLogViaRabbitCommand($"Objetivo de aprendizagem com ID '{objetivo.Id}' possui código vazio ou nulo após normalização. Inserção ignorada.", LogNivel.Negocio, LogContexto.ObjetivosAprendizagem));
                                    continue;
                                }

                                var existeCodigo = await repositorioObjetivoAprendizagem.ExistePorCodigoCompletoEAnoTurmaAsync(codigoCompleto, objetivo.Ano);
                                if (!existeCodigo)
                                {
                                    try { 
                                    var objetivoParaInserir = MapearObjetivoRespostaParaDominio(objetivo);
                                    objetivoParaInserir.CodigoCompleto = codigoCompleto;

                                    await repositorioObjetivoAprendizagem.InserirAsync(objetivoParaInserir);
                                    }
                                    catch (Exception ex)
                                    {
                                        await mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro ao inserir os objetivos de aprendizagem: {ex.Message}", LogNivel.Critico, LogContexto.ObjetivosAprendizagem, ex.ToString()));
                                    }
                                    houveAlteracaoNosDados = true;                                  
                                }
                                else
                                {
                                    await mediator.Send(new SalvarLogViaRabbitCommand($"Objetivo de aprendizagem com código '{codigoCompleto}' já existe na base de dados. Inserção ignorada.", LogNivel.Negocio, LogContexto.ObjetivosAprendizagem));
                                }
                            }
                        }

                        if (objetivosAReativar != null && objetivosAReativar.Any())
                        {
                            try
                            {
                                foreach (var objetivo in objetivosAReativar)
                                {
                                    if (ValidarAno(objetivo.Ano))
                                        await ReativarObjetivoComVerificacaoCodigo(objetivo);
                                    else
                                        await mediator.Send(new SalvarLogViaRabbitCommand($"Objetivo ID '{objetivo.Id}' possui ano inválido '{objetivo.Ano}'. Reativação ignorada.", LogNivel.Negocio, LogContexto.ObjetivosAprendizagem));
                                }
                            }
                            catch (Exception ex)
                            {
                                await mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro ao reativar os objetivos de aprendizagem: {ex.Message}", LogNivel.Critico, LogContexto.ObjetivosAprendizagem, ex.ToString()));
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
                            await repositorioCache.RemoverAsync(NomeChaveCache.OBJETIVOS_APRENDIZAGEM);

                        await mediator.Send(new SalvarLogViaRabbitCommand($"Sincronização de objetivos de aprendizagem concluída com sucesso.", LogNivel.Negocio, LogContexto.ObjetivosAprendizagem));
                    }
                    else
                    {
                        await mediator.Send(new SalvarLogViaRabbitCommand("Sincronização concluída: nenhum objetivo de aprendizagem retornado pela API da Jurema.", LogNivel.Negocio, LogContexto.ObjetivosAprendizagem));
                    }
                }
                else
                    await mediator.Send(new SalvarLogViaRabbitCommand("Parâmetro 'DataUltimaAtualizacaoObjetivosJurema' não encontrado na base de dados, os objetivos de aprendizagem não serão atualizados.", LogNivel.Negocio, LogContexto.ObjetivosAprendizagem));
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro ao sincronizar os objetivos de aprendizagem com a Jurema: {ex.Message}", LogNivel.Critico, LogContexto.ObjetivosAprendizagem, ex.ToString()));
            }
        }

        private bool ValidarAno(string ano)
        {
            if (string.IsNullOrWhiteSpace(ano))
                return false;

            if (ano.All(v => char.IsDigit(v)))
            {
                var anoNumerico = Convert.ToInt32(ano);
                return Anos.ContainsValue(anoNumerico);
            }

            return Anos.ContainsKey(ano);
        }

        private async Task<bool> ReativarObjetivoComVerificacaoCodigo(ObjetivoAprendizagemResposta objetivo)
        {
            var objetivoBase = await repositorioObjetivoAprendizagem.ObterPorIdAsync(objetivo.Id);

            if (objetivoBase != null && objetivoBase.Excluido)
            {
                var codigoNormalizado = NormalizarCodigoObjetivo(objetivo.Codigo);

                var existeCodigoEmObjetivoAtivo = await repositorioObjetivoAprendizagem.ExistePorCodigoCompletoEAnoTurmaAsync(codigoNormalizado, objetivo.Ano);

                if (existeCodigoEmObjetivoAtivo)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand(
                        $"Reativação do objetivo ID '{objetivo.Id}' ignorada: já existe outro objetivo ativo com o código '{codigoNormalizado}'.",
                        LogNivel.Negocio, LogContexto.ObjetivosAprendizagem));

                    return false;
                }

                await repositorioObjetivoAprendizagem.ReativarAsync(objetivo.Id);

                return true;
            }

            return false;
        }

        private async Task AtualizarObjetivoBaseComVerificacaoCodigo(ObjetivoAprendizagemResposta objetivo)
        {
            var objetivoBase = await repositorioObjetivoAprendizagem.ObterPorIdAsync(objetivo.Id);
            if (objetivoBase != null)
            {
                var codigoNormalizado = NormalizarCodigoObjetivo(objetivo.Codigo);

                var existeCodigoEmOutroObjetivo = await repositorioObjetivoAprendizagem.ExistePorCodigoCompletoEAnoTurmaAsync(codigoNormalizado, objetivo.Ano);

                if (existeCodigoEmOutroObjetivo)
                {
                    MapearParaObjetivoDominioSemCodigo(objetivo, objetivoBase);

                    await mediator.Send(new SalvarLogViaRabbitCommand(
                        $"Objetivo ID '{objetivo.Id}': código '{codigoNormalizado}' já existe em outro objetivo. Mantido código original '{objetivoBase.CodigoCompleto}' e atualizados demais campos.",
                        LogNivel.Negocio, LogContexto.ObjetivosAprendizagem));
                }
                else
                {
                    MapearParaObjetivoDominio(objetivo, objetivoBase);
                }

                await repositorioObjetivoAprendizagem.AtualizarAsync(objetivoBase);
            }
        }

        private static void MapearParaObjetivoDominioSemCodigo(ObjetivoAprendizagemResposta objetivo, ObjetivoAprendizagem objetivoBase)
        {
            objetivoBase.AnoTurma = objetivo.Ano;
            objetivoBase.AtualizadoEm = objetivo.AtualizadoEm;
            objetivoBase.ComponenteCurricularId = objetivo.ComponenteCurricularId;
            objetivoBase.CriadoEm = objetivo.CriadoEm;
            objetivoBase.Descricao = objetivo.Descricao;
        }

        private static string NormalizarCodigoObjetivo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return string.Empty;

            return Regex.Replace(codigo.Trim(), @"[^\w\d]", "");
        }

        private static void MapearParaObjetivoDominio(ObjetivoAprendizagemResposta objetivo, ObjetivoAprendizagem objetivoBase)
        {
            objetivoBase.AnoTurma = objetivo.Ano;
            objetivoBase.AtualizadoEm = objetivo.AtualizadoEm;
            objetivoBase.CodigoCompleto = NormalizarCodigoObjetivo(objetivo.Codigo);
            objetivoBase.ComponenteCurricularId = objetivo.ComponenteCurricularId;
            objetivoBase.CriadoEm = objetivo.CriadoEm;
            objetivoBase.Descricao = objetivo.Descricao;
        }

        private ObjetivoAprendizagem MapearObjetivoRespostaParaDominio(ObjetivoAprendizagemResposta objetivo)
        {
            return new ObjetivoAprendizagem
            {
                AnoTurma = objetivo.Ano,
                AtualizadoEm = objetivo.AtualizadoEm,
                CodigoCompleto = NormalizarCodigoObjetivo(objetivo.Codigo),
                ComponenteCurricularId = objetivo.ComponenteCurricularId,
                CriadoEm = objetivo.CriadoEm,
                Descricao = objetivo.Descricao,
                Id = objetivo.Id
            };
        }
    }
}