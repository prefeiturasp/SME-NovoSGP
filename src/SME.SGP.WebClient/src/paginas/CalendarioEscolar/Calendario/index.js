import React from 'react';
import Card from '~/componentes/card';
import Grid from '~/componentes/grid';
import Calendario from '~/componentes-sgp/Calendario/Calendario';

const CalendarioEscolar = () => {
  return (
    <Card className="rounded mb-4">
      <Grid cols={12}>
        <Calendario />
      </Grid>
    </Card>
  );
};

export default CalendarioEscolar;
