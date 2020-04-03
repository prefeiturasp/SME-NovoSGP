import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import { Loader } from '~/componentes';
import { Ordenacao } from '~/componentes-sgp';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Alert from '~/componentes/alert';
import DetalhesAluno from '~/componentes/Alunos/Detalhes';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import TabelaRetratil from '~/componentes/TabelaRetratil';
import { Container } from './conselho-classe.css';

const ConselhoClasse = () => {
  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const [carregandoGeral, setCarregandoGeral] = useState(false);

  const mockAlunos = [
    {
      id: 1,
      idEol: 1,
      numeroChamada: 1,
      nome: 'Italo Gustavo Pereira de Maio',
      ativo: true,
      situacao: 'Transferido em 11/11/2011',
    },
    {
      id: 2,
      idEol: 2,
      numeroChamada: 2,
      nome: 'Thiago de Oliveira Ramos',
      ativo: true,
      situacao: 'Novo aluno',
    },
    {
      id: 3,
      idEol: 3,
      numeroChamada: 3,
      nome: 'Alana Ferreira de Oliveira',
      ativo: true,
      situacao: null,
    },
    {
      id: 4,
      idEol: 4,
      numeroChamada: 4,
      nome: 'Alany Santos da Silva Sauro',
      ativo: true,
      situacao: null,
    },
    {
      id: 5,
      idEol: 5,
      numeroChamada: 5,
      nome: 'Alany Santos da Silva Sauro',
      ativo: false,
      situacao: null,
    },
    {
      id: 6,
      idEol: 6,
      numeroChamada: 6,
      nome: 'Alany Santos da Silva Sauro',
      ativo: true,
      situacao: 'Transferido em 11/11/2011',
    },
    {
      id: 7,
      idEol: 7,
      numeroChamada: 7,
      nome: 'Alany Santos da Silva Sauro',
      ativo: true,
      situacao: null,
    },
    {
      id: 8,
      idEol: 8,
      numeroChamada: 8,
      nome: 'Alany Santos da Silva Sauro',
      ativo: true,
      situacao: null,
    },
    {
      id: 9,
      idEol: 9,
      numeroChamada: 9,
      nome: 'Alany Santos da Silva Sauro',
      ativo: true,
      situacao: null,
    },
    {
      id: 10,
      idEol: 10,
      numeroChamada: 10,
      nome: 'Alany Santos da Silva Sauro',
      ativo: false,
      situacao: null,
    },
    {
      id: 11,
      idEol: 11,
      numeroChamada: 11,
      nome: 'Alany Santos da Silva Sauro',
      ativo: true,
      situacao: null,
    },
    {
      id: 12,
      idEol: 12,
      numeroChamada: 12,
      nome: 'Alany Santos da Silva Sauro',
      ativo: true,
      situacao: null,
    },
  ];

  const [alunos, setAlunos] = useState(mockAlunos);

  const onChangeAlunoSelecionado = aluno => {
    console.log(aluno);
  };

  const onClickVoltar = () => {
    console.log('onClickVoltar');
  };

  const onClickCancelar = () => {
    console.log('onClickCancelar');
  };

  const onClickSalvar = () => {
    console.log('onClickSalvar');
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
                    <Button
                      label="Voltar"
                      icon="arrow-left"
                      color={Colors.Azul}
                      border
                      className="mr-2"
                      onClick={onClickVoltar}
                    />
                    <Button
                      label="Cancelar"
                      color={Colors.Roxo}
                      border
                      className="mr-2"
                      onClick={onClickCancelar}
                    />
                    <Button
                      label="Salvar"
                      color={Colors.Roxo}
                      border
                      bold
                      className="mr-2"
                      onClick={onClickSalvar}
                    />
                  </div>
                </div>
              </div>
              <div className="col-md-12 mb-2 d-flex">
                <Ordenacao
                  conteudoParaOrdenar={alunos}
                  ordenarColunaNumero="id"
                  ordenarColunaTexto="nome"
                  retornoOrdenado={retorno => setAlunos(retorno)}
                />
                <Button
                  className="btn-imprimir"
                  icon="print"
                  color={Colors.Azul}
                  border
                  onClick={() => {}}
                  disabled
                />
              </div>
              <div className="col-md-12 mb-2">
                <TabelaRetratil
                  onChangeAlunoSelecionado={onChangeAlunoSelecionado}
                  alunos={alunos}
                >
                  <DetalhesAluno
                    dados={{
                      nome: 'ALANA FERREIRA DE OLIVEIRA',
                      numero: 1,
                      dataNascimento: '02/02/2020',
                      codigoEOL: 4241513,
                      situacao: 'Matriculado',
                      dataSituacao: '04/02/2019',
                      frequencia: 96,
                    }}
                  />
                </TabelaRetratil>
              </div>
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
