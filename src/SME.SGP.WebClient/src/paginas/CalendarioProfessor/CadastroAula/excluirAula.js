import React, { useEffect, useState } from 'react';
import { Formik, Form } from 'formik';
import { ModalConteudoHtml, RadioGroupButton, Loader } from '~/componentes';
import ServicoCadastroAula from '~/servicos/Paginas/CalendarioProfessor/CadastroAula/ServicoCadastroAula';
import { erros, sucesso } from '~/servicos/alertas';

function ExcluirAula({
  visivel,
  idAula,
  dataAula,
  nomeComponente,
  recorrencia,
  onFecharModal,
  onCancelar,
}) {
  const listRecorrenciaInicial = [{ label: 'Somente o dia', value: 1 }];

  const [opcoesExcluirRecorrencia, setOpcoesExcluirRecorrencia] = useState(
    listRecorrenciaInicial
  );

  const [carregando, setCarregando] = useState(false);
  const [recorrenciaSelecionada, setRRecorrenciaSelecionada] = useState(1);

  const excluirAula = () => {
    const nome = nomeComponente();
    setCarregando(true);
    ServicoCadastroAula.excluirAula(idAula, recorrenciaSelecionada, nome)
      .then(resposta => {
        sucesso(resposta.data.mensagens[0]);
        onFecharModal();
      })
      .catch(e => erros(e))
      .finally(() => setCarregando(false));
  };

  useEffect(() => {
    if (visivel) {
      let opcaoAIncluir = { label: 'Bimestre atual', value: 2 };

      if (recorrencia.recorrenciaAula == 3)
        opcaoAIncluir = { label: 'Todos os bimestres', value: 3 };

      setOpcoesExcluirRecorrencia(opcoes => {
        return [...opcoes, opcaoAIncluir];
      });
    } else {
      setOpcoesExcluirRecorrencia(listRecorrenciaInicial);
    }
  }, [visivel]);

  return (
    <ModalConteudoHtml
      key="excluirAula"
      visivel={visivel}
      onConfirmacaoPrincipal={excluirAula}
      onConfirmacaoSecundaria={onCancelar}
      onClose={onCancelar}
      labelBotaoPrincipal="Confirmar"
      labelBotaoSecundario="Cancelar"
      titulo={`Excluir aula - ${dataAula}`}
      closable={false}
    >
      <Loader loading={carregando}>
        <Formik
          enableReinitialize
          initialValues={{ tipoRecorrenciaExclusao: 1 }}
          onSubmit={() => {}}
          validateOnChange
          validateOnBlur
        >
          {form => (
            <Form className="col-md-12 mb-4">
              <div className="row justify-content-start">
                <div
                  className="col-sm-12 col-md-12"
                  style={{ paddingTop: '10px' }}
                >
                  <p>{`Existem ${recorrencia.quantidadeAulasRecorrentes} ocorrências desta aula a partir desta data.`}</p>
                  <p>
                    {recorrencia.existeFrequenciaOuPlanoAula
                      ? ` Esta aula ou sua recorrência possui frequência ou plano de aula registrado, ao excluí-la estará excluindo ${
                          recorrencia.quantidadeAulasRecorrentes == 1
                            ? 'esse registro'
                            : 'estes registros'
                        } também.`
                      : ''}
                  </p>
                  <p>Qual opção de exclusão você deseja realizar?</p>
                </div>
                <div className="col-sm-12 col-md-12 d-block">
                  <RadioGroupButton
                    form={form}
                    id="tipo-recorrencia-exclusao"
                    label="Realizar exclusão"
                    opcoes={opcoesExcluirRecorrencia}
                    name="tipoRecorrenciaExclusao"
                    onChange={e => setRRecorrenciaSelecionada(e.target.value)}
                    desabilitado={false}
                  />
                </div>
              </div>
            </Form>
          )}
        </Formik>
      </Loader>
    </ModalConteudoHtml>
  );
}

export default ExcluirAula;
