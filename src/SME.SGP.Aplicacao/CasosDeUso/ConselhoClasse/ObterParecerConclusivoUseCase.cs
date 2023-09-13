using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Commands;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.ConselhoClasse;

namespace SME.SGP.Aplicacao
{
    public class ObterParecerConclusivoUseCase : IObterParecerConclusivoUseCase
    {
        private readonly IMediator mediator;
        private readonly IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAlunoConsulta;

        public ObterParecerConclusivoUseCase(IMediator mediator,IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAlunoConsulta)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioConselhoClasseAlunoConsulta = repositorioConselhoClasseAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAlunoConsulta));
        }

        public async Task<ParecerConclusivoDto> Executar(ConselhoClasseParecerConclusivoConsultaDto parecerConclusivoConsultaDto)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(parecerConclusivoConsultaDto.CodigoTurma));
            if (turma.EhNulo())
                throw new NegocioException(MensagemNegocioTurma.NAO_FOI_POSSIVEL_OBTER_DADOS_TURMA);
            
            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(parecerConclusivoConsultaDto.FechamentoTurmaId, parecerConclusivoConsultaDto.AlunoCodigo, turma.AnoLetivo != DateTime.Now.Year));
            
            if (fechamentoTurma.EhNulo())
            {
                if (!turma.EhAnoAnterior())
                    throw new NegocioException(MensagemNegocioFechamentoTurma.NAO_EXISTE_FECHAMENTO_TURMA);
            }
            else
                turma = fechamentoTurma.Turma;
            
            
            if (turma.AnoLetivo != 2020 && !turma.EhAnoAnterior() && !await mediator.Send(new ExisteConselhoClasseUltimoBimestreQuery(turma, parecerConclusivoConsultaDto.AlunoCodigo)))
                return new ParecerConclusivoDto() { EmAprovacao = false };
            
            var conselhoClasseAluno = await repositorioConselhoClasseAlunoConsulta.ObterPorConselhoClasseAlunoCodigoAsync(parecerConclusivoConsultaDto.ConselhoClasseId, parecerConclusivoConsultaDto.AlunoCodigo);
            if (!turma.EhAnoAnterior() && (conselhoClasseAluno.EhNulo() || !conselhoClasseAluno.ConselhoClasseParecerId.HasValue) && fechamentoTurma.PeriodoEscolarId.EhNulo())
                return await mediator.Send(new GerarParecerConclusivoPorConselhoFechamentoAlunoCommand(parecerConclusivoConsultaDto.ConselhoClasseId, parecerConclusivoConsultaDto.FechamentoTurmaId, parecerConclusivoConsultaDto.AlunoCodigo));
            
            var parecerConclusivoDto = new ParecerConclusivoDto()
            {
                Id = (conselhoClasseAluno?.ConselhoClasseParecerId).HasValue ? conselhoClasseAluno.ConselhoClasseParecerId.Value : 0,
                Nome = conselhoClasseAluno?.ConselhoClasseParecer?.Nome,
                EmAprovacao = false
            };
            
            await VerificaEmAprovacaoParecerConclusivo(conselhoClasseAluno?.Id, parecerConclusivoDto);
            
            return parecerConclusivoDto;
        }
        
        private async Task VerificaEmAprovacaoParecerConclusivo(long? conselhoClasseAlunoId, ParecerConclusivoDto parecerConclusivoDto)
        {
            if (conselhoClasseAlunoId.NaoEhNulo() && conselhoClasseAlunoId > 0)
            {
                var wfAprovacaoParecerConclusivo = await mediator.Send(new ObterSePossuiParecerEmAprovacaoQuery(conselhoClasseAlunoId));

                if (wfAprovacaoParecerConclusivo.NaoEhNulo())
                {
                    parecerConclusivoDto.Id = wfAprovacaoParecerConclusivo.ConselhoClasseParecerId.Value;
                    parecerConclusivoDto.Nome = wfAprovacaoParecerConclusivo.ConselhoClasseParecer.Nome;
                    parecerConclusivoDto.EmAprovacao = true;
                }
            }
        }	
    }
}