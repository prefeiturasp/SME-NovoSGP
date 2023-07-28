pipeline {
    environment {
      branchname =  env.BRANCH_NAME.toLowerCase()
      kubeconfig = getKubeconf(env.branchname)
      registryCredential = 'jenkins_registry'
      deployment1 = "${env.branchname == 'release-r2' ? 'sme-api-rc2' : 'sme-api' }"           
      deployment2 = "${env.branchname == 'release-r2' ? 'sme-worker-fechamento-r2' : 'sme-worker-fechamento' }"  
      deployment3 = "${env.branchname == 'release-r2' ? 'sme-worker-geral-r2' : 'sme-worker-geral' }"
      deployment4 = "${env.branchname == 'release-r2' ? 'sme-worker-aee-r2' : 'sme-worker-aee' }"
      deployment5 = "${env.branchname == 'release-r2' ? 'sme-worker-aula-r2' : 'sme-worker-aula' }"
      deployment6 = "${env.branchname == 'release-r2' ? 'sme-worker-frequencia-r2' : 'sme-worker-frequencia' }"
      deployment7 = "${env.branchname == 'release-r2' ? 'sme-worker-institucional-r2' : 'sme-worker-institucional' }"
      deployment8 = "${env.branchname == 'release-r2' ? 'sme-worker-pendencias-r2' : 'sme-worker-pendencias' }"
      deployment9 = "${env.branchname == 'release-r2' ? 'sme-worker-avaliacao-r2' : 'sme-worker-avaliacao' }"
      deployment10 = "${env.branchname == 'release-r2' ? 'sme-worker-auditoria-r2' : 'sme-worker-auditoria' }"
      deployment11 = "${env.branchname == 'release-r2' ? 'sme-worker-notificacoes-r2' : 'sme-worker-notificacoes' }"
      deployment12 = "${env.branchname == 'release-r2' ? 'sme-worker-notificacoes-hub-r2' : 'sme-worker-notificacoes-hub' }"
      deployment13 = "${env.branchname == 'release-r2' ? 'sme-worker-compressao-r2' : 'sme-worker-compressao' }"
      deployment14 = "${env.branchname == 'release-r2' ? 'sme-worker-naapa-r2' : 'sme-worker-naapa' }"
    }
  
    agent {
      node { label 'SME-AGENT-SGP' }
    }

    options {
      timestamps ()
      buildDiscarder(logRotator(numToKeepStr: '5', artifactNumToKeepStr: '5'))
      disableConcurrentBuilds()
      skipDefaultCheckout()
    }
  
    stages {

        stage('CheckOut') {            
            steps { checkout scm }            
        }
   
        stage('Sonar & Build') {
          when { anyOf { branch 'master'; branch 'main'; branch 'pre-prod'; branch "story/*"; branch 'development'; branch 'release'; branch 'release-r2'; branch 'infra/*'; } } 
          parallel {
            stage('Sonar') {
            agent { node { label 'SME-AGENT-SGP-SONAR' } }
            when { anyOf { branch '_master'; branch 'main'; branch '_pre-prod'; branch "story/*"; branch '_development'; branch '_release'; branch '_release-r2'; branch 'infra/*'; } } 
                steps {
                  checkout scm
                  script{
                    withSonarQubeEnv('sonarqube-local'){
                      sh 'dotnet-sonarscanner begin /k:"SME-NovoSGP" /d:sonar.cs.opencover.reportsPaths="teste/SME.SGP.TesteIntegracao/coverage.opencover.xml" /d:sonar.coverage.exclusions="**Test*.cs, **/*SME.SGP.Dados.*, **/*SME.SGP.Dominio.Interfaces, **/*SME.SGP.Api, **/*SME.SGP.Infra, **/*SME.SGP.IoC, **/*SME.SGP.Infra.*, **/*/Workers/*, **/*/Hub/*"'
                      sh 'dotnet build SME.SGP.sln'
                      sh 'dotnet test teste/SME.SGP.TesteIntegracao --no-build /p:CollectCoverage=true /p:CoverletOutputFormat=opencover'
                      sh 'dotnet-sonarscanner'
                    }
                 }
               }
            }
            stage('sme-sgp-backend') {
              agent { node { label 'SME-AGENT-SGP' } }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-sgp-backend"
                  dockerImage1 = docker.build(imagename, "-f src/SME.SGP.Api/Dockerfile .")
                }
              }
            }
            stage('sme-worker-geral') {
              agent { node { label 'SME-AGENT-SGP' } }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-geral"
                  dockerImage2 = docker.build(imagename, "-f src/SME.SGP.Worker.Rabbbit/Dockerfile .")
                }
              }
            }
            stage('sme-worker-fechamento') {
              agent { node { label 'SME-AGENT-SGP' } }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-fechamento"
                  dockerImage3 = docker.build(imagename, "-f src/SME.SGP.Fechamento.Worker/Dockerfile .")
                }
              }
            }
            stage('sme-worker-aee') {
              agent { node { label 'SME-AGENT-SGP' } }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-aee"
                  dockerImage4 = docker.build(imagename, "-f src/SME.SGP.AEE.Worker/Dockerfile .")
                }
              }
            }
            stage('sme-worker-aula') {
              agent { node { label 'SME-AGENT-SGP' } }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-aula"
                  dockerImage5 = docker.build(imagename, "-f src/SME.SGP.Aula.Worker/Dockerfile .")
                }
              }
            }
            stage('sme-worker-frequencia') {
              agent { node { label 'SME-AGENT-SGP' } }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-frequencia"
                  dockerImage6 = docker.build(imagename, "-f src/SME.SGP.Frequencia.Worker/Dockerfile .")
                }
              }
            }
            stage('sme-worker-institucional') {
              agent { node { label 'SME-AGENT-SGP' } }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-institucional"
                  dockerImage7 = docker.build(imagename, "-f src/SME.SGP.Institucional.Worker/Dockerfile .")
                }
              }
            }
            stage('sme-worker-pendencias') {
              agent { node { label 'SME-AGENT-SGP' } }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-pendencias"
                  dockerImage8 = docker.build(imagename, "-f src/SME.SGP.Pendencias.Worker/Dockerfile .")
                }
              }
            }
            stage('sme-worker-avaliacao') {
              agent { node { label 'SME-AGENT-SGP' } }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-avaliacao"
                  dockerImage9 = docker.build(imagename, "-f src/SME.SGP.Avaliacao.Worker/Dockerfile .")
                }
              }
            }
            stage('sme-worker-auditoria') {
              agent { node { label 'SME-AGENT-SGP' } }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-auditoria"
                  dockerImage10 = docker.build(imagename, "-f src/SME.SGP.Auditoria.Worker/Dockerfile .")
                }
              }
            }
            stage('sme-worker-notificacoes') {
              agent { node { label 'SME-AGENT-SGP' } }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-notificacoes"
                  dockerImage11 = docker.build(imagename, "-f src/SME.SGP.Notificacoes.Worker/Dockerfile .")
                }
              }
            }
            stage('sme-worker-notificacoes-hub') {
              agent { node { label 'SME-AGENT-SGP' } }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-notificacoes-hub"
                  dockerImage12 = docker.build(imagename, "-f src/SME.SGP.Notificacoes.Hub/Dockerfile .")
                }
              }
            }            
            stage('sme-worker-compressao') {
              agent { node { label 'SME-AGENT-SGP' } }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-compressao"
                  dockerImage13 = docker.build(imagename, "-f src/SME.SGP.ComprimirArquivos.Worker/Dockerfile .")
                }
              }
            }
            stage('sme-worker-naapa') {
              agent { node { label 'SME-AGENT-SGP' } }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-naapa"
                  dockerImage14 = docker.build(imagename, "-f src/SME.SGP.NAAPA.Worker/Dockerfile .")
                }
              }
            }   
          }
    }
    stage('Push'){
      agent { node { label 'SME-AGENT-SGP' } }
      when { anyOf {  branch 'master'; branch 'main'; branch 'development'; branch 'release'; branch 'release-r2'; branch 'pre-prod'; } }       
      steps {
        script{
              docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                dockerImage1.push()
                dockerImage2.push()
                dockerImage3.push()
                dockerImage4.push()
                dockerImage5.push()
                dockerImage6.push()
                dockerImage7.push()
                dockerImage8.push()
                dockerImage9.push()
                dockerImage10.push()
                dockerImage11.push()
                dockerImage12.push()
                dockerImage13.push()
                dockerImage14.push()
              }
        }
      }
    }
        stage('Deploy'){
            when { anyOf {  branch 'master'; branch 'main'; branch 'development'; branch 'release'; branch 'release-r2'; branch 'pre-prod'; } }        
            steps {
                script{
                  //if(testPassed){
                        if ( env.branchname == 'main' ||  env.branchname == 'master' ) {
                            withCredentials([string(credentialsId: 'aprovadores-sgp', variable: 'aprovadores')]) {
                                timeout(time: 24, unit: "HOURS") {
                                    input message: 'Deseja realizar o deploy?', ok: 'SIM', submitter: "${aprovadores}"
                                }
                            }
                        }
                        withCredentials([file(credentialsId: "${kubeconfig}", variable: 'config')]){
                                sh('cp $config '+"$home"+'/.kube/config')
                                sh "kubectl rollout restart deployment/${deployment1} -n sme-novosgp"
                                sh "kubectl rollout restart deployment/${deployment2} -n sme-novosgp"   
                                sh "kubectl rollout restart deployment/${deployment3} -n sme-novosgp"   
                                sh "kubectl rollout restart deployment/${deployment4} -n sme-novosgp"
                                sh "kubectl rollout restart deployment/${deployment5} -n sme-novosgp"
                                sh "kubectl rollout restart deployment/${deployment6} -n sme-novosgp"
                                sh "kubectl rollout restart deployment/${deployment7} -n sme-novosgp"
                                sh "kubectl rollout restart deployment/${deployment8} -n sme-novosgp"
                                sh "kubectl rollout restart deployment/${deployment9} -n sme-novosgp"
                                sh "kubectl rollout restart deployment/${deployment10} -n sme-novosgp"
                                sh "kubectl rollout restart deployment/${deployment11} -n sme-novosgp"
                                sh "kubectl rollout restart deployment/${deployment12} -n sme-novosgp"
                                sh "kubectl rollout restart deployment/${deployment13} -n sme-novosgp"
                                sh "kubectl rollout restart deployment/${deployment14} -n sme-novosgp"
                                sh('rm -f '+"$home"+'/.kube/config')
                        }
                    //}
                }
            }           
        }
             
      stage('Flyway') {
        agent { label 'master' }
        when { anyOf {  branch 'master'; branch 'main'; branch 'development'; branch 'release'; branch 'release-r2'; branch 'pre-prod'; } }
        steps{
          withCredentials([string(credentialsId: "flyway_sgp_${branchname}", variable: 'url')]) {
            checkout scm
            sh 'docker run --rm -v $(pwd)/scripts:/opt/scripts registry.sme.prefeitura.sp.gov.br/devops/flyway:5.2.4 -url=$url -locations="filesystem:/opt/scripts" -outOfOrder=true migrate'
          }
        }       
      }

      stage('Deploy Treinamento'){
          when { anyOf { branch 'release'; } }        
          steps {
              script{
                  try {
                      withCredentials([file(credentialsId: "${kubeconfig}", variable: 'config')]){
                          sh('cp $config '+"$home"+'/.kube/config')
                          sh "kubectl -n sme-novosgp-treino rollout restart deploy"
                          sh('rm -f '+"$home"+'/.kube/config')
                      }
                  }
                  catch (err) {
                      echo err.getMessage()
                  }
              }
          }           
      }

      stage('Treinamento Flyway') {
        agent { label 'master' }
        when { anyOf {  branch 'release'; } }
        steps{
          script{
            try {
                withCredentials([string(credentialsId: "flyway_sgp_treinamento", variable: 'url')]) {
                checkout scm
                sh 'docker run --rm -v $(pwd)/scripts:/opt/scripts registry.sme.prefeitura.sp.gov.br/devops/flyway:5.2.4 -url=$url -locations="filesystem:/opt/scripts" -outOfOrder=true migrate'
                }
            } 
            catch (err) {
                echo err.getMessage()
            }
          }
        }       
      }        
    }

  post {
    success { sendTelegram("🚀 Job Name: ${JOB_NAME} \nBuild: ${BUILD_DISPLAY_NAME} \nStatus: Success \nLog: \n${env.BUILD_URL}console") }
    unstable { sendTelegram("💣 Job Name: ${JOB_NAME} \nBuild: ${BUILD_DISPLAY_NAME} \nStatus: Unstable \nLog: \n${env.BUILD_URL}console") }
    failure { sendTelegram("💥 Job Name: ${JOB_NAME} \nBuild: ${BUILD_DISPLAY_NAME} \nStatus: Failure \nLog: \n${env.BUILD_URL}console") }
    aborted { sendTelegram ("😥 Job Name: ${JOB_NAME} \nBuild: ${BUILD_DISPLAY_NAME} \nStatus: Aborted \nLog: \n${env.BUILD_URL}console") }
  }
}
def sendTelegram(message) {
    def encodedMessage = URLEncoder.encode(message, "UTF-8")
    withCredentials([string(credentialsId: 'telegramToken', variable: 'TOKEN'),
    string(credentialsId: 'telegramChatId', variable: 'CHAT_ID')]) {
        response = httpRequest (consoleLogResponseBody: true,
                contentType: 'APPLICATION_JSON',
                httpMode: 'GET',
                url: 'https://api.telegram.org/bot'+"$TOKEN"+'/sendMessage?text='+encodedMessage+'&chat_id='+"$CHAT_ID"+'&disable_web_page_preview=true',
                validResponseCodes: '200')
        return response
    }
}
def getKubeconf(branchName) {
    if("main".equals(branchName)) { return "config_prd"; }
    else if ("master".equals(branchName)) { return "config_prd"; }
    else if ("pre-prod".equals(branchName)) { return "config_prd"; }
    else if ("homolog".equals(branchName)) { return "config_hom"; }
    else if ("release".equals(branchName)) { return "config_hom"; }
    else if ("release-r2".equals(branchName)) { return "config_hom"; }
    else if ("development".equals(branchName)) { return "config_dev"; }
    else if ("develop".equals(branchName)) { return "config_dev"; }
}
