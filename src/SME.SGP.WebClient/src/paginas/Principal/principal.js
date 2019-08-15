import React from 'react';
import history from '../../servicos/history';

export default function Principal() {
  function irPlanoCiclo() {
    history.push('/planejamento/plano-ciclo');
  }

  return (
    <React.Fragment>
      <button type="button" className="btn btn-info" onClick={irPlanoCiclo}>
        Ir para Plano de Ciclo
      </button>
    </React.Fragment>
  );
}
