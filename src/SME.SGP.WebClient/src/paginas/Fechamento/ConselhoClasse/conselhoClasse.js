import React, { useEffect, useState, useCallback } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Loader } from '~/componentes';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Alert from '~/componentes/alert';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import {
  setAlunosConselhoClasse,
  setDadosAlunoObjectCard,
  limparDadosConselhoClasse,
} from '~/redux/modulos/conselhoClasse/actions';
import { erros } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import { Container } from './conselhoClasse.css';
import BotaoOrdenarListaAlunos from './DadosConselhoClasse/BotaoOrdenarListaAlunos/botaoOrdenarListaAlunos';
import BotoesAcoesConselhoClasse from './DadosConselhoClasse/BotoesAcoes/botoesAcoesConselhoClasse';
import DadosConselhoClasse from './DadosConselhoClasse/dadosConselhoClasse';
import ObjectCardConselhoClasse from './DadosConselhoClasse/ObjectCardConselhoClasse/objectCardConselhoClasse';
import TabelaRetratilConselhoClasse from './DadosConselhoClasse/TabelaRetratilConselhoClasse/tabelaRetratilConselhoClasse';
import servicoSalvarConselhoClasse from './servicoSalvarConselhoClasse';

const ConselhoClasse = () => {
  const dispatch = useDispatch();

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;
  const { turma, anoLetivo } = turmaSelecionada;

  const [carregandoGeral, setCarregandoGeral] = useState(false);
  const [exibirListas, setExibirListas] = useState(false);

  const obterListaAlunos = useCallback(async () => {
    setCarregandoGeral(true);
    const retorno = await ServicoConselhoClasse.obterListaAlunos(
      turma,
      anoLetivo
    ).catch(e => erros(e));
    if (retorno && retorno.data) {
      dispatch(setAlunosConselhoClasse(retorno.data));
      setExibirListas(true);
    }
    setCarregandoGeral(false);
  }, [anoLetivo, dispatch, turma]);

  const resetarInfomacoes = useCallback(() => {
    dispatch(limparDadosConselhoClasse());
  }, [dispatch]);

  useEffect(() => {
    resetarInfomacoes();
    if (turma) {
      obterListaAlunos();
    }
  }, [obterListaAlunos, turma, resetarInfomacoes]);

  const obterFrequenciaAluno = async codigoAluno => {
    const retorno = await ServicoConselhoClasse.obterFrequenciaAluno(
      codigoAluno
    ).catch(e => erros(e));
    if (retorno && retorno.data) {
      return retorno.data;
    }
    return 0;
  };

  const onChangeAlunoSelecionado = async aluno => {
    resetarInfomacoes();
    const frequenciaGeralAluno = await obterFrequenciaAluno(aluno.codigoEOL);
    const novoAluno = aluno;
    novoAluno.frequencia = frequenciaGeralAluno;    
    dispatch(setDadosAlunoObjectCard(aluno));
  };

  const permiteOnChangeAluno = async () => {
    const continuar = await servicoSalvarConselhoClasse.validarSalvarRecomendacoesAlunoFamilia();
    if (continuar) {
      return true;
    }
    return false;
  };

  return (
    <Container>
      {!turmaSelecionada.turma ? (
        <div className="col-md-12">
          <Alert
            alerta={{
              tipo: 'warning',
              id: 'alerta-sem-turma-conselho-classe',
              mensagem: 'Você precisa escolher uma turma.',
              estiloTitulo: { fontSize: '18px' },
            }}
            className="mb-2"
          />
        </div>
      ) : (
        ''
      )}
      <Cabecalho pagina="Conselho de classe" />
      <Loader loading={carregandoGeral}>
        <Card>
          {turmaSelecionada.turma ? (
            <>
              <div className="col-md-12">
                <div className="row">
                  <div className="col-md-12 d-flex justify-content-end pb-4">
                    <BotoesAcoesConselhoClasse />
                  </div>
                </div>
              </div>
              {exibirListas ? (
                <>
                  <div className="col-md-12 mb-2 d-flex">
                    <BotaoOrdenarListaAlunos />

                    <Button
                      className="btn-imprimir"
                      icon="print"
                      color={Colors.Azul}
                      border
                      onClick={() => {}}
                      disabled
                      id="btn-imprimir-conselho-classe"
                    />
                  </div>
                  <div className="col-md-12 mb-2">
                    <TabelaRetratilConselhoClasse
                      onChangeAlunoSelecionado={onChangeAlunoSelecionado}
                      permiteOnChangeAluno={permiteOnChangeAluno}
                    >
                      <>
                        <ObjectCardConselhoClasse />
                        <DadosConselhoClasse
                          codigoTurma={turmaSelecionada.turma}
                          modalidade={turmaSelecionada.modalidade}
                        />
                      </>
                    </TabelaRetratilConselhoClasse>
                  </div>
                </>
              ) : (
                ''
              )}
            </>
          ) : (
            ''
          )}
        </Card>
      </Loader>
    </Container>
  );
};

export default ConselhoClasse;
