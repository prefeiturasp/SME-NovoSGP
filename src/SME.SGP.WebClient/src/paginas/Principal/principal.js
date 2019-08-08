import React from 'react';
import history from '../../servicos/history';

// import { Container } from './styles';

export default function Principal() {
  function irPlanoCiclo() {
    history.push('/planejamento/plano-ciclo');
  }

  return (
    <>
      <h1>Principal, Home ou Main</h1>
      <button type="button" className="btn btn-info" onClick={irPlanoCiclo}>
        Ir para Plano de Ciclo
      </button>
    </>
  );
}
