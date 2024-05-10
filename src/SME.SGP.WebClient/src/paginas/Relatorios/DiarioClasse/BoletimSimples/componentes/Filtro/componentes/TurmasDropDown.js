import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types';

// Componentes
import { SelectComponent } from '~/componentes';

// Servicos
import AbrangenciaServico from '~/servicos/Abrangencia';

function TurmasDropDown({ form, onChange, label, consideraHistorico, anoLetivo }) {  
  const [listaTurmas, setListaTurmas] = useState([]);  
  const { ueId, modalidadeId } = form.values;      

  useEffect(() => {    
    async function buscaTurmas() {      
      const { data } = await AbrangenciaServico.buscarTurmas(
        ueId,
        modalidadeId,
        '',
        anoLetivo,
        consideraHistorico,
        true
      );
      if (data) {
        const lista = data.map(item => ({
          desc: item.nome,
          valor: item.codigo,
        }));

        lista.unshift({ desc: 'Todas', valor: '0' });
        setListaTurmas(lista);
      }
    }

    if (ueId && modalidadeId) {
      buscaTurmas();
    } else {
      setListaTurmas([]);
    }
  }, [ueId, modalidadeId]);

  useEffect(() => {
    if (listaTurmas.length === 1) {
      form.setFieldValue('turmaId', listaTurmas[0].valor);
      onChange(listaTurmas[0].valor);
    }
  }, [listaTurmas]);

  return (
    <SelectComponent
      form={form}
      name="turmaId"
      className="fonte-14"
      label={!label ? null : label}
      onChange={onChange}
      lista={listaTurmas}
      valueOption="valor"
      valueText="desc"
      placeholder="Turma"
      disabled={!ueId || !modalidadeId}
    />
  );
}

TurmasDropDown.propTypes = {
  form: PropTypes.oneOfType([
    PropTypes.objectOf(PropTypes.object),
    PropTypes.any,
  ]),
  onChange: PropTypes.func,
  label: PropTypes.string,
  consideraHistorico: PropTypes.bool,
  anoLetivo: PropTypes.string
};

TurmasDropDown.defaultProps = {
  form: {},
  onChange: () => {},
  label: null,
  consideraHistorico: false,
  anoLetivo: ''
};

export default TurmasDropDown;
