import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Componentes
import InputRF from './componentes/InputRF';
import InputNome from './componentes/InputNome';
import { Grid, Label } from '~/componentes';

// Services
import service from './services/LocalizadorService';

function Localizador({ onChange, showLabel, form }) {
  const [dataSource, setDataSource] = useState([]);
  const [pessoaSelecionada, setPessoaSelecionada] = useState({});

  const onChangeInput = async valor => {
    const { dados } = await service.buscarPessoasMock({ nome: valor });
    setDataSource(dados);
  };

  const onBuscarPorRF = async ({ rf }) => {
    const { dados } = await service.buscarPessoasMock({ rf });
    if (dados.length <= 0) return;
    setPessoaSelecionada(dados[0]);
  };

  const onSelectPessoa = objeto => {
    setPessoaSelecionada({
      rf: parseInt(objeto.key, 10),
      nome: objeto.props.value,
    });
  };

  useEffect(() => {
    onChange(pessoaSelecionada);
  }, [pessoaSelecionada]);

  return (
    <>
      <Grid cols={4}>
        {showLabel && (
          <Label text="Registro Funcional (RF)" control="professorRf" />
        )}
        <InputRF
          pessoaSelecionada={pessoaSelecionada}
          onSelect={onBuscarPorRF}
          name="professorRf"
          form={form}
        />
      </Grid>
      <Grid cols={8}>
        {showLabel && <Label text="Nome" control="professorNome" />}
        <InputNome
          dataSource={dataSource}
          onSelect={onSelectPessoa}
          onChange={onChangeInput}
          pessoaSelecionada={pessoaSelecionada}
          name="professorNome"
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
