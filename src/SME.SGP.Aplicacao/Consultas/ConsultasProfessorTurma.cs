﻿using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasProfessor : AbstractUseCase, IConsultasProfessor
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IServicoEol servicoEOL;

        public ConsultasProfessor(IServicoEol servicoEOL, IRepositorioCache repositorioCache, IMediator mediator) : base(mediator)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
        }

        public IEnumerable<ProfessorTurmaDto> Listar(string codigoRf)
        {
            return MapearParaDto(servicoEOL.ObterListaTurmasPorProfessor(codigoRf));
        }

        public async Task<IEnumerable<ProfessorResumoDto>> ObterResumoAutoComplete(int anoLetivo, string dreId, string ueId, string nomeProfessor)
        {
            if (String.IsNullOrEmpty(nomeProfessor) && nomeProfessor.Length < 2)
                return null;
            var retornoProfessores = await servicoEOL.ObterProfessoresAutoComplete(anoLetivo, dreId, ueId, nomeProfessor);
            for (int i = 0; i < retornoProfessores.Count(); i++)
            {
                var professorSgp = await ObterProfessorSGPConsultaPorNome(retornoProfessores.ToList()[i].CodigoRF);
                if (professorSgp != null)
                    retornoProfessores.ToList()[i].UsuarioId = professorSgp.Id;
            }
            return retornoProfessores.Where(x => x.UsuarioId > 0);
        }

        public async Task<IEnumerable<ProfessorResumoDto>> ObterResumoAutoComplete(int anoLetivo, string dreId, string nomeProfessor, bool incluirEmei)
        {
            if (nomeProfessor.Length < 2)
                return null;

            return await servicoEOL.ObterProfessoresAutoComplete(anoLetivo, dreId, nomeProfessor, incluirEmei);
        }

        public async Task<ProfessorResumoDto> ObterResumoPorRFAnoLetivo(string codigoRF, int anoLetivo, bool buscarOutrosCargos = false)
        {
            var professorResumo = await ObterProfessorEOL(codigoRF, anoLetivo, buscarOutrosCargos);
            var professorSgp = await ObterProfessorSGP(codigoRF);

            if (professorResumo != null)
                professorResumo.UsuarioId = professorSgp.Id;

            return professorResumo;
        }
        
        public async Task<ProfessorResumoDto> ObterResumoPorRFUeDreAnoLetivo(string codigoRF, int anoLetivo, string dreId, string ueId, bool buscarOutrosCargos = false, bool buscarPorTodasDre = false)
        {
            var professorResumo = await ObterProfessorUeRFEOL(codigoRF, anoLetivo, dreId, ueId, buscarOutrosCargos, buscarPorTodasDre);

            return professorResumo;
        }
        
        private async Task<Usuario> ObterProfessorSGP(string codigoRF)
        {
            var usuarioSgp = await mediator.Send(new ObterUsuarioPorRfQuery(codigoRF));
            if (usuarioSgp == null)
                throw new NegocioException("RF não localizado no SGP");

            return usuarioSgp;
        }

        private async Task<Usuario> ObterProfessorSGPConsultaPorNome(string codigoRF)
        {
            var usuarioSgp = await mediator.Send(new ObterUsuarioPorRfQuery(codigoRF));
            return usuarioSgp;
        }

        private async Task<ProfessorResumoDto> ObterProfessorEOL(string codigoRF, int anoLetivo, bool buscarOutrosCargos)
        {
            var professorResumo = await servicoEOL.ObterResumoProfessorPorRFAnoLetivo(codigoRF, anoLetivo, buscarOutrosCargos);
            if (professorResumo == null)
                throw new NegocioException("RF não localizado do EOL");

            return professorResumo;
        }
        private async Task<ProfessorResumoDto> ObterProfessorUeRFEOL(string codigoRF, int anoLetivo, string dreId, string ueId, bool buscarOutrosCargos = false, bool buscarPorTodasDre = false)
        {
            var professorResumo = await servicoEOL.ObterProfessorPorRFUeDreAnoLetivo(codigoRF, anoLetivo, dreId, ueId, buscarOutrosCargos, buscarPorTodasDre);
            if (professorResumo == null)
                throw new NegocioException("RF não localizado do EOL");

            return professorResumo;
        }
        public async Task<IEnumerable<TurmaDto>> ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(string rfProfessor, string codigoEscola, int anoLetivo)
        {
            //Tem patterns mais interessantes para lidar com caches como ComputeIfAbsent por exemplo usando lazy delegates e genericos e formatos
            // ex: var turmaDto = await cache.ObterAsync<IEnumerable<TurmaDto>>(chave, () => funcaoquedeveserarmazenadaemcache, CacheEntryFormat.JSON);
            // return turmaDto
            //Outro caso utilizando patterns como decorators deixando a logica de cache envolta da chamada sem poluicao
            //Ou ate mesmo usando dynamic proxies com diretive de cache especificas
            IEnumerable<TurmaDto> turmasDto = null;
            var chaveCache = $"Turmas-Professor-{rfProfessor}-ano-{anoLetivo}-escolal-{codigoEscola}";
            var disciplinasCacheString = repositorioCache.Obter(chaveCache);

            if (!string.IsNullOrWhiteSpace(disciplinasCacheString))
            {
                turmasDto = JsonConvert.DeserializeObject<IEnumerable<TurmaDto>>(disciplinasCacheString);
            }
            else
            {
                turmasDto = await servicoEOL.ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(rfProfessor, codigoEscola, anoLetivo);
                if (turmasDto != null && turmasDto.Any())
                {
                    await repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(turmasDto));
                }
            }
            return turmasDto;
        }

        private IEnumerable<ProfessorTurmaDto> MapearParaDto(IEnumerable<ProfessorTurmaReposta> turmas)
        {
            return turmas?.Select(m => new ProfessorTurmaDto()
            {
                Ano = m.Ano,
                AnoLetivo = m.AnoLetivo,
                CodDre = m.CodDre,
                CodEscola = m.CodEscola,
                CodModalidade = m.CodModalidade,
                CodTipoEscola = m.CodTipoEscola,
                CodTipoUE = m.CodTipoUE,
                CodTurma = m.CodTurma,
                Dre = m.Dre,
                DreAbrev = m.DreAbrev,
                Modalidade = m.Modalidade,
                NomeTurma = m.NomeTurma,
                TipoEscola = m.TipoEscola,
                Semestre = m.Semestre,
                TipoUE = m.TipoUE,
                Ue = m.Ue,
                UeAbrev = m.UeAbrev
            });
        }
    }
}