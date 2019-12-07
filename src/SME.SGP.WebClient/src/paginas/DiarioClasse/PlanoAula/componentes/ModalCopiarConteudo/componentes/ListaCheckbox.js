import React from 'react';
import t from 'prop-types';

// Componentes
import { CheckboxComponent } from '~/componentes';

function ListaCheckbox({ onChange, valores }) {
  return (
    <>
      <CheckboxComponent
        className="mb-2"
        label="Objetivos de aprendizagem e meus objetivos (Currículo da Cidade)"
        name="objetivosAprendizagem"
        onChangeCheckbox={target => onChange(target, 'objetivosAprendizagem')}
        disabled
        checked={valores.objetivosAprendizagem}
      />
      <CheckboxComponent
        className="mb-2"
        label="Desenvolvimento da aula"
        name="desenvolvimentoAula"
        onChangeCheckbox={target => onChange(target, 'desenvolvimentoAula')}
        disabled
        checked={valores.desenvolvimentoAula}
      />
      <CheckboxComponent
        className="mb-2"
        label="Recuperação Contínua"
        name="recuperacaoContinua"
        onChangeCheckbox={target => onChange(target, 'recuperacaoContinua')}
        disabled={false}
        checked={valores.recuperacaoContinua}
      />
      <CheckboxComponent
        className="mb-2"
        label="Lição de Casa"
        name="licaoCasa"
        onChangeCheckbox={target => onChange(target, 'licaoCasa')}
        disabled={false}
        checked={valores.licaoCasa}
      />
    </>
  );
}

ListaCheckbox.propTypes = {
  onChange: t.func,
  valores: t.oneOfType([t.object]),
};

ListaCheckbox.defaultProps = {
  onChange: null,
  valores: null,
};

export default ListaCheckbox;
