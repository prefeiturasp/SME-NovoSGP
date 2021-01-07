import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { Label } from '~/componentes';
import { erros, erro } from '~/servicos/alertas';
import InputCodigo from './componentes/InputCodigo';
import InputNome from './componentes/InputNome';
import InputTurma from './componentes/InputTurma';
import service from './services/LocalizadorAlunoService';
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
    exibirPesquisaCodigoAluno,
    exibirPesquisaCodigoTurma,
  } = props;

  const [dataSource, setDataSource] = useState([]);
  const [pessoaSelecionada, setPessoaSelecionada] = useState({});
  const [desabilitarCampo, setDesabilitarCampo] = useState({
    codigo: false,
    nome: false,
    turma: false,
  });

  useEffect(() => {
    setPessoaSelecionada({
      alunoCodigo: '',
      alunoNome: '',
      turmaCodigo: '',
    });
  }, [ueId]);

  const onChangeNome = async valor => {
    valor = removerNumeros(valor);
    if (valor.length === 0) {
      setPessoaSelecionada({
        alunoCodigo: '',
        alunoNome: '',
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

    const retorno = await service
      .buscarPorNomeAluno({
        nome: valor,
        codigoUe: ueId,
        anoLetivo,
      })
      .catch(() => {
        setDataSource([]);
      });

    if (retorno && retorno?.data?.items?.length > 0) {
      setDataSource([]);
      setDataSource(
        retorno.data.items.map(aluno => ({
          alunoCodigo: aluno.codigo,
          alunoNome: aluno.nome,
        }))
      );
    }
  };

  const onBuscarPorCodigoTurma = async codigo => {
    const retorno = await service
      .buscarPorCodigoTurma({
        codigoTurma: codigo.codigo,
        codigoUe: ueId,
        anoLetivo,
      })
      .catch(e => {
        if (e?.response?.status === 601) {
          erro('Turma não encontrada no EOL');
        } else {
          erros(e);
        }
      });

    if (retorno?.data?.items?.length > 0) {
      const { codigo: cAluno, nome } = retorno.data.items[0];
      setDataSource(
        retorno.data.items.map(aluno => ({
          alunoCodigo: aluno.codigo,
          alunoNome: aluno.nome,
        }))
      );
      setPessoaSelecionada({
        alunoCodigo: parseInt(cAluno, 10),
        alunoNome: nome,
      });
      setDesabilitarCampo(estado => ({
        ...estado,
        nome: true,
      }));
    }
  };

  const onChangeCodigoTurma = valor => {
    if (valor.length === 0) {
      setPessoaSelecionada({
        alunoCodigo: '',
        alunoNome: '',
      });
      setDesabilitarCampo(estado => ({
        ...estado,
        nome: false,
      }));
      onChange();
    }
  };

  const onBuscarPorCodigoAluno = async codigo => {
    const retorno = await service
      .buscarPorCodigoAluno({
        codigo: codigo.codigo,
        codigoUe: ueId,
        anoLetivo,
      })
      .catch(e => {
        if (e?.response?.status === 601) {
          erro('Estudante não encontrado no EOL');
        } else {
          erros(e);
        }
      });

    if (retorno?.data?.items?.length > 0) {
      const { codigo: cAluno, nome } = retorno.data.items[0];
      setDataSource(
        retorno.data.items.map(aluno => ({
          alunoCodigo: aluno.codigo,
          alunoNome: aluno.nome,
        }))
      );
      setPessoaSelecionada({
        alunoCodigo: parseInt(cAluno, 10),
        alunoNome: nome,
      });
      setDesabilitarCampo(estado => ({
        ...estado,
        nome: true,
      }));
    }
  };

  const onChangeCodigoAluno = valor => {
    if (valor.length === 0) {
      setPessoaSelecionada({
        alunoCodigo: '',
        alunoNome: '',
      });
      setDesabilitarCampo(estado => ({
        ...estado,
        nome: false,
      }));
      onChange();
    }
  };

  const onSelectPessoa = objeto => {
    setPessoaSelecionada({
      alunoCodigo: parseInt(objeto.key, 10),
      alunoNome: objeto.props.value,
    });
    setDesabilitarCampo(estado => ({
      ...estado,
      codigo: true,
    }));
    onChange({
      alunoCodigo: parseInt(objeto.key, 10),
      alunoNome: objeto.props.value,
    });
  };

  useEffect(() => {
    if (pessoaSelecionada && pessoaSelecionada.alunoCodigo) {
      const dados = [pessoaSelecionada.alunoCodigo];
      store.dispatch(setAlunosCodigo(dados));
    } else {
      store.dispatch(setAlunosCodigo([]));
    }
  }, [pessoaSelecionada]);

  return (
    <React.Fragment>
      {exibirPesquisaCodigoTurma ? (
        <>
          <div className="col-sm-12 col-md-4 col-lg-2 col-xl-2">
            {showLabel && <Label text="Código Turma" control="turmaCodigo" />}
            <InputTurma
              pessoaSelecionada={pessoaSelecionada}
              onSelect={onBuscarPorCodigoTurma}
              onChange={onChangeCodigoTurma}
              name="turmaCodigo"
              desabilitado={desabilitado || desabilitarCampo.turma}
            />
          </div>
        </>
      ) : (
        <></>
      )}
      <div className="col-sm-12 col-md-6 col-lg-8 col-xl-8">
        {showLabel && <Label text="Nome" control="alunoNome" />}
        <InputNome
          dataSource={dataSource}
          onSelect={onSelectPessoa}
          onChange={onChangeNome}
          pessoaSelecionada={pessoaSelecionada}
          name="alunoNome"
          desabilitado={desabilitado || desabilitarCampo.nome}
          regexIgnore={/\d+/}
        />
      </div>
      {exibirPesquisaCodigoAluno ? (
        <>
          <div className="col-sm-12 col-md-4 col-lg-2 col-xl-2">
            {showLabel && <Label text="Código EOL" control="alunoCodigo" />}
            <InputCodigo
              pessoaSelecionada={pessoaSelecionada}
              onSelect={onBuscarPorCodigoAluno}
              onChange={onChangeCodigoAluno}
              name="alunoCodigo"
              desabilitado={desabilitado || desabilitarCampo.codigo}
            />
          </div>
        </>
      ) : (
        <></>
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
  exibirPesquisaCodigoAluno: PropTypes.bool,
  exibirPesquisaCodigoTurma: PropTypes.bool,
};

LocalizadorEstudante.defaultProps = {
  onChange: () => {},
  showLabel: false,
  desabilitado: false,
  ueId: '',
  anoLetivo: '',
  exibirPesquisaCodigoAluno: true,
  exibirPesquisaCodigoTurma: false,
};

export default LocalizadorEstudante;
