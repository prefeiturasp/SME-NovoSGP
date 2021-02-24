import React from 'react';
import PropTypes from 'prop-types';
import { Grid, Label } from '~/componentes';

const LocalizadorPadrao = ({
  showLabel,
  labelNome,
  dataSource,
  onSelect,
  onChangeInput,
  form,
  placeholder,
  desabilitado,
}) => {
  return (
    <Grid className="pr-0" cols={8}>
      {showLabel && <Label text={labelNome} control="professorNome" />}
      <InputNome
        dataSource={dataSource}
        onSelect={onSelect}
        onChange={onChangeInput}
        pessoaSelecionada={pessoaSelecionada}
        form={form}
        name="professorNome"
        placeholderNome={placeholder}
        desabilitado={
          desabilitado ||
          validacaoDesabilitaPerfilProfessor() ||
          desabilitarCampo.nome
        }
      />
    </Grid>
  );
};

LocalizadorPadrao.propTypes = {
  showLabel: PropTypes.bool,
  labelNome: PropTypes.string,
  dataSource: PropTypes.array,
  onSelect: PropTypes.func,
  onChangeInput: PropTypes.func,
  form: PropTypes.object,
  placeholder: PropTypes.string,
  desabilitado: PropTypes.bool,
};

LocalizadorPadrao.defaultProps = {
  showLabel: false,
  labelNome: "",
  dataSource: []
  onSelect: () => {},
  onChangeInput: () => {},
  form: {},
  placeholder: "",
  desabilitado: false,
};

export default LocalizadorPadrao;
