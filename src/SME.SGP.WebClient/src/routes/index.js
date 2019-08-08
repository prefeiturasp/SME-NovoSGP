import React from 'react';
import { Switch, Route } from 'react-router-dom';

import Principal from '../pages/Principal/principal';
import PlanoCiclo from '../pages/Planejamento/PlanoCiclo/planoCiclo';

export default function Rotas() {
  return (
    <Switch>
      <Route path="/" exact component={Principal} />
      <Route path="/planejamento/plano-ciclo" component={PlanoCiclo} />
    </Switch>
  );
}
