import React from 'react';
import { Switch, Route } from 'react-router-dom';

import Principal from '../paginas/Principal/principal';
import PlanoCiclo from '../paginas/Planejamento/PlanoCiclo/planoCiclo';
import PlanoAnual from '../paginas/Planejamento/PlanoAnual/planoAnual';
import Monica from '../paginas/Planejamento/PlanoAnual/monica';

export default function Rotas() {
  return (
    <Switch>
      <Route path="/" exact component={Principal} />
      {/* <Route
        path="/planejamento/plano-ciclo/:ano/:cicloId/:escolaId"
        exact
        component={PlanoCiclo}
      />
      <Route
        path="/planejamento/plano-ciclo/:ano"
        exact
        component={PlanoCiclo}
      /> */}
      <Route path="/planejamento/plano-ciclo" component={PlanoCiclo} />
      <Route path="/planejamento/plano-anual" component={PlanoAnual} />
      <Route path="/monica" component={Monica} />
    </Switch>
  );
}
