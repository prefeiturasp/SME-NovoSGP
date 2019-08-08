import React from 'react';
import { Switch, Route } from 'react-router-dom';

import Principal from '../pages/Principal/principal';
import PlanoCiclo from '../pages/Planejamento/PlanoCiclo/planoCiclo';
import PlanoAnual from '../pages/Planejamento/PlanoAnual/planoAnual';

export default function Rotas() {
  return (
    <Switch>
      <Route path="/" exact component={Principal} />
      <Route path="/planejamento/plano-ciclo" component={PlanoCiclo} />
      <Route path="/planejamento/plano-anual" component={PlanoAnual} />
    </Switch>
  );
}
