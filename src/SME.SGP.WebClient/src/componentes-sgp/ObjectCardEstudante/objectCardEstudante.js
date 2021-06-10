import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Loader } from '~/componentes';
import DetalhesAluno from '~/componentes/Alunos/Detalhes';
import { setDadosObjectCardEstudante } from '~/redux/modulos/objectCardEstudante/actions';
import { erros } from '~/servicos';
import ServicoEstudante from '~/servicos/Paginas/Estudante/ServicoEstudante';

const ObjectCardEstudante = props => {
  const {
    codigoAluno,
    anoLetivo,
    exibirBotaoImprimir,
    exibirFrequencia,
    permiteAlterarImagem,
  } = props;

  const dispatch = useDispatch();

  const dadosObjectCardEstudante = useSelector(
    store => store.objectCardEstudante.dadosObjectCardEstudante
  );

  const [exibirLoader, setExibirLoader] = useState(false);

  const obterDadosEstudante = useCallback(async () => {
    setExibirLoader(true);
    const resultado = await ServicoEstudante.obterDadosEstudante(
      codigoAluno,
      anoLetivo
    )
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (resultado?.data) {
      const aluno = {
        ...resultado.data,
        codigoEOL: resultado.data.codigoAluno,
        numeroChamada: resultado.data.numeroAlunoChamada,
        turma: resultado.data.turmaEscola,
      };
      dispatch(setDadosObjectCardEstudante(aluno));
    }
  }, [dispatch, codigoAluno, anoLetivo]);

  useEffect(() => {
    if (!dadosObjectCardEstudante?.codigoEOL) {
      if (codigoAluno && anoLetivo) {
        obterDadosEstudante();
      } else {
        dispatch(setDadosObjectCardEstudante());
      }
    }
  }, [
    dispatch,
    codigoAluno,
    anoLetivo,
    dadosObjectCardEstudante,
    obterDadosEstudante,
  ]);

  useEffect(() => {
    return () => dispatch(setDadosObjectCardEstudante());
  }, [dispatch]);

  return (
    <Loader loading={exibirLoader}>
      <DetalhesAluno
        dados={dadosObjectCardEstudante}
        exibirBotaoImprimir={exibirBotaoImprimir}
        exibirFrequencia={exibirFrequencia}
        permiteAlterarImagem={permiteAlterarImagem}
      />
    </Loader>
  );
};

ObjectCardEstudante.propTypes = {
  codigoAluno: PropTypes.oneOfType([PropTypes.any]),
  anoLetivo: PropTypes.oneOfType([PropTypes.any]),
  exibirBotaoImprimir: PropTypes.bool,
  exibirFrequencia: PropTypes.bool,
  permiteAlterarImagem: PropTypes.bool,
};

ObjectCardEstudante.defaultProps = {
  codigoAluno: '',
  anoLetivo: '',
  exibirBotaoImprimir: true,
  exibirFrequencia: true,
  permiteAlterarImagem: true,
};

export default ObjectCardEstudante;
