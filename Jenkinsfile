pipeline {
    environment {
      branchname =  env.BRANCH_NAME.toLowerCase()
      kubeconfig = getKubeconf(env.branchname)
      registryCredential = 'jenkins_registry'
      namespace = "${env.branchname == 'pre-prod' ? 'sme-novosgp-d1' : env.branchname == 'development' ? 'novosgp-dev' : env.branchname == 'release' ? 'novosgp-hom' : env.branchname == 'release-r2' ? 'novosgp-hom2' : 'sme-novosgp' }"
           
    }
  
      agent {
      kubernetes { label 'builder' }
    }

    options {
      timestamps ()
      buildDiscarder(logRotator(numToKeepStr: '10', artifactNumToKeepStr: '5'))
      disableConcurrentBuilds()
      skipDefaultCheckout()
    }
  
    stages {

        stage('CheckOut') {            
            steps { checkout scm }            
        }
   
   
        stage('Sonar & Testes') {
        when { anyOf { branch 'master'; branch 'main'; branch 'pre-prod'; branch "story/*"; branch 'development'; branch 'release'; branch 'release-r2'; branch 'infra/*'; } } 
        parallel {
          stage('TesteIntegracao & build'){
            agent { kubernetes {
               label 'dotnet5-sonar'
               defaultContainer 'dotnet5-sonar'
              }
            }
            steps{
              checkout scm
              script{
                  withSonarQubeEnv('sonarqube-local'){
                    sh 'dotnet-sonarscanner begin /k:"SME-NovoSGP" /d:sonar.cs.opencover.reportsPaths="teste/SME.SGP.TesteIntegracao/coverage.opencover.xml" /d:sonar.coverage.exclusions="**Test*.cs, **/*SME.SGP.Dados.*, **/*SME.SGP.Dominio.Interfaces, **/*SME.SGP.Api, **/*SME.SGP.Infra, **/*SME.SGP.IoC, **/*SME.SGP.Infra.*, **/*/Workers/*, **/*/Hub/*"'
                    sh 'dotnet test teste/SME.SGP.TesteIntegracao /p:CollectCoverage=true /p:CoverletOutputFormat=opencover'
                    sh 'dotnet-sonarscanner end'
                  }
               }
            }
          }

          stage('TesteIntegracao.AEE'){
            agent { kubernetes {
               label 'dotnet5-sonar'
               defaultContainer 'dotnet5-sonar'
              }
            }
            steps{
              checkout scm
              script{
                  withSonarQubeEnv('sonarqube-local'){
                    sh 'dotnet-sonarscanner begin /k:"SME-NovoSGP" /d:sonar.cs.opencover.reportsPaths="teste/SME.SGP.TesteIntegracao.AEE/coverage.opencover.xml" /d:sonar.coverage.exclusions="**Test*.cs, **/*SME.SGP.Dados.*, **/*SME.SGP.Dominio.Interfaces, **/*SME.SGP.Api, **/*SME.SGP.Infra, **/*SME.SGP.IoC, **/*SME.SGP.Infra.*, **/*/Workers/*, **/*/Hub/*"'
                    sh 'dotnet test teste/SME.SGP.TesteIntegracao.AEE /p:CollectCoverage=true /p:CoverletOutputFormat=opencover'
                    sh 'dotnet-sonarscanner end'
                  }
               }
            }
          }

          stage('TesteIntegracao.Aula'){
            agent { kubernetes {
               label 'dotnet5-sonar'
               defaultContainer 'dotnet5-sonar'
              }
            }
            steps{
              checkout scm
              script{
                  withSonarQubeEnv('sonarqube-local'){
                    sh 'dotnet-sonarscanner begin /k:"SME-NovoSGP" /d:sonar.cs.opencover.reportsPaths="teste/SME.SGP.TesteIntegracao.Aula/coverage.opencover.xml" /d:sonar.coverage.exclusions="**Test*.cs, **/*SME.SGP.Dados.*, **/*SME.SGP.Dominio.Interfaces, **/*SME.SGP.Api, **/*SME.SGP.Infra, **/*SME.SGP.IoC, **/*SME.SGP.Infra.*, **/*/Workers/*, **/*/Hub/*"'
                    sh 'dotnet test teste/SME.SGP.TesteIntegracao.Aula /p:CollectCoverage=true /p:CoverletOutputFormat=opencover'
                    sh 'dotnet-sonarscanner end'
                  }
               }
            }
          }

          stage('TesteIntegracao.Fechamento'){
            agent { kubernetes {
               label 'dotnet5-sonar'
               defaultContainer 'dotnet5-sonar'
              }
            }
            steps{
              checkout scm
              script{
                  withSonarQubeEnv('sonarqube-local'){
                    sh 'dotnet-sonarscanner begin /k:"SME-NovoSGP" /d:sonar.cs.opencover.reportsPaths="teste/SME.SGP.TesteIntegracao.Fechamento/coverage.opencover.xml" /d:sonar.coverage.exclusions="**Test*.cs, **/*SME.SGP.Dados.*, **/*SME.SGP.Dominio.Interfaces, **/*SME.SGP.Api, **/*SME.SGP.Infra, **/*SME.SGP.IoC, **/*SME.SGP.Infra.*, **/*/Workers/*, **/*/Hub/*"'
                    sh 'dotnet test teste/SME.SGP.TesteIntegracao.Fechamento /p:CollectCoverage=true /p:CoverletOutputFormat=opencover'
                    sh 'dotnet-sonarscanner end'
                  }
               }
            }
          }

          stage('TesteIntegracao.Frequencia'){
            agent { kubernetes {
               label 'dotnet5-sonar'
               defaultContainer 'dotnet5-sonar'
              }
            }
            steps{
              checkout scm
              script{
                  withSonarQubeEnv('sonarqube-local'){
                    sh 'dotnet-sonarscanner begin /k:"SME-NovoSGP" /d:sonar.cs.opencover.reportsPaths="teste/SME.SGP.TesteIntegracao.Frequencia/coverage.opencover.xml" /d:sonar.coverage.exclusions="**Test*.cs, **/*SME.SGP.Dados.*, **/*SME.SGP.Dominio.Interfaces, **/*SME.SGP.Api, **/*SME.SGP.Infra, **/*SME.SGP.IoC, **/*SME.SGP.Infra.*, **/*/Workers/*, **/*/Hub/*"'
                    sh 'dotnet test teste/SME.SGP.TesteIntegracao.Frequencia /p:CollectCoverage=true /p:CoverletOutputFormat=opencover'
                    sh 'dotnet-sonarscanner end'
                  }
               }
            }
          }

          stage('TesteIntegracao.Pendencia'){
            agent { kubernetes {
               label 'dotnet5-sonar'
               defaultContainer 'dotnet5-sonar'
              }
            }
            steps{
              checkout scm
              script{
                  withSonarQubeEnv('sonarqube-local'){
                    sh 'dotnet-sonarscanner begin /k:"SME-NovoSGP" /d:sonar.cs.opencover.reportsPaths="teste/SME.SGP.TesteIntegracao.Pendencia/coverage.opencover.xml" /d:sonar.coverage.exclusions="**Test*.cs, **/*SME.SGP.Dados.*, **/*SME.SGP.Dominio.Interfaces, **/*SME.SGP.Api, **/*SME.SGP.Infra, **/*SME.SGP.IoC, **/*SME.SGP.Infra.*, **/*/Workers/*, **/*/Hub/*"'
                    sh 'dotnet build SME.SGP.sln'
                    sh 'dotnet test teste/SME.SGP.TesteIntegracao.Pendencia --no-build /p:CollectCoverage=true /p:CoverletOutputFormat=opencover'
                    sh 'dotnet-sonarscanner end'
                  }
               }
            }
          }
        }
      } 

        stage('Build') {
          when { anyOf { branch 'master'; branch 'main'; branch 'pre-prod'; branch "story/*"; branch 'development'; branch 'release'; branch 'release-r2'; branch 'infra/*'; } } 
          parallel {
            stage('sme-sgp-backend') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-sgp-backend"
                  dockerImage1 = docker.build(imagename, "-f src/SME.SGP.Api/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage1.push() } 
                }
              }
            }
            stage('sme-worker-geral') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-geral"
                  dockerImage2 = docker.build(imagename, "-f src/SME.SGP.Worker.Rabbbit/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage2.push() }   
                }
              }
            }
            stage('sme-worker-fechamento') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-fechamento"
                  dockerImage3 = docker.build(imagename, "-f src/SME.SGP.Fechamento.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage3.push() }   
                }
              }
            }
            stage('sme-worker-aee') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-aee"
                  dockerImage4 = docker.build(imagename, "-f src/SME.SGP.AEE.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage4.push() }  
                }
              }
            }
            stage('sme-worker-aula') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-aula"
                  dockerImage5 = docker.build(imagename, "-f src/SME.SGP.Aula.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage5.push() }   
                }
              }
            }
            stage('sme-worker-frequencia') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-frequencia"
                  dockerImage6 = docker.build(imagename, "-f src/SME.SGP.Frequencia.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage6.push() }   
                }
              }
            }
            stage('sme-worker-institucional') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-institucional"
                  dockerImage7 = docker.build(imagename, "-f src/SME.SGP.Institucional.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage7.push() }   
                }
              }
            }
            stage('sme-worker-pendencias') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-pendencias"
                  dockerImage8 = docker.build(imagename, "-f src/SME.SGP.Pendencias.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage8.push() }  
                }
              }
            }
            stage('sme-worker-avaliacao') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-avaliacao"
                  dockerImage9 = docker.build(imagename, "-f src/SME.SGP.Avaliacao.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage9.push() }  
                }
              }
            }
            stage('sme-worker-auditoria') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-auditoria"
                  dockerImage10 = docker.build(imagename, "-f src/SME.SGP.Auditoria.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage10.push() }  
                }
              }
            }
            stage('sme-worker-notificacoes') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-notificacoes"
                  dockerImage11 = docker.build(imagename, "-f src/SME.SGP.Notificacoes.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage11.push() }  
                }
              }
            }
            stage('sme-worker-notificacoes-hub') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-notificacoes-hub"
                  dockerImage12 = docker.build(imagename, "-f src/SME.SGP.Notificacoes.Hub/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage12.push() }   
                }
              }
            }            
            stage('sme-worker-compressao') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-compressao"
                  dockerImage13 = docker.build(imagename, "-f src/SME.SGP.ComprimirArquivos.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage13.push() }  
                }
              }
            }
            stage('sme-worker-naapa') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-naapa"
                  dockerImage14 = docker.build(imagename, "-f src/SME.SGP.NAAPA.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage14.push() }  
                }
              }
            }   
          }
    }
        stage('Deploy'){
             agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
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
                                sh('rm -f '+"$home"+'/.kube/config')
                                sh('cp $config '+"$home"+'/.kube/config')
                                sh "kubectl rollout restart deployment/sme-api -n ${namespace}"
                                sh "kubectl rollout restart deployment/sme-worker-fechamento -n ${namespace}"   
                                sh "kubectl rollout restart deployment/sme-worker-geral -n ${namespace}"   
                                sh "kubectl rollout restart deployment/sme-worker-aee -n ${namespace}"
                                sh "kubectl rollout restart deployment/sme-worker-aula -n ${namespace}"
                                sh "kubectl rollout restart deployment/sme-worker-frequencia -n ${namespace}"
                                sh "kubectl rollout restart deployment/sme-worker-institucional -n ${namespace}"
                                sh "kubectl rollout restart deployment/sme-worker-pendencias -n ${namespace}"
                                sh "kubectl rollout restart deployment/sme-worker-avaliacao -n ${namespace}"
                                sh "kubectl rollout restart deployment/sme-worker-auditoria -n ${namespace}"
                                sh "kubectl rollout restart deployment/sme-worker-notificacoes -n ${namespace}"
                                sh "kubectl rollout restart deployment/sme-worker-notificacoes-hub -n ${namespace}"
                                sh "kubectl rollout restart deployment/sme-worker-compressao -n ${namespace}"
                                sh "kubectl rollout restart deployment/sme-worker-naapa -n ${namespace}"
                                sh('rm -f '+"$home"+'/.kube/config')
                        }
                    //}
                }
            }           
        }
             
      stage('Flyway') {
        agent { kubernetes { 
                  label 'flyway'
                  defaultContainer 'flyway'
                }
              }
        when { anyOf {  branch 'master'; branch 'main'; branch 'development'; branch 'release'; branch 'release-r2'; branch 'pre-prod'; } }
        steps{
          withCredentials([string(credentialsId: "flyway_sgp_${branchname}", variable: 'url')]) {
            checkout scm
            sh 'pwd'
            sh 'ls'
            sh 'flyway -url=$url -locations="filesystem:scripts" -outOfOrder=true migrate'
             }
        }       
      }

      stage('Deploy Treinamento'){
          agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
          when { anyOf { branch 'release'; } }        
          steps {
              script{
                  try {
                      withCredentials([file(credentialsId: "${kubeconfig}", variable: 'config')]){
                          sh('rm -f '+"$home"+'/.kube/config')
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
        agent { kubernetes { 
                  label 'flyway'
                  defaultContainer 'flyway'
                }
              }
        when { anyOf {  branch 'release'; } }
        steps{
          script{
            try {
                withCredentials([string(credentialsId: "flyway_sgp_treinamento", variable: 'url')]) {
                checkout scm
                sh 'flyway -url=$url -locations="filesystem:scripts" -outOfOrder=true migrate'
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
    else if ("homolog".equals(branchName)) { return "config_release"; }
    else if ("release".equals(branchName)) { return "config_release"; }
    else if ("release-r2".equals(branchName)) { return "config_release"; }
    else if ("development".equals(branchName)) { return "config_release"; }
    else if ("develop".equals(branchName)) { return "config_release"; }
}
