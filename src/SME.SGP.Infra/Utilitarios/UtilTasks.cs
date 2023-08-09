﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

//o ideal é ter algo um pouquinho melhor do que o default da plataforma que vai acabar lancando apenas uma excecao de tasks executando
//pode ou criar como method extensions ou encapsular em alguma classe estatica mesmo como essa
namespace SME.SGP.Infra.Utilitarios
{
    public static class UtilTasks
    {
        public static async Task WhenAll(params Task[] tasks)
        {
            var allTasks = Task.WhenAll(tasks);
            try
            {
                await allTasks;
                return;
            }
            catch
            {
                //Just ignore because if one task fails, all will fail with single exception message even
                //another task fails too
            }
            throw allTasks.Exception ?? throw new Exception("Unreachable code");
        }
    }
}