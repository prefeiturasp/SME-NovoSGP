﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ValidarMediaAlunosAtividadeAvaliativaUseCase : AbstractUseCase, IValidarMediaAlunosAtividadeAvaliativaUseCase
    {
        private readonly IRepositorioConceitoConsulta repositorioConceito;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IRepositorioNotaParametro repositorioNotaParametro;
        private readonly IRepositorioAulaConsulta repositorioAula;
        private readonly IRepositorioTurmaConsulta repositorioTurma;
        private readonly IRepositorioNotaTipoValorConsulta repositorioNotaTipoValor;
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IRepositorioCiclo repositorioCiclo;
        private readonly IServicoUsuario servicoUsuario;

        public ValidarMediaAlunosAtividadeAvaliativaUseCase(IMediator mediator, IRepositorioConceitoConsulta repositorioConceito, 
                                                            IConsultasAbrangencia consultasAbrangencia, IRepositorioNotaParametro repositorioNotaParametro,
                                                            IRepositorioAulaConsulta repositorioAula, IRepositorioTurmaConsulta repositorioTurma,
                                                            IRepositorioNotaTipoValorConsulta repositorioNotaTipoValor, IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar,
                                                            IRepositorioCiclo repositorioCiclo, IServicoUsuario servicoUsuario) : base(mediator)
        {
            this.repositorioConceito = repositorioConceito ?? throw new ArgumentNullException(nameof(repositorioConceito));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
            this.repositorioNotaParametro = repositorioNotaParametro ?? throw new ArgumentNullException(nameof(repositorioNotaParametro));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioNotaTipoValor = repositorioNotaTipoValor ?? throw new ArgumentNullException(nameof(repositorioNotaTipoValor));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioCiclo = repositorioCiclo ?? throw new ArgumentNullException(nameof(repositorioCiclo));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroValidarMediaAlunosAtividadeAvaliativaDto>();

            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;

            var atividadeAvaliativa = filtro.AtividadesAvaliativas.FirstOrDefault(x => x.Id == filtro.ChaveNotasPorAvaliacao);
            
            var valoresConceito = await repositorioConceito.ObterPorData(atividadeAvaliativa.DataAvaliacao);
            
            var abrangenciaTurma = await mediator.Send(new ObterAbrangenciaTurmaQuery(atividadeAvaliativa.TurmaId, filtro.Usuario.Login, filtro.Usuario.PerfilAtual, filtro.ConsideraHistorico, filtro.TemAbrangenciaUeOuDreOuSme));
                
            var tipoNota = await TipoNotaPorAvaliacao(atividadeAvaliativa, filtro.Usuario, abrangenciaTurma, abrangenciaTurma != null);
            
            var ehTipoNota = tipoNota.TipoNota == TipoNota.Nota;
            
            var notaParametro = await repositorioNotaParametro.ObterPorDataAvaliacao(atividadeAvaliativa.DataAvaliacao);
            
            var quantidadeAlunos = filtro.NotasPorAvaliacao.Count();
            
            var quantidadeAlunosSuficientes = 0;

            var periodosEscolares = await BuscarPeriodosEscolaresDaAtividade(atividadeAvaliativa);
            var periodoAtividade = periodosEscolares.FirstOrDefault(x => x.PeriodoInicio.Date <= atividadeAvaliativa.DataAvaliacao.Date && x.PeriodoFim.Date >= atividadeAvaliativa.DataAvaliacao.Date);

            foreach (var nota in filtro.NotasPorAvaliacao)
            {
                var valorConceito = ehTipoNota ? valoresConceito.FirstOrDefault(a => a.Id == nota.ConceitoId) : null;

                quantidadeAlunosSuficientes += ehTipoNota ? nota.Nota >= notaParametro.Media ? 1 : 0 
                                                          : valorConceito != null && valorConceito.Aprovado ? 1 : 0;
            }
            string mensagemNotasConceitos = $"<p>Os resultados da atividade avaliativa '{atividadeAvaliativa.NomeAvaliacao}' da turma {abrangenciaTurma.NomeTurma} da {abrangenciaTurma.NomeUe} (DRE {abrangenciaTurma.NomeDre}) no bimestre {periodoAtividade.Bimestre} de {abrangenciaTurma.AnoLetivo} foram alterados " +
          $"pelo Professor {filtro.Usuario.Nome} ({filtro.Usuario.CodigoRf}) em {dataAtual.ToString("dd/MM/yyyy")} às {dataAtual.ToString("HH:mm")} estão abaixo da média.</p>" +
          $"<a href='{filtro.HostAplicacao}diario-classe/notas/{filtro.DisciplinaId}/{periodoAtividade.Bimestre}'>Clique aqui para visualizar os detalhes.</a>";

            // Avalia se a quantidade de alunos com nota/conceito suficientes esta abaixo do percentual parametrizado para notificação
            if (quantidadeAlunosSuficientes < (quantidadeAlunos * filtro.PercentualAlunosInsuficientes / 100))
            {
                var usuariosCPs = await ObterUsuariosCPs(abrangenciaTurma.CodigoUe);

                foreach (var usuarioCP in usuariosCPs)
                    await mediator.Send(new NotificarUsuarioCommand(
                        $"Alteração em Atividade Avaliativa - Turma {abrangenciaTurma.NomeTurma}",
                        mensagemNotasConceitos,
                        usuarioCP.CodigoRf,
                        NotificacaoCategoria.Alerta,
                        NotificacaoTipo.Notas,
                        atividadeAvaliativa.DreId,
                        atividadeAvaliativa.UeId,
                        atividadeAvaliativa.TurmaId,
                        DateTimeExtension.HorarioBrasilia().Year,
                        usuarioId: usuarioCP.Id ));
            }

            return true;
        }

        private async Task<IEnumerable<Usuario>> ObterUsuariosCPs(string codigoUe)
        {
            var usuariosCPs = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.CP));

            return await CarregaUsuariosPorRFs(usuariosCPs);
        }

        private async Task<IEnumerable<Usuario>> CarregaUsuariosPorRFs(IEnumerable<FuncionarioDTO> listaCPsUe)
        {
            var usuarios = new List<Usuario>();
            foreach (var cpUe in listaCPsUe)
                usuarios.Add(await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(cpUe.CodigoRF));
            return usuarios;
        }

        private async Task<NotaTipoValor> TipoNotaPorAvaliacao(AtividadeAvaliativa atividadeAvaliativa, Usuario usuario, AbrangenciaFiltroRetorno abrangenciaTurma, bool consideraHistorico = false)
        {
            if (abrangenciaTurma.TipoTurma == TipoTurma.EdFisica)
                return repositorioNotaTipoValor.ObterPorTurmaId(Convert.ToInt64(atividadeAvaliativa.TurmaId), TipoTurma.EdFisica);

            var notaTipo = await ObterNotaTipo(abrangenciaTurma, atividadeAvaliativa.DataAvaliacao, usuario, consideraHistorico);

            if (notaTipo == null)
                throw new NegocioException(MensagemNegocioNota.TIPO_NOTA_NAO_ENCONTRADO);

            return notaTipo;
        }

        private async Task<IEnumerable<PeriodoEscolar>> BuscarPeriodosEscolaresDaAtividade(AtividadeAvaliativa atividadeAvaliativa)
        {
            var dataFinal = atividadeAvaliativa.DataAvaliacao.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var aula = await repositorioAula.ObterAulaIntervaloTurmaDisciplina(atividadeAvaliativa.DataAvaliacao, dataFinal, atividadeAvaliativa.TurmaId, atividadeAvaliativa.Id);

            if (aula == null)
                throw new NegocioException($"Não encontrada aula para a atividade avaliativa '{atividadeAvaliativa.NomeAvaliacao}' no dia {atividadeAvaliativa.DataAvaliacao.Date.ToString("dd/MM/yyyy")}");

            IEnumerable<PeriodoEscolar> periodosEscolares = await repositorioPeriodoEscolar.ObterPorTipoCalendario(aula.TipoCalendarioId);
            return periodosEscolares;
        }

        public async Task<NotaTipoValor> ObterNotaTipo(AbrangenciaFiltroRetorno abrangenciaFiltroRetorno, DateTime data, Usuario usuario, bool consideraHistorico = false)
        {
            var anoCicloModalidade = !String.IsNullOrEmpty(abrangenciaFiltroRetorno?.Ano) 
                ? abrangenciaFiltroRetorno.Ano == AnoCiclo.Alfabetizacao.Name() 
                    ? AnoCiclo.Alfabetizacao.Description() : abrangenciaFiltroRetorno.Ano : string.Empty;
            
            var ciclo = await repositorioCiclo.ObterCicloPorAnoModalidade(anoCicloModalidade, abrangenciaFiltroRetorno.Modalidade);

            if (ciclo == null)
                throw new NegocioException(MensagemNegocioTurma.CICLO_TURMA_NAO_ENCONTRADO);

            return await repositorioNotaTipoValor.ObterPorCicloIdDataAvalicacao(ciclo.Id, data);
        }
    }
}
