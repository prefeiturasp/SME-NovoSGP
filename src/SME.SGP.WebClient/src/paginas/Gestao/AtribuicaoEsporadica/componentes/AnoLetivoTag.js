import React, { useEffect } from 'react';
import t from 'prop-types';

// Redux
import { useSelector } from 'react-redux';

// Componentes
import { Tag } from '~/componentes';

function AnoLetivoTag({ form }) {
  const anoLetivo = useSelector(store => {
    if (store.filtro.anosLetivos.length > 1) {
      return store.filtro.anosLetivos[0].valor;
    }
    return store.usuario.turmaSelecionada.anoLetivo;
  });

  useEffect(() => {
    if (form) {
      form.setFieldValue('anoLetivo', String(anoLetivo), false);
    }
  }, []);

  return (
    <Tag tamanho="grande" fluido centralizado>
      2019
    </Tag>
  );
}

AnoLetivoTag.propTypes = {
  form: t.oneOfType([t.object, t.func]),
};

AnoLetivoTag.defaultProps = {
  form: {},
};

export default AnoLetivoTag;
