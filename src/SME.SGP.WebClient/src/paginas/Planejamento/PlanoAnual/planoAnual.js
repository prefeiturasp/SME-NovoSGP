import React from 'react';
import shortid from 'shortid';
import CardCollapse from '../../../componentes/cardCollapse';
import Grid from '../../../componentes/grid';

function PlanoAnual() {
  const bimestres = [{ nome: '1º Bimestre' }, { nome: '2º Bimestre' }];

  return (
    <div className="container">
      <div className="row">
        <Grid cols="12">
          <h1>Plano Anual</h1>
          {bimestres.length > 0
            ? bimestres.map(bimestre => {
                return (
                  <CardCollapse
                    key={shortid.generate()}
                    titulo={bimestre.nome}
                    indice={shortid.generate()}
                  />
                );
              })
            : null}
        </Grid>
      </div>
    </div>
  );
}

export default PlanoAnual;
