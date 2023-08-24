﻿using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery : IRequest<IEnumerable<ComponenteCurricularEol>>
    {
        public ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(string codigoTurma, string login, Guid perfil, bool realizarAgrupamentoComponente = false, bool checaMotivoDisponibilizacao = true)
        {
            CodigoTurma = codigoTurma;
            Login = login;
            Perfil = perfil;
            RealizarAgrupamentoComponente = realizarAgrupamentoComponente;
            ChecaMotivoDisponibilizacao = checaMotivoDisponibilizacao;
        }

        public string CodigoTurma { get; set; }
        public string Login { get; set; }
        public Guid Perfil { get; set; }
        public bool RealizarAgrupamentoComponente { get; set; }
        public bool ChecaMotivoDisponibilizacao { get; set; }
    }
}
