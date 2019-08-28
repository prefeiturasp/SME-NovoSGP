import React from 'react';
import Card from '../../componentes/card';
import Grid from '../../componentes/grid';

const Principal = (props) => {

  return (
    <Card>
      <Grid cols="12">
        <h5><span className="fas fa-thumbtack"></span>Notificações</h5>
      </Grid>
    </Card>
  );
}

export default Principal;
