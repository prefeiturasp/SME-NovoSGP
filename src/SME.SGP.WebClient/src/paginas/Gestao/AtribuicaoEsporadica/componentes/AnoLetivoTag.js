import React, { useEffect } from 'react';
import t from 'prop-types';

// Redux
import { useSelector } from 'react-redux';

// Componentes
import { Tag, Label } from '~/componentes';

function AnoLetivoTag({ form, label }) {
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

  const anoAtual = window.moment().format('YYYY');

  return (
    <>
      {label && <Label text={label} />}
      <Tag tamanho="grande" fluido centralizado>
        {anoAtual}
      </Tag>
    </>
  );
}

AnoLetivoTag.propTypes = {
  form: t.oneOfType([t.object, t.func]),
  label: t.string,
};

AnoLetivoTag.defaultProps = {
  form: null,
  label: null,
};

export default AnoLetivoTag;
