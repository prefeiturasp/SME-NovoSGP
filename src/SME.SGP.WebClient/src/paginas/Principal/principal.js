import React from 'react';
import Card from '../../componentes/card';
import Grid from '../../componentes/grid';
import CardLink from '../../componentes/cardlink';

const Principal = (props) => {

  return (
    <>
      <Card>
        <Grid cols={12}>
          <h5><span className="fas fa-thumbtack"></span>Notificações</h5>
        </Grid>
      </Card>
      <Grid cols={12}>
        <CardLink cols={[4,4,4,12]} className="p-r-5"/>
        <CardLink cols={[4,4,4,12]} />
        <CardLink cols={[4,4,4,12]} />
      </Grid>
    </>
  );
}

export default Principal;
