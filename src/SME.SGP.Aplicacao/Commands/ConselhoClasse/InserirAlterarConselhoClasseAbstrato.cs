using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public abstract class InserirAlterarConselhoClasseAbstrato
    {
        protected readonly IMediator mediator;
        protected readonly IRepositorioConselhoClasseNota repositorioConselhoClasseNota;

        protected InserirAlterarConselhoClasseAbstrato(
                            IMediator mediator,
                            IRepositorioConselhoClasseNota repositorioConselhoClasseNota)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));
        }

        protected ConselhoClasseNota ObterConselhoClasseNota(ConselhoClasseNotaDto conselhoClasseNotaDto, long conselhoClasseAlunoId)
        {
            var conselhoClasseNota = new ConselhoClasseNota()
            {
                ConselhoClasseAlunoId = conselhoClasseAlunoId,
                ComponenteCurricularCodigo = conselhoClasseNotaDto.CodigoComponenteCurricular,
                Justificativa = conselhoClasseNotaDto.Justificativa,
            };
            
            if (conselhoClasseNotaDto.Nota.HasValue)
                conselhoClasseNota.Nota = conselhoClasseNotaDto.Nota.Value;
            
            if (conselhoClasseNotaDto.Conceito.HasValue)
                conselhoClasseNota.ConceitoId = conselhoClasseNotaDto.Conceito.Value;

            return conselhoClasseNota;
        }

        protected void ValidarNotasFechamentoConselhoClasse2020(ConselhoClasseNota conselhoClasseNota)
        {
            if (conselhoClasseNota.ConceitoId.HasValue && conselhoClasseNota.ConceitoId.Value == 3)
                throw new NegocioException("Não é possível atribuir conceito NS (Não Satisfatório) pois em 2020 não há retenção dos estudantes conforme o Art 5º da LEI Nº 17.437 DE 12 DE AGOSTO DE 2020.");
            else if (conselhoClasseNota.Nota < 5)
                throw new NegocioException("Não é possível atribuir uma nota menor que 5 pois em 2020 não há retenção dos estudantes conforme o Art 5º da LEI Nº 17.437 DE 12 DE AGOSTO DE 2020.");
        }

        protected async Task<bool> EnviarParaAprovacao(Turma turma, Usuario usuarioLogado)
        {
           return turma.AnoLetivo < DateTime.Today.Year
                && !usuarioLogado.EhGestorEscolar()
                && await ParametroAprovacaoAtivo(turma.AnoLetivo);
        }

        protected async Task GerarWFAprovacao(
                                ConselhoClasseNota conselhoClasseNota, 
                                Turma turma, 
                                int? bimestre, 
                                Usuario usuarioLogado, 
                                string alunoCodigo, 
                                double? notaAnterior, 
                                long? conceitoIdAnterior)
        {
            if (conselhoClasseNota.Id == 0)
            {
                conselhoClasseNota.Nota = null;
                conselhoClasseNota.ConceitoId = null;

                await repositorioConselhoClasseNota.SalvarAsync(conselhoClasseNota);
            }

            await mediator.Send(new GerarWFAprovacaoNotaConselhoClasseCommand(conselhoClasseNota,
                                                                              turma,
                                                                              bimestre,
                                                                              usuarioLogado,
                                                                              alunoCodigo,
                                                                              notaAnterior,
                                                                              conceitoIdAnterior));
        }

        protected int? ObterBimestre(int? bimestre)
        {
            if (bimestre.HasValue && bimestre.Value > 0)
                return bimestre;

            return null;
        }
        protected async Task MoverJustificativaConselhoClasseNota(ConselhoClasseNotaDto conselhoClasseNotaDto, string justificativaObj)
        {
            if (!string.IsNullOrEmpty(conselhoClasseNotaDto.Justificativa))
            {
                conselhoClasseNotaDto.Justificativa = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.Editor, justificativaObj, conselhoClasseNotaDto.Justificativa));
            }
            if (!string.IsNullOrEmpty(justificativaObj))
            {
                await mediator.Send(new RemoverArquivosExcluidosCommand(justificativaObj, conselhoClasseNotaDto.Justificativa, TipoArquivo.Editor.Name()));
            }
        }

        private async Task<bool> ParametroAprovacaoAtivo(int anoLetivo)
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.AprovacaoAlteracaoNotaConselho, anoLetivo));
            if (parametro.EhNulo())
                throw new NegocioException($"Não foi possível localizar o parametro 'AprovacaoAlteracaoNotaConselho' para o ano {anoLetivo}");

            return parametro.Ativo;
        }
    }
}
