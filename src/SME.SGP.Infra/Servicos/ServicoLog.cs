﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ServicoLog : IServicoLog
    {
        public ServicoLog(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
        }

        public void Registrar(string mensagem)
        {

        }

        public void Registrar(Exception ex)
        {

        }
    }
}