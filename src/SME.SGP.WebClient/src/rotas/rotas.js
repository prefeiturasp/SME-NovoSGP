import React from 'react';
import { Switch, Route } from 'react-router-dom';

import Principal from '../paginas/Principal/principal';
import PlanoCiclo from '../paginas/Planejamento/PlanoCiclo/planoCiclo';
import PlanoAnual from '../paginas/Planejamento/PlanoAnual/planoAnual';
import AtribuicaoSupervisor from '../paginas/Gestao/AtribuicaoSupervisor/atribuicaoSupervisorCadastro';

export default function Rotas() {
  return (
    <Switch>
      <Route path="/:rf?" exact component={Principal} />
      <Route path="/planejamento/plano-ciclo" exact component={PlanoCiclo} />
      <Route path="/planejamento/plano-anual" component={PlanoAnual} />
      <Route
        path="/gestao/atribuicao-supervisor/:dreId/:supervisorId"
        component={AtribuicaoSupervisor}
        exact
      />
      <Route
        path="/gestao/atribuicao-supervisor"
        component={AtribuicaoSupervisor}
      />
    </Switch>
  );
}
