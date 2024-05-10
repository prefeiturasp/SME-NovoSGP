import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { Label } from '~/componentes';
import { erros, erro } from '~/servicos/alertas';
import InputCodigo from './componentes/InputCodigo';
import InputNome from './componentes/InputNome';
import service from './services/LocalizadorEstudantesAtivosService';
import { store } from '~/redux';
import { setAlunosCodigo } from '~/redux/modulos/localizadorEstudante/actions';
import { removerNumeros } from '~/utils/funcoes/gerais';

const LocalizadorEstudantesAtivos = props => {
  const {
    onChange,
    showLabel,
    desabilitado,
    ueId,
    turmaCodigo,
    exibirCodigoEOL,
    valorInicialAlunoCodigo,
    placeholder,
    semMargin,
    limparCamposAposPesquisa,
    dataReferencia,
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
      nomeAlunoComTurmaModalidade: '',
    });
    setDataSource([]);
  }, [ueId, turmaCodigo]);

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
      nomeAlunoComTurmaModalidade: '',
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
      alunoNome: valor,
      ueCodigo: ueId,
      dataReferencia: dataReferencia
        ? window.moment(dataReferencia).format('YYYY-MM-DD')
        : window.moment().format('YYYY-MM-DD'),
    };

    if (turmaCodigo) {
      params.codigoTurma = turmaCodigo;
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
          alunoCodigo: aluno.codigoAluno,
          alunoNome: aluno.nomeAluno,
          codigoTurma: aluno.codigoTurma,
          turmaId: aluno.turmaId,
          nomeAlunoComTurmaModalidade: aluno.nomeAlunoComTurmaModalidade,
        }))
      );

      if (retorno?.data?.items?.length === 1) {
        const p = retorno.data.items[0];
        const pe = {
          alunoCodigo: parseInt(p.codigoAluno, 10),
          alunoNome: p.nomeAluno,
          codigoTurma: p.codigoTurma,
          turmaId: p.turmaId,
          nomeAlunoComTurmaModalidade: p.nomeAlunoComTurmaModalidade,
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
      alunoCodigo: codigo.codigo,
      ueCodigo: ueId,
      dataReferencia: dataReferencia
        ? window.moment(dataReferencia).format('YYYY-MM-DD')
        : window.moment().format('YYYY-MM-DD'),
    };

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
        codigoAluno,
        nomeAluno,
        codigoTurma,
        turmaId,
        nomeAlunoComTurmaModalidade,
      } = retorno.data.items[0];
      setDataSource(
        retorno.data.items.map(aluno => ({
          alunoCodigo: aluno.codigoAluno,
          alunoNome: aluno.nomeAluno,
          codigoTurma: aluno.codigoTurma,
          turmaId: aluno.turmaId,
          nomeAlunoComTurmaModalidade: aluno.nomeAlunoComTurmaModalidade,
        }))
      );
      setPessoaSelecionada({
        alunoCodigo: parseInt(codigoAluno, 10),
        alunoNome: nomeAluno,
        codigoTurma,
        turmaId,
        nomeAlunoComTurmaModalidade,
      });
      setDesabilitarCampo(estado => ({
        ...estado,
        nome: true,
      }));
      onChange({
        alunoCodigo: parseInt(codigoAluno, 10),
        alunoNome: nomeAluno,
        codigoTurma,
        turmaId,
        nomeAlunoComTurmaModalidade,
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
      nomeAlunoComTurmaModalidade: objeto.props.nomeAlunoComTurmaModalidade,
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
      <div className={`${exibirCodigoEOL ? classeNome : 'col-md-12'} `}>
        {showLabel && <Label text="Nome" control="alunoNome" />}
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

LocalizadorEstudantesAtivos.propTypes = {
  onChange: PropTypes.func,
  showLabel: PropTypes.bool,
  desabilitado: PropTypes.bool,
  ueId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  turmaCodigo: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  exibirCodigoEOL: PropTypes.bool,
  valorInicialAlunoCodigo: PropTypes.oneOfType([
    PropTypes.number,
    PropTypes.string,
  ]),
  placeholder: PropTypes.string,
  semMargin: PropTypes.bool,
  limparCamposAposPesquisa: PropTypes.bool,
  dataReferencia: PropTypes.oneOfType([PropTypes.any]),
};

LocalizadorEstudantesAtivos.defaultProps = {
  onChange: () => {},
  showLabel: false,
  desabilitado: false,
  ueId: '',
  turmaCodigo: '',
  exibirCodigoEOL: true,
  valorInicialAlunoCodigo: '',
  placeholder: '',
  semMargin: false,
  limparCamposAposPesquisa: false,
  dataReferencia: '',
};

export default LocalizadorEstudantesAtivos;
