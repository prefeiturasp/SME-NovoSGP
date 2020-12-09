import React, { useCallback,useEffect,useState } from 'react';
import PropTypes from 'prop-types';

import FiltroHelper from '~componentes-sgp/filtro/helper';

// Redux
import { useSelector } from 'react-redux';

// Componentes
import { SelectComponent, Loader } from '~/componentes';

function AnoLetivoDropDown({ form, onChange, consideraHistorico }) {
  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);  
  const [carregando, setCarregando] = useState(false);

  const obterAnosLetivos = useCallback(async () => {    
    setCarregando(true);
    const anosLetivo = await FiltroHelper.obterAnosLetivos({
      consideraHistorico: consideraHistorico
    });    
    
    setListaAnosLetivo(anosLetivo);           
    setCarregando(false);
  }, [consideraHistorico]);

  useEffect(() => {
    obterAnosLetivos();
  }, [obterAnosLetivos]);

  useEffect(() => {    
    if (listaAnosLetivo && listaAnosLetivo.length > 0) {
      form.setFieldValue('anoLetivo', String(listaAnosLetivo[0].valor), false);
      onChange(String(listaAnosLetivo[0].valor));
    }
    else{
      form.setFieldValue('anoLetivo', '', false);
      onChange(String(''));
    }

  }, [listaAnosLetivo]);

  return (   
    <>
      <Loader loading={carregando} tip="">      
        <SelectComponent
        label="Ano Letivo"
        valueOption="valor"
        valueText="desc"
        form={form}
        name="anoLetivo"
        lista={listaAnosLetivo}
        placeholder="Ano Letivo"
        onChange={onChange}
        disabled={listaAnosLetivo && listaAnosLetivo.length === 1}      
        />
      </Loader>
    </> 
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
