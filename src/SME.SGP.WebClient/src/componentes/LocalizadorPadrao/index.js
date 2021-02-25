import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Componentes
import { AutoComplete, Input } from 'antd';
import { Label, Loader } from '~/componentes';

// Styles
import { api } from '~/servicos';
import { InputBuscaEstilo } from './styles';

function LocalizadorPadrao({
  labelNome,
  onChange,
  valorSelecionado,
  desabilitado,
  placeholder,
  url,
  campoValor,
  campoDescricao,
}) {
  const [sugestoes, setSugestoes] = useState([]);
  const [valor, setValor] = useState('');
  const [dataSource, setDataSource] = useState([]);
  const [timeoutBusca, setTimeoutBusca] = useState('');
  const [carregando, setCarregando] = useState(false);

  const onChangeValor = busca => {
    if (busca?.length) setValor(busca);
    else {
      setValor();
      onChange();
      setDataSource([]);
    }
  };

  const onSearchValor = async selecionado => {
    const dados = await api.get(`${url}?nome=${selecionado}`);
    if (dados.status === 200 && dados?.data) {
      setDataSource(dados.data);
      setCarregando(false);
    } else {
      setCarregando(false);
    }
  };

  const validaAntesBuscar = busca => {
    setValor(busca);
    if (timeoutBusca) {
      clearTimeout(timeoutBusca);
    }

    if (url && busca?.length >= 3) {
      const timeout = setTimeout(() => {
        setCarregando(true);
        onSearchValor(busca);
      }, 800);
      setTimeoutBusca(timeout);
    }
  };

  const onSelect = itemSelecionado => {
    const keys = Object.keys(itemSelecionado.props);
    if (keys?.length) {
      const selecionado = {};
      selecionado[campoValor] = itemSelecionado.props.value;
      selecionado[campoDescricao] = itemSelecionado.props.children;
      onChange(selecionado);
    }
  };

  useEffect(() => {
    setSugestoes(dataSource);
  }, [dataSource]);

  useEffect(() => {
    setValor(valorSelecionado);
  }, [valorSelecionado]);

  const options =
    sugestoes &&
    sugestoes.map(item => (
      <AutoComplete.Option
        key={`lp-${item[campoValor]}`}
        value={item[campoValor]}
      >
        {item[campoDescricao]}
      </AutoComplete.Option>
    ));

  return (
    <div>
      {labelNome && <Label text={labelNome} control="descricao" />}
      <Loader loading={carregando}>
        <InputBuscaEstilo>
          <AutoComplete
            onChange={e => onChangeValor(e)}
            onSearch={e => validaAntesBuscar(e)}
            onSelect={(value, option) => onSelect(option)}
            dataSource={options}
            value={valor}
            disabled={desabilitado}
            allowClear
          >
            <Input
              placeholder={placeholder}
              prefix={<i className="fa fa-search fa-lg" />}
              disabled={desabilitado}
              allowClear
            />
          </AutoComplete>
        </InputBuscaEstilo>
      </Loader>
    </div>
  );
}

LocalizadorPadrao.propTypes = {
  labelNome: PropTypes.string,
  valorSelecionado: PropTypes.oneOfType([
    PropTypes.objectOf(PropTypes.object),
    PropTypes.any,
  ]),
  onChange: PropTypes.func,
  desabilitado: PropTypes.bool,
  placeholder: PropTypes.string,
  url: PropTypes.string,
  campoValor: PropTypes.string,
  campoDescricao: PropTypes.string,
};

LocalizadorPadrao.defaultProps = {
  labelNome: '',
  valorSelecionado: '',
  onChange: () => {},
  desabilitado: false,
  url: '',
  campoValor: 'valor',
  campoDescricao: 'descricao',
  placeholder: '',
};

export default LocalizadorPadrao;
