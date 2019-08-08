import React from 'react';
<<<<<<< HEAD:src/SME.SGP.WebClient/src/pages/Principal/principal.js
import history from '../../services/history';
=======
import history from '../../servicos/history';

>>>>>>> d40a400cfdd6613c734f1a21c83f4230fea1bfbd:src/SME.SGP.WebClient/src/paginas/Principal/principal.js
// import { Container } from './styles';

export default function Principal() {
  function irPlanoCiclo() {
    history.push('/planejamento/plano-ciclo');
  }

  return (
    <React.Fragment>
      <h1>Principal, Home ou Main</h1>
      <button type="button" className="btn btn-info" onClick={irPlanoCiclo}>
        Ir para Plano de Ciclo
      </button>
    </React.Fragment>
  );
}
