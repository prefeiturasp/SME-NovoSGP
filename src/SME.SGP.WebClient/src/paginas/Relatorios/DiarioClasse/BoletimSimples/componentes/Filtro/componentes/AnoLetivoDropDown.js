import React, { useCallback,useEffect,useState } from 'react';
import PropTypes from 'prop-types';

import FiltroHelper from '~componentes-sgp/filtro/helper';

// Redux
import { useSelector } from 'react-redux';

// Componentes
import { SelectComponent } from '~/componentes';

function AnoLetivoDropDown({ form, onChange, consideraHistorico }) {

  const anosLetivos = useSelector(store => store.filtro.anosLetivos);
  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);  

  const obterAnosLetivos = useCallback(async () => {    
    let anosLetivo = [];

    const anosLetivoComHistorico = await FiltroHelper.obterAnosLetivos({
      consideraHistorico: true,
    });
    const anosLetivoSemHistorico = await FiltroHelper.obterAnosLetivos({
      consideraHistorico: false,
    });

    anosLetivo = anosLetivoComHistorico.concat(anosLetivoSemHistorico);
    
    setListaAnosLetivo(anosLetivo);           
  }, []);

  useEffect(() => {
    obterAnosLetivos();
  }, [obterAnosLetivos]);

  useEffect(() => {
    if (anosLetivos.length === 1) {
      form.setFieldValue('anoLetivo', String(anosLetivos[0].valor), false);
      onChange(String(anosLetivos[0].valor));
    }
  }, [anosLetivos]);

  return (
    <SelectComponent
      label="Ano Letivo"
      valueOption="valor"
      valueText="desc"
      form={form}
      name="anoLetivo"
      lista={consideraHistorico ? listaAnosLetivo : anosLetivos}
      placeholder="Ano Letivo"
      onChange={onChange}
      disabled={anosLetivos && anosLetivos.length === 1 && !consideraHistorico}      
    />
  );
}

AnoLetivoDropDown.propTypes = {
  form: PropTypes.oneOfType([
    PropTypes.objectOf(PropTypes.object),
    PropTypes.any,
  ]),
  onChange: PropTypes.func,
};

AnoLetivoDropDown.defaultProps = {
  form: {},
  onChange: () => null,
  consideraHistorico: false,
};

export default AnoLetivoDropDown;
