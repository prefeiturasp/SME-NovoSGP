import React from 'react';
import { Switch, Route } from 'react-router-dom';

import Principal from '../paginas/Principal/principal';
import PlanoCiclo from '../paginas/Planejamento/PlanoCiclo/planoCiclo';
import PlanoAnual from '../paginas/Planejamento/PlanoAnual/planoAnual';

export default function Rotas() {
  return (
    <Switch>
      <Route path="/" exact component={Principal} />
      <Route path="/planejamento/plano-ciclo" exact={true} component={PlanoCiclo} />
      <Route path="/planejamento/plano-ciclo/:ano/:cicloId/:escolaId" exact={true} component={PlanoCiclo} />
      <Route path="/planejamento/plano-anual" component={PlanoAnual} />
    </Switch>
  );
}
