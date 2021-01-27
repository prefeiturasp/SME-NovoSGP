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
  } = props;

  const [dataSource, setDataSource] = useState([]);
  const [pessoaSelecionada, setPessoaSelecionada] = useState({});
  const [desabilitarCampo, setDesabilitarCampo] = useState({
    codigo: false,
    nome: false,
  });
  const [timeoutBuscarPorCodigo, setTimeoutBuscarPorCodigo] = useState('');

  useEffect(() => {
    setPessoaSelecionada({
      alunoCodigo: '',
      alunoNome: '',
      codigoTurma: '',
      turmaId: '',
    });
    setDataSource([]);
  }, [ueId, codigoTurma]);

  const onChangeNome = async valor => {
    valor = removerNumeros(valor);
    if (valor.length === 0) {
      setPessoaSelecionada({
        alunoCodigo: '',
        alunoNome: '',
        codigoTurma: '',
        turmaId: '',
      });
      setTimeout(() => {
        setDesabilitarCampo(() => ({
          codigo: false,
          nome: false,
        }));
      }, 200);
      setDataSource([]);
      onChange();
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

    const retorno = await service.buscarPorNome(params).catch(() => {
      onChange();
      setDataSource([]);
      setPessoaSelecionada({
        alunoCodigo: '',
        alunoNome: '',
        codigoTurma: '',
        turmaId: '',
      });
    });

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

  const onBuscarPorCodigo = async codigo => {
    const params = {
      codigo: codigo.codigo,
      codigoUe: ueId,
      anoLetivo,
    };

    if (codigoTurma) {
      params.codigoTurma = codigoTurma;
    }

    const retorno = await service.buscarPorCodigo(params).catch(e => {
      if (e?.response?.status === 601) {
        erro('Estudante não encontrado no EOL');
      } else {
        erros(e);
      }
      onChange();
      setDataSource([]);
      setPessoaSelecionada({
        alunoCodigo: '',
        alunoNome: '',
        codigoTurma: '',
        turmaId: '',
      });
    });

    if (retorno?.data?.items?.length > 0) {
      const {
        codigo: cAluno,
        nome,
        codigoTurma,
        turmaId,
      } = retorno.data.items[0];
      setDataSource(
        retorno.data.items.map(aluno => ({
          alunoCodigo: aluno.codigo,
          alunoNome: aluno.nome,
          codigoTurma: aluno.codigoTurma,
          turmaId: aluno.turmaId,
        }))
      );
      setPessoaSelecionada({
        alunoCodigo: parseInt(cAluno, 10),
        alunoNome: nome,
        codigoTurma,
        turmaId,
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
      });
    }
  };

  const validaAntesBuscarPorCodigo = valor => {
    if (timeoutBuscarPorCodigo) {
      clearTimeout(timeoutBuscarPorCodigo);
    }

    if (ueId) {
      const timeout = setTimeout(() => {
        onBuscarPorCodigo(valor);
      }, 500);

      setTimeoutBuscarPorCodigo(timeout);
    }
  };

  const onChangeCodigo = valor => {
    if (valor.length === 0) {
      setPessoaSelecionada({
        alunoCodigo: '',
        alunoNome: '',
        codigoTurma: '',
        turmaId: '',
      });
      setDesabilitarCampo(estado => ({
        ...estado,
        nome: false,
      }));
      onChange();
    }
  };

  const onSelectPessoa = objeto => {
    const pessoa = {
      alunoCodigo: parseInt(objeto.key, 10),
      alunoNome: objeto.props.value,
      codigoTurma: objeto.props.codigoTurma,
      turmaId: objeto.props.turmaId,
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
        {showLabel && <Label text="Nome" control="alunoNome" />}
        <InputNome
          placeholder={placeholder}
          dataSource={dataSource}
          onSelect={onSelectPessoa}
          onChange={onChangeNome}
          pessoaSelecionada={pessoaSelecionada}
          name="alunoNome"
          desabilitado={desabilitado || desabilitarCampo.nome}
          regexIgnore={/\d+/}
        />
      </div>
      {exibirCodigoEOL ? (
        <div className="col-sm-12 col-md-6 col-lg-4 col-xl-4">
          {showLabel && <Label text="Código EOL" control="alunoCodigo" />}
          <InputCodigo
            pessoaSelecionada={pessoaSelecionada}
            onSelect={validaAntesBuscarPorCodigo}
            onChange={onChangeCodigo}
            name="alunoCodigo"
            desabilitado={desabilitado || desabilitarCampo.codigo}
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
};

export default LocalizadorEstudante;
