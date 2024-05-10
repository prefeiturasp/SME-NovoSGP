import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Componentes
import { SelectComponent } from '~/componentes';

// Servicos
import AbrangenciaServico from '~/servicos/Abrangencia';

function TurmasDropDown({ form, onChange, label }) {
  const [listaTurmas, setListaTurmas] = useState([]);

  const { ueId, modalidadeId } = form.values;
  useEffect(() => {
    async function buscaTurmas() {
      const { data } = await AbrangenciaServico.buscarTurmas(
        ueId,
        modalidadeId
      );
      if (data) {
        setListaTurmas(
          data.map(item => ({
            desc: item.nome,
            valor: item.codigo,
          }))
        );
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
};

TurmasDropDown.defaultProps = {
  form: {},
  onChange: () => {},
  label: null,
};

export default TurmasDropDown;
