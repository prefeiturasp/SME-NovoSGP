import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Componentes
import InputRF from './componentes/InputRF';
import InputNome from './componentes/InputNome';
import { Grid } from '~/componentes';

// Services
import service from './services/LocalizadorService';

function Localizador({ onChange }) {
  const [dataSource, setDataSource] = useState([]);
  const [pessoaSelecionada, setPessoaSelecionada] = useState({});

  const onChangeInput = async valor => {
    const novoDataSource = await service.buscarPessoasMock({ nome: valor });
    setDataSource(novoDataSource.dados);
  };

  const onSelectPessoa = objeto => {
    setPessoaSelecionada({
      rf: parseInt(objeto.key),
      nome: objeto.props.value,
    });
  };

  useEffect(() => {
    onChange(pessoaSelecionada);
  }, [pessoaSelecionada]);

  return (
    <>
      <Grid cols={3}>
        <InputRF
          pessoaSelecionada={pessoaSelecionada}
          onSelect={onSelectPessoa}
        />
      </Grid>
      <Grid cols={9}>
        <InputNome
          dataSource={dataSource}
          onSelect={onSelectPessoa}
          onChange={onChangeInput}
          pessoaSelecionada={pessoaSelecionada}
        />
      </Grid>
    </>
  );
}

Localizador.defaultValues = {
  onChange: () => {},
};

Localizador.propTypes = {
  onChange: PropTypes.func.isRequired,
};

export default Localizador;
