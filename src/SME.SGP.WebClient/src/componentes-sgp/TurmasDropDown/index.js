import React, { useEffect, useState } from 'react';
import t from 'prop-types';

// Componentes
import { SelectComponent } from '~/componentes';

// Serviços
import AbrangenciaServico from '~/servicos/Abrangencia';

function TurmasDropDown({ form, onChange, label, ueId, modalidadeId, valor }) {
  const [listaTurmas, setListaTurmas] = useState([]);

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
    debugger;
    if (ueId && modalidadeId) {
      buscaTurmas();
    } else {
      setListaTurmas([]);
    }
  }, [ueId, modalidadeId]);

  useEffect(() => {
    if (listaTurmas.length === 1 && form) {
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
      valueSelect={valor}
      // disabled={listaTurmas.length === 0 || listaTurmas.length === 1}
    />
  );
}

TurmasDropDown.propTypes = {
  onChange: t.func,
  form: t.oneOfType([t.objectOf(t.object), t.any]),
  label: t.string,
  ueId: t.string,
  modalidadeId: t.string,
};

TurmasDropDown.defaultProps = {
  onChange: () => {},
  form: null,
  label: null,
  ueId: null,
  modalidadeId: null,
};

export default TurmasDropDown;
