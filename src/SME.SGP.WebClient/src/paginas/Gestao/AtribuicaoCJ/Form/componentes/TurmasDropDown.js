import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Componentes
import { SelectComponent, Loader } from '~/componentes';

// Servicos
import AtribuicaoCJServico from '~/servicos/Paginas/AtribuicaoCJ';

import FiltroHelper from '~/componentes-sgp/filtro/helper';
import { erros } from '~/servicos/alertas';

function TurmasDropDown({ form, onChange, label }) {
  const [listaTurmas, setListaTurmas] = useState([]);
  const [carregandoLista, setCarregandoLista] = useState(false);

  const { ueId, modalidadeId } = form.values;
  useEffect(() => {
    async function buscaTurmas() {
      setCarregandoLista(true);
      const { data } = await AtribuicaoCJServico.buscarTurmas(
        ueId,
        modalidadeId
      ).catch(e => {
        erros(e);
        setCarregandoLista(false);
      });
      if (data) {
        setListaTurmas(
          data
            .map(item => ({
              desc: item.nome,
              valor: item.codigo,
            }))
            .sort(FiltroHelper.ordenarLista('desc'))
        );
      }
      setCarregandoLista(false);
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
    <Loader loading={carregandoLista} tip="">
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
    </Loader>
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
