import React, { useState, useEffect } from 'react';
import t from 'prop-types';

// Componentes
import { SelectComponent } from '~/componentes';

// Servicos
import AtribuicaoCJServico from '~/servicos/Paginas/AtribuicaoCJ';

// Funções
import { valorNuloOuVazio } from '~/utils/funcoes/gerais';

function ModalidadesDropDown({ label, form, onChange, disabled }) {
  const [listaModalidades, setListaModalidades] = useState([]);

  useEffect(() => {
    async function buscarModalidades() {
      const { data } = await AtribuicaoCJServico.buscarModalidades(
        form.values.ueId
      );
      if (data) {
        setListaModalidades(
          data.map(item => ({
            desc: item.nome,
            valor: String(item.id),
          }))
        );
      }
    }
    if (!form.values.ueId) return;
    buscarModalidades();
  }, [form.values.ueId]);

  useEffect(() => {
    if (listaModalidades.length === 1) {
      form.setFieldValue('modalidadeId', listaModalidades[0].valor);
      onChange(listaModalidades[0].valor);
    }
  }, [listaModalidades]);

  useEffect(() => {
    onChange();
    if (!valorNuloOuVazio(form.values.modalidadeId)) {
      onChange(form.values.modalidadeId);
    }
  }, [form.values.modalidadeId]);

  return (
    <SelectComponent
      label={!label ? null : label}
      form={form}
      name="modalidadeId"
      className="fonte-14"
      onChange={onChange}
      lista={listaModalidades}
      valueOption="valor"
      valueText="desc"
      placeholder="Modalidade"
      disabled={disabled}
    />
  );
}

ModalidadesDropDown.propTypes = {
  form: t.oneOfType([t.objectOf(t.object), t.any]),
  onChange: t.func,
  label: t.string,
  disabled: t.bool,
};

ModalidadesDropDown.defaultProps = {
  form: {},
  onChange: () => {},
  label: null,
  disabled: false,
};

export default ModalidadesDropDown;
