import PropTypes from 'prop-types';
import React from 'react';
import styled from 'styled-components';
import { RadioGroupButton } from '~/componentes';
import Label from '~/componentes/label';

const ContainerRadioGroupButton = styled.div`
  .ant-radio-group {
    display: flex;

    .ant-radio-wrapper {
      display: flex;
      align-items: center;
    }
  }

  label {
    margin-bottom: 0;
  }
`;

const CampoDinamicoRadio = props => {
  const { questaoAtual, form, label, desabilitado, onChange } = props;

  const opcoes = questaoAtual?.opcaoResposta.map(item => {
    return {
      label: <Label text={item.nome} observacaoText={item.observacao} />,
      value: item.id,
    };
  });

  return (
    <ContainerRadioGroupButton className="col-md-12 mb-3">
      {label}
      <RadioGroupButton
        className="mt-2"
        id={String(questaoAtual?.id)}
        name={String(questaoAtual?.id)}
        form={form}
        opcoes={opcoes}
        desabilitado={desabilitado || questaoAtual.somenteLeitura}
        onChange={e => {
          const valorAtualSelecionado = e.target.value;
          onChange(valorAtualSelecionado);
        }}
      />
    </ContainerRadioGroupButton>
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
