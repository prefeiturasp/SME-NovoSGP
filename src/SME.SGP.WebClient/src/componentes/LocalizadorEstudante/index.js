import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { Label } from '~/componentes';
import { erros, erro } from '~/servicos/alertas';
import InputCodigo from './componentes/InputCodigo';
import InputNome from './componentes/InputNome';
import service from './services/LocalizadorEstudanteService';
import { store } from '~/redux';
import { setAlunosCodigo } from '~/redux/modulos/localizadorEstudante/actions';
import { removerNumeros } from '~/utils/funcoes/gerais';

const LocalizadorEstudante = props => {
  const {
    onChange,
    showLabel,
    desabilitado,
    ueId,
    anoLetivo,
    codigoTurma,
    exibirCodigoEOL,
    valorInicialAlunoCodigo,
    placeholder,
    semMargin,
    limparCamposAposPesquisa,
    labelAlunoNome,
  } = props;

  const classeNome = semMargin
    ? 'col-sm-12 col-md-6 col-lg-8 col-xl-8 p-0'
    : 'col-sm-12 col-md-6 col-lg-8 col-xl-8';
  const classeCodigo = semMargin
    ? 'col-sm-12 col-md-6 col-lg-4 col-xl-4 p-0 pl-4'
    : 'col-sm-12 col-md-6 col-lg-4 col-xl-4';
  
  const [dataSource, setDataSource] = useState([]);
  const [pessoaSelecionada, setPessoaSelecionada] = useState({});
  const [desabilitarCampo, setDesabilitarCampo] = useState({
    codigo: false,
    nome: false,
  });
  const [timeoutBuscarPorCodigoNome, setTimeoutBuscarPorCodigoNome] = useState(
    ''
  );
  const [exibirLoader, setExibirLoader] = useState(false);

  useEffect(() => {
    setPessoaSelecionada({
      alunoCodigo: '',
      alunoNome: '',
      codigoTurma: '',
      turmaId: '',
      nomeComModalidadeTurma: '',
    });
    setDataSource([]);
  }, [ueId, codigoTurma]);

  useEffect(() => {
    if (Object.keys(pessoaSelecionada).length && limparCamposAposPesquisa) {
      setPessoaSelecionada({});
      setDesabilitarCampo({
        codigo: false,
        nome: false,
      });
    }
  }, [pessoaSelecionada, limparCamposAposPesquisa]);

  const limparDados = () => {
    onChange();
    setDataSource([]);
    setPessoaSelecionada({
      alunoCodigo: '',
      alunoNome: '',
      codigoTurma: '',
      turmaId: '',
      nomeComModalidadeTurma: '',
    });
    setTimeout(() => {
      setDesabilitarCampo(() => ({
        codigo: false,
        nome: false,
      }));
    }, 200);
  };

  const onChangeNome = async valor => {
    valor = removerNumeros(valor);
    if (valor.length === 0) {
      limparDados();
      return;
    }

    if (valor.length < 3) return;

    const params = {
      nome: valor,
      codigoUe: ueId,
      anoLetivo,
    };

    if (codigoTurma) {
      params.codigoTurma = codigoTurma;
    }
    setExibirLoader(true);
    const retorno = await service.buscarPorNome(params).catch(e => {
      if (e?.response?.status === 601) {
        erro('Estudante/Criança não encontrado no EOL');
      } else {
        erros(e);
      }
      setExibirLoader(false);
      limparDados();
    });
    setExibirLoader(false);
    if (retorno?.data?.items?.length > 0) {
      setDataSource([]);
      setDataSource(
        retorno.data.items.map(aluno => ({
          alunoCodigo: aluno.codigo,
          alunoNome: aluno.nome,
          codigoTurma: aluno.codigoTurma,
          turmaId: aluno.turmaId,
          nomeComModalidadeTurma: aluno.nomeComModalidadeTurma,
        }))
      );

      if (retorno?.data?.items?.length === 1) {
        const p = retorno.data.items[0];
        const pe = {
          alunoCodigo: parseInt(p.codigo, 10),
          alunoNome: p.nome,
          codigoTurma: p.codigoTurma,
          turmaId: p.turmaId,
        };
        setPessoaSelecionada(pe);
        setDesabilitarCampo(estado => ({
          ...estado,
          codigo: true,
        }));
        onChange(pe);
      }
    }
  };

  const onBuscarPorCodigo = async codigo => {
    if (!codigo.codigo) {
      limparDados();
      return;
    }
    const params = {
      codigo: codigo.codigo,
      codigoUe: ueId,
      anoLetivo,
    };

    if (codigoTurma) {
      params.codigoTurma = codigoTurma;
    }

    setExibirLoader(true);
    const retorno = await service.buscarPorCodigo(params).catch(e => {
      setExibirLoader(false);
      if (e?.response?.status === 601) {
        erro('Estudante/Criança não encontrado no EOL');
      } else {
        erros(e);
      }
      limparDados();
    });

    setExibirLoader(false);

    if (retorno?.data?.items?.length > 0) {
      const {
        codigo: cAluno,
        nome,
        codigoTurma,
        turmaId,
        nomeComModalidadeTurma,
      } = retorno.data.items[0];
      setDataSource(
        retorno.data.items.map(aluno => ({
          alunoCodigo: aluno.codigo,
          alunoNome: aluno.nome,
          codigoTurma: aluno.codigoTurma,
          turmaId: aluno.turmaId,
          nomeComModalidadeTurma: aluno.nomeComModalidadeTurma,
        }))
      );
      setPessoaSelecionada({
        alunoCodigo: parseInt(cAluno, 10),
        alunoNome: nome,
        codigoTurma,
        turmaId,
        nomeComModalidadeTurma,
      });
      setDesabilitarCampo(estado => ({
        ...estado,
        nome: true,
      }));
      onChange({
        alunoCodigo: parseInt(cAluno, 10),
        alunoNome: nome,
        codigoTurma,
        turmaId,
        nomeComModalidadeTurma,
      });
    }
  };

  const validaAntesBuscarPorCodigo = valor => {
    if (timeoutBuscarPorCodigoNome) {
      clearTimeout(timeoutBuscarPorCodigoNome);
    }

    if (ueId) {
      const timeout = setTimeout(() => {
        onBuscarPorCodigo(valor);
      }, 800);

      setTimeoutBuscarPorCodigoNome(timeout);
    }
  };

  const validaAntesBuscarPorNome = valor => {
    if (timeoutBuscarPorCodigoNome) {
      clearTimeout(timeoutBuscarPorCodigoNome);
    }

    if (ueId) {
      const timeout = setTimeout(() => {
        onChangeNome(valor);
      }, 800);

      setTimeoutBuscarPorCodigoNome(timeout);
    }
  };

  const onChangeCodigo = valor => {
    if (valor.length === 0) {
      limparDados();
    }
  };

  const onSelectPessoa = objeto => {
    const pessoa = {
      alunoCodigo: parseInt(objeto.key, 10),
      alunoNome: objeto.props.value,
      codigoTurma: objeto.props.codigoTurma,
      turmaId: objeto.props.turmaId,
      nomeComModalidadeTurma: objeto.props.nomeComModalidadeTurma,
    };
    setPessoaSelecionada(pessoa);
    onChange(pessoa);
    setDesabilitarCampo(estado => ({
      ...estado,
      codigo: true,
    }));
  };

  useEffect(() => {
    if (pessoaSelecionada && pessoaSelecionada.alunoCodigo) {
      const dados = [pessoaSelecionada.alunoCodigo];
      store.dispatch(setAlunosCodigo(dados));
    } else {
      store.dispatch(setAlunosCodigo([]));
    }
  }, [pessoaSelecionada]);

  useEffect(() => {
    if (
      valorInicialAlunoCodigo &&
      !pessoaSelecionada?.alunoCodigo &&
      !dataSource?.length
    ) {
      validaAntesBuscarPorCodigo({ codigo: valorInicialAlunoCodigo });
    }
  }, [valorInicialAlunoCodigo, dataSource, pessoaSelecionada]);

  return (
    <React.Fragment>
      <div
        className={`${
          exibirCodigoEOL ? 'col-sm-12 col-md-6 col-lg-8 col-xl-8' : 'col-md-12'
        } `}
      >
        {showLabel && <Label text={labelAlunoNome} control="alunoNome" />}
        <InputNome
          placeholder={placeholder}
          dataSource={dataSource}
          onSelect={onSelectPessoa}
          onChange={validaAntesBuscarPorNome}
          pessoaSelecionada={pessoaSelecionada}
          name="alunoNome"
          desabilitado={desabilitado || desabilitarCampo.nome}
          regexIgnore={/\d+/}
          exibirLoader={exibirLoader}
        />
      </div>
      {exibirCodigoEOL ? (
        <div className={classeCodigo}>
          {showLabel && <Label text="Código EOL" control="alunoCodigo" />}
          <InputCodigo
            pessoaSelecionada={pessoaSelecionada}
            onSelect={validaAntesBuscarPorCodigo}
            onChange={onChangeCodigo}
            name="alunoCodigo"
            desabilitado={desabilitado || desabilitarCampo.codigo}
            exibirLoader={exibirLoader}
          />
        </div>
      ) : (
        ''
      )}
    </React.Fragment>
  );
};

LocalizadorEstudante.propTypes = {
  onChange: PropTypes.func,
  showLabel: PropTypes.bool,
  desabilitado: PropTypes.bool,
  ueId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  anoLetivo: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  codigoTurma: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  exibirCodigoEOL: PropTypes.bool,
  valorInicialAlunoCodigo: PropTypes.oneOfType([
    PropTypes.number,
    PropTypes.string,
  ]),
  placeholder: PropTypes.string,
  semMargin: PropTypes.bool,
  limparCamposAposPesquisa: PropTypes.bool,
  labelAlunoNome: PropTypes.string,
};

LocalizadorEstudante.defaultProps = {
  onChange: () => {},
  showLabel: false,
  desabilitado: false,
  ueId: '',
  anoLetivo: '',
  codigoTurma: '',
  exibirCodigoEOL: true,
  valorInicialAlunoCodigo: '',
  placeholder: '',
  semMargin: false,
  limparCamposAposPesquisa: false,
  labelAlunoNome: 'Nome',
};

export default LocalizadorEstudante;
