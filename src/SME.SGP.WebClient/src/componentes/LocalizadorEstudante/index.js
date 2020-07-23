import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { Label } from '~/componentes';
import { erros, erro } from '~/servicos/alertas';
import InputCodigo from './componentes/InputCodigo';
import InputNome from './componentes/InputNome';
import service from './services/LocalizadorService';
import { store } from '~/redux';
import { setAlunosCodigo } from '~/redux/modulos/localizadorEstudante/actions';
import { removerNumeros } from '~/utils/funcoes/gerais';

const LocalizadorEstudante = props => {
  const { onChange, showLabel, desabilitado, ueId, anoLetivo } = props;

  const [dataSource, setDataSource] = useState([]);
  const [pessoaSelecionada, setPessoaSelecionada] = useState({});
  const [desabilitarCampo, setDesabilitarCampo] = useState({
    codigo: false,
    nome: false,
  });

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
      .buscarPorNome({
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

  const onBuscarPorCodigo = async codigo => {
    const retorno = await service
      .buscarPorCodigo({
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

  const onChangeCodigo = valor => {
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
      <div className="col-sm-12 col-md-6 col-lg-4 col-xl-4">
        {showLabel && <Label text="Código EOL" control="alunoCodigo" />}
        <InputCodigo
          pessoaSelecionada={pessoaSelecionada}
          onSelect={onBuscarPorCodigo}
          onChange={onChangeCodigo}
          name="alunoCodigo"
          desabilitado={desabilitado || desabilitarCampo.codigo}
        />
      </div>
    </React.Fragment>
  );
};

LocalizadorEstudante.propTypes = {
  onChange: PropTypes.func,
  showLabel: PropTypes.bool,
  desabilitado: PropTypes.bool,
  ueId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  anoLetivo: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
};

LocalizadorEstudante.defaultProps = {
  onChange: () => {},
  showLabel: false,
  desabilitado: false,
  ueId: '',
  anoLetivo: '',
};

export default LocalizadorEstudante;
