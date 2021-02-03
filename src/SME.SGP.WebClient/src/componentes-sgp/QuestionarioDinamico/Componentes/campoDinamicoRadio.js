import PropTypes from 'prop-types';
import React from 'react';
import { RadioGroupButton } from '~/componentes';

const CampoDinamicoRadio = props => {
  const { questaoAtual, form, label, desabilitado, onChange } = props;

  const opcoes = questaoAtual?.opcaoResposta.map(item => {
    return { label: item.nome, value: item.id };
  });

  return (
    <div className="col-md-12 mb-3">
      {label}
      <RadioGroupButton
        id={String(questaoAtual?.id)}
        name={String(questaoAtual?.id)}
        form={form}
        opcoes={opcoes}
        desabilitado={desabilitado}
        onChange={e => {
          const valorAtualSelecionado = e.target.value;
          onChange(valorAtualSelecionado);
        }}
      />
    </div>
  );
};

CampoDinamicoRadio.propTypes = {
  questaoAtual: PropTypes.oneOfType([PropTypes.any]),
  form: PropTypes.oneOfType([PropTypes.any]),
  label: PropTypes.oneOfType([PropTypes.any]),
  desabilitado: PropTypes.bool,
  onChange: PropTypes.oneOfType([PropTypes.any]),
};

CampoDinamicoRadio.defaultProps = {
  questaoAtual: null,
  form: null,
  label: '',
  desabilitado: false,
  onChange: () => {},
};

export default CampoDinamicoRadio;
