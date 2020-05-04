import React, { useMemo } from 'react';
import t from 'prop-types';

// Estilos
import { DiaCompletoWrapper } from './styles';

// Componentes
import { Loader } from '~/componentes';

function DiaCompleto({ dia, carregandoDia, diasPermitidos }) {
  const deveExibir = useMemo(
    () => dia && !!diasPermitidos.find(x => x.toString() === dia.toString()),
    [dia, diasPermitidos]
  );

  return (
    <DiaCompletoWrapper className={`${deveExibir && `visivel`}`}>
      <Loader loading={carregandoDia} tip="Carregando...">
        {deveExibir ? JSON.stringify(dia) : null}
      </Loader>
    </DiaCompletoWrapper>
  );
}

DiaCompleto.propTypes = {
  dia: t.oneOfType([t.any]),
  diasPermitidos: t.oneOfType([t.any]),
  carregandoDia: t.bool,
};

DiaCompleto.defaultProps = {
  dia: {},
  diasPermitidos: [],
  carregandoDia: false,
};

export default DiaCompleto;
