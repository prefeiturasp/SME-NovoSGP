import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { Label } from '~/componentes';
import { erro, erros } from '~/servicos/alertas';
import { removerNumeros } from '~/utils/funcoes/gerais';
import InputCodigo from './componentes/InputCodigo';
import InputNome from './componentes/InputNome';
import ServicoLocalizadorFuncionario from './services/ServicoLocalizadorFuncionario';

const LocalizadorFuncionario = props => {
  const {
    onChange,
    desabilitado,
    codigoUe,
    codigoDre,
    codigoTurma,
    exibirCampoRf,
    valorInicial,
    placeholder,
    idPerfil,
  } = props;

  const [dataSource, setDataSource] = useState([]);
  const [funcionarioSelecionado, setFuncionarioSelecionado] = useState({});
  const [desabilitarCampo, setDesabilitarCampo] = useState({
    rf: false,
    nome: false,
  });
  const [timeoutBuscarPorCodigoNome, setTimeoutBuscarPorCodigoNome] = useState(
    ''
  );
  const [exibirLoader, setExibirLoader] = useState(false);

  useEffect(() => {
    setFuncionarioSelecionado({
      nome: '',
      rf: '',
    });
    setDataSource([]);
  }, [codigoUe, codigoTurma]);

  const limparDados = useCallback(() => {
    onChange();
    setDataSource([]);
    setFuncionarioSelecionado({
      nome: '',
      rf: '',
    });
    setTimeout(() => {
      setDesabilitarCampo(() => ({
        rf: false,
        nome: false,
      }));
    }, 200);
  }, [onChange]);

  const onChangeNome = async valor => {
    valor = removerNumeros(valor);
    if (valor.length === 0) {
      limparDados();
      return;
    }

    if (valor.length < 3) return;

    const params = {
      nome: valor,
      codigoUe,
    };

    if (codigoTurma) {
      params.codigoTurma = codigoTurma;
    }
    setExibirLoader(true);
    const retorno = await ServicoLocalizadorFuncionario.buscarPorNome(
      params
    ).catch(() => {
      setExibirLoader(false);
      limparDados();
    });
    setExibirLoader(false);
    if (retorno && retorno?.data?.items?.length > 0) {
      setDataSource([]);
      setDataSource(
        retorno.data.items.map(aluno => ({
          alunoCodigo: aluno.codigo,
          alunoNome: aluno.nome,
          codigoTurma: aluno.codigoTurma,
          turmaId: aluno.turmaId,
        }))
      );
    }
  };

  const onBuscarPorCodigo = useCallback(
    async codigo => {
      if (!codigo) {
        limparDados();
        return;
      }
      const params = {
        codigo: codigo.codigo,
        codigoUe,
      };

      if (codigoTurma) {
        params.codigoTurma = codigoTurma;
      }

      setExibirLoader(true);
      const retorno = await ServicoLocalizadorFuncionario.buscarPorCodigo(
        params
      ).catch(e => {
        setExibirLoader(false);
        if (e?.response?.status === 601) {
          erro('Funcionário não encontrado no EOL');
        } else {
          erros(e);
        }
        limparDados();
      });

      setExibirLoader(false);

      if (retorno?.data?.items?.length > 0) {
        const { rf, nome } = retorno.data.items[0];

        setDataSource(
          retorno.data.items.map(funcionario => ({
            rf: funcionario.rf,
            nome: funcionario.nome,
          }))
        );
        setFuncionarioSelecionado({
          rf,
          nome,
        });
        setDesabilitarCampo(estado => ({
          ...estado,
          nome: true,
        }));
        onChange({
          rf,
          nome,
        });
      }
    },
    [codigoTurma, codigoUe, limparDados, onChange]
  );

  const validaAntesBuscarPorCodigo = useCallback(
    valor => {
      if (timeoutBuscarPorCodigoNome) {
        clearTimeout(timeoutBuscarPorCodigoNome);
      }

      if (codigoUe) {
        const timeout = setTimeout(() => {
          onBuscarPorCodigo(valor);
        }, 500);

        setTimeoutBuscarPorCodigoNome(timeout);
      }
    },
    [codigoUe, onBuscarPorCodigo, timeoutBuscarPorCodigoNome]
  );

  const validaAntesBuscarPorNome = valor => {
    if (timeoutBuscarPorCodigoNome) {
      clearTimeout(timeoutBuscarPorCodigoNome);
    }

    if (codigoUe) {
      const timeout = setTimeout(() => {
        onChangeNome(valor);
      }, 500);

      setTimeoutBuscarPorCodigoNome(timeout);
    }
  };

  const onChangeCodigo = valor => {
    if (valor.length === 0) {
      limparDados();
    }
  };

  const onSelectFuncionario = objeto => {
    const pessoa = {
      rf: objeto.key,
      nome: objeto.props.value,
    };
    setFuncionarioSelecionado(pessoa);
    onChange(pessoa);
    setDesabilitarCampo(estado => ({
      ...estado,
      rf: true,
    }));
  };

  useEffect(() => {
    if (
      valorInicial &&
      !funcionarioSelecionado?.alunoCodigo &&
      !dataSource?.length
    ) {
      validaAntesBuscarPorCodigo({ codigo: valorInicial });
    }
  }, [
    valorInicial,
    dataSource,
    funcionarioSelecionado,
    validaAntesBuscarPorCodigo,
  ]);

  return (
    <>
      <div
        className={`${
          exibirCampoRf ? 'col-sm-12 col-md-6 col-lg-8 col-xl-8' : 'col-md-12'
        } `}
      >
        <Label text="Nome" />
        <InputNome
          placeholder={placeholder}
          dataSource={dataSource}
          onSelect={onSelectFuncionario}
          onChange={validaAntesBuscarPorNome}
          funcionarioSelecionado={funcionarioSelecionado}
          name="nome"
          desabilitado={desabilitado || desabilitarCampo.nome}
          regexIgnore={/\d+/}
          exibirLoader={exibirLoader}
        />
      </div>
      {exibirCampoRf ? (
        <div className="col-sm-12 col-md-6 col-lg-4 col-xl-4">
          <Label text="RF" />
          <InputCodigo
            funcionarioSelecionado={funcionarioSelecionado}
            onSelect={validaAntesBuscarPorCodigo}
            onChange={onChangeCodigo}
            name="rf"
            desabilitado={desabilitado || desabilitarCampo.rf}
            exibirLoader={exibirLoader}
          />
        </div>
      ) : (
        ''
      )}
    </>
  );
};

LocalizadorFuncionario.propTypes = {
  onChange: PropTypes.func,
  desabilitado: PropTypes.bool,
  codigoUe: PropTypes.oneOfType([PropTypes.any]),
  codigoDre: PropTypes.oneOfType([PropTypes.any]),
  codigoTurma: PropTypes.oneOfType([PropTypes.any]),
  exibirCampoRf: PropTypes.bool,
  valorInicial: PropTypes.oneOfType([PropTypes.any]),
  placeholder: PropTypes.string,
  idPerfil: PropTypes.string,
};

LocalizadorFuncionario.defaultProps = {
  onChange: () => {},
  desabilitado: false,
  codigoUe: '',
  codigoDre: '',
  codigoTurma: '',
  exibirCampoRf: true,
  valorInicial: '',
  placeholder: '',
  idPerfil: '',
};

export default LocalizadorFuncionario;
