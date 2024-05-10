import React, { useEffect, useState } from 'react';
import t from 'prop-types';

// Componentes
import { SelectComponent } from '~/componentes';

// Serviços
import AbrangenciaServico from '~/servicos/Abrangencia';

function TurmasDropDown({
  form,
  onChange,
  label,
  ueId,
  modalidadeId,
  valor,
  dados,
  allowClear,
}) {
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

    if (ueId && modalidadeId && !dados) {
      buscaTurmas();
    } else if (dados) {
      setListaTurmas(dados);
    } else {
      setListaTurmas([]);
    }
  }, [dados, modalidadeId, ueId]);

  useEffect(() => {
    if (listaTurmas.length === 1 && form) {
      form.setFieldValue('turmaId', listaTurmas[0].valor);
      onChange(listaTurmas[0].valor);
    }
  }, [form, listaTurmas, onChange]);

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
      disabled={form && (listaTurmas.length === 0 || listaTurmas.length === 1)}
      allowClear={allowClear}
    />
  );
}

TurmasDropDown.propTypes = {
  onChange: t.func,
  form: t.oneOfType([t.objectOf(t.object), t.any]),
  label: t.string,
  ueId: t.string,
  modalidadeId: t.string,
  valor: t.string,
  dados: t.oneOfType([t.object, t.array]),
  allowClear: t.bool,
};

TurmasDropDown.defaultProps = {
  onChange: () => {},
  form: null,
  label: null,
  ueId: null,
  modalidadeId: null,
  valor: '',
  dados: null,
  allowClear: true,
};

export default TurmasDropDown;
