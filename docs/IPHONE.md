# Leitura de iPhone por USB

O módulo usa as ferramentas open-source libimobiledevice para consultar dados que o iPhone permite expor por USB. Instale uma distribuição confiável e compatível com Windows e coloque `idevice_id.exe`, `ideviceinfo.exe`, `idevicediagnostics.exe` e suas DLLs necessárias em `tools/libimobiledevice/` ao lado do executável. Essas ferramentas não são incluídas automaticamente no EXE.

1. Conecte o iPhone com cabo de dados e desbloqueie a tela.
2. Toque em **Confiar neste computador** e informe o código, se solicitado.
3. Clique em **Ler iPhone USB**.
4. O relatório tenta ler UDID, nome, tipo de produto/modelo, versão/build do iOS, número de série, IMEI/IMEI secundário/MEID quando expostos, firmware do modem, endereços MAC e informações de armazenamento.
5. A ferramenta também tenta solicitar diagnósticos, que podem incluir dados da bateria conforme a versão de iOS e a ferramenta usada.

## Limitações importantes
- O iOS não garante que IMEI, saúde da bateria ou porcentagem sejam acessíveis a ferramentas de terceiros em todas as versões/modelos.
- Se o relatório não trouxer capacidade/saúde da bateria, isso não pode ser deduzido com segurança. Não exibe valores inventados.
- É necessário confiar no computador; não contorna bloqueio de tela, pareamento ou proteções Apple.
- Use somente em aparelhos próprios ou com autorização do cliente.