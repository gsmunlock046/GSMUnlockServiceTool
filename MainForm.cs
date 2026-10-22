using GSMUnlockServiceTool.Services;
namespace GSMUnlockServiceTool;
public sealed class MainForm : Form
{
 readonly ListBox devices=new(); readonly TextBox log=new(); readonly Label status=new(); readonly TextBox manufacturer=new(); readonly TextBox model=new(); readonly ComboBox mode=new(); string? selectedAdbSerial;
 public MainForm()
 {
  Text="GSM Unlock Service Tool"; MinimumSize=new Size(920,620); Size=new Size(1140,760); StartPosition=FormStartPosition.CenterScreen; BackColor=Color.FromArgb(15,20,32); ForeColor=Color.FromArgb(230,236,245); Font=new Font("Segoe UI",10);
  var root=new TableLayoutPanel{Dock=DockStyle.Fill,ColumnCount=2,RowCount=3,Padding=new Padding(14)}; root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,285)); root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100)); root.RowStyles.Add(new RowStyle(SizeType.Absolute,76)); root.RowStyles.Add(new RowStyle(SizeType.Percent,100)); root.RowStyles.Add(new RowStyle(SizeType.Absolute,32)); Controls.Add(root);
  var header=new Panel{Dock=DockStyle.Fill,BackColor=Color.FromArgb(22,30,47)}; header.Controls.Add(new Label{Text="GSM UNLOCK SERVICE",Font=new Font("Segoe UI",17,FontStyle.Bold),ForeColor=Color.FromArgb(78,164,255),AutoSize=true,Location=new Point(18,8)}); header.Controls.Add(new Label{Text="USB  •  IPHONE  •  ADB  •  FASTBOOT  •  DIAGNÓSTICO",AutoSize=true,Location=new Point(20,43),ForeColor=Color.Silver}); root.Controls.Add(header,0,0); root.SetColumnSpan(header,2);
  var left=new TableLayoutPanel{Dock=DockStyle.Fill,RowCount=9,Padding=new Padding(0,12,12,0)}; for(int i=0;i<7;i++)left.RowStyles.Add(new RowStyle(SizeType.Absolute,43)); left.RowStyles.Add(new RowStyle(SizeType.Percent,100)); left.RowStyles.Add(new RowStyle(SizeType.Absolute,40));
  AddButton(left,"Verificar USB",0,ScanUsb); AddButton(left,"Procurar aparelhos ADB",1,ScanAdb); AddButton(left,"Procurar aparelhos Fastboot",2,ScanFastboot); AddButton(left,"Ler informações ADB",3,ReadInfo); AddButton(left,"Ler iPhone USB",4,ReadIPhone); AddButton(left,"Buscar aplicativos Cleaner",5,FindCleanerApps); AddButton(left,"Remover Cleaner selecionado",6,RemoveCleanerApp);
  devices.Dock=DockStyle.Fill; devices.BackColor=Color.FromArgb(22,30,47); devices.ForeColor=Color.White; left.Controls.Add(devices,0,7); AddButton(left,"Limpar log",8,()=>log.Clear()); root.Controls.Add(left,0,1);
  var right=new TableLayoutPanel{Dock=DockStyle.Fill,RowCount=3,Padding=new Padding(0,12,0,0)}; right.RowStyles.Add(new RowStyle(SizeType.Absolute,145)); right.RowStyles.Add(new RowStyle(SizeType.Percent,100)); right.RowStyles.Add(new RowStyle(SizeType.Absolute,42));
  var group=new GroupBox{Text="Consulta preliminar de compatibilidade",Dock=DockStyle.Fill,ForeColor=Color.White}; var g=new TableLayoutPanel{Dock=DockStyle.Fill,ColumnCount=4,RowCount=2,Padding=new Padding(8)}; g.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,85));g.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));g.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,60));g.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50)); g.RowStyles.Add(new RowStyle(SizeType.Absolute,42));g.RowStyles.Add(new RowStyle(SizeType.Absolute,42));
  manufacturer.Dock=DockStyle.Fill;manufacturer.Text="Xiaomi";model.Dock=DockStyle.Fill;model.PlaceholderText="Ex.: Redmi Note 12";mode.Dock=DockStyle.Fill;mode.DropDownStyle=ComboBoxStyle.DropDownList;mode.Items.AddRange(new object[]{"USB","ADB","Fastboot"});mode.SelectedIndex=0;
  g.Controls.Add(Lbl("Fabricante"),0,0);g.Controls.Add(manufacturer,1,0);g.Controls.Add(Lbl("Modelo"),2,0);g.Controls.Add(model,3,0);g.Controls.Add(Lbl("Modo"),0,1);g.Controls.Add(mode,1,1);var check=Btn("Consultar");check.Click+=(_,_)=>Log(CompatibilityService.Check(manufacturer.Text,model.Text,mode.Text));g.Controls.Add(check,3,1);group.Controls.Add(g);right.Controls.Add(group,0,0);
  log.Dock=DockStyle.Fill;log.Multiline=true;log.ReadOnly=true;log.ScrollBars=ScrollBars.Both;log.BackColor=Color.FromArgb(8,12,21);log.ForeColor=Color.FromArgb(190,220,200);log.Font=new Font("Consolas",9.5f);log.WordWrap=false;right.Controls.Add(log,0,1);right.Controls.Add(new Label{Text="Leitura somente informativa. iPhone requer desbloqueio e confiança no computador.",Dock=DockStyle.Fill,ForeColor=Color.Silver,TextAlign=ContentAlignment.MiddleLeft},0,2);root.Controls.Add(right,1,1);
  status.Dock=DockStyle.Fill;status.TextAlign=ContentAlignment.MiddleLeft;status.Text="Pronto • conecte um aparelho USB";root.Controls.Add(status,0,2);root.SetColumnSpan(status,2);Log("Bem-vindo. Para iPhone, desbloqueie o aparelho e toque em Confiar neste computador.");
 }
 static Label Lbl(string t)=>new(){Text=t,Dock=DockStyle.Fill,TextAlign=ContentAlignment.MiddleLeft,ForeColor=Color.White};
 static Button Btn(string t)=>new(){Text=t,Dock=DockStyle.Fill,FlatStyle=FlatStyle.Flat,BackColor=Color.FromArgb(35,105,190),ForeColor=Color.White,Margin=new Padding(3)};
 void AddButton(TableLayoutPanel p,string text,int row,Action action){var b=Btn(text);b.Click+=(_,_)=>{try{action();}catch(Exception e){Log("Erro: "+e.Message);}};p.Controls.Add(b,0,row);}
 void Log(string text){if(InvokeRequired){BeginInvoke(()=>Log(text));return;}log.AppendText($"[{DateTime.Now:HH:mm:ss}] {text}{Environment.NewLine}");status.Text=text.Split('\n')[0];}
 void ScanUsb(){var list=DeviceService.GetUsbDevices();Log($"Dispositivos USB/Plug and Play: {list.Count}");foreach(var d in list.Take(80))Log($"{d.Status} | {d.Name} | {d.DeviceId}");}
 async void ScanAdb(){selectedAdbSerial=null; var r=await AndroidService.RunAdbAsync("devices -l");Log("ADB: "+(r.Success?"comando executado":r.Error));if(!string.IsNullOrWhiteSpace(r.Output))Log(r.Output.Trim());if(!string.IsNullOrWhiteSpace(r.Error))Log(r.Error.Trim());devices.Items.Clear();foreach(var d in await AndroidService.GetAdbDevicesAsync())devices.Items.Add($"{d.Serial} [{d.State}]");if(devices.Items.Count==0)Log("Nenhum ADB detectado. Ative Depuração USB e autorize este computador no telefone."); else Log("Selecione um aparelho com estado [device] antes de buscar aplicativos.");}
 async void ScanFastboot(){var r=await AndroidService.RunFastbootAsync("devices");Log("Fastboot: "+(r.Success?"comando executado":r.Error));if(!string.IsNullOrWhiteSpace(r.Output))Log(r.Output.Trim());if(!string.IsNullOrWhiteSpace(r.Error))Log(r.Error.Trim());devices.Items.Clear();foreach(var d in await AndroidService.GetFastbootDevicesAsync())devices.Items.Add(d);if(devices.Items.Count==0)Log("Nenhum aparelho Fastboot detectado. Verifique modo e driver.");}
 async void FindCleanerApps(){
  if(devices.SelectedItem is null){Log("Primeiro clique em Procurar aparelhos ADB e selecione um aparelho autorizado com estado [device].");return;}
  var selected=devices.SelectedItem.ToString()!;
  if(!selected.Contains("[device]",StringComparison.OrdinalIgnoreCase)){Log("O aparelho selecionado não está autorizado no ADB. Desbloqueie-o e aceite a depuração USB.");return;}
  selectedAdbSerial=selected.Split(' ')[0];
  try{
   Log("Buscando pacotes instalados com 'cleaner' no identificador...");
   var matches=await CleanerService.FindCleanerPackagesAsync(selectedAdbSerial);
   devices.Items.Clear();
   foreach(var item in matches)devices.Items.Add(item.PackageId);
   if(matches.Count==0)Log("Nenhum pacote com 'cleaner' no identificador foi encontrado.");
   else Log($"Encontrados {matches.Count} pacote(s). Selecione um para revisar e remover.");
  }catch(Exception ex){Log("Falha na busca: "+ex.Message);}
 }
 async void RemoveCleanerApp(){
  if(string.IsNullOrWhiteSpace(selectedAdbSerial)){Log("Busque e selecione um aparelho ADB antes de procurar aplicativos Cleaner.");return;}
  if(devices.SelectedItem is null){Log("Selecione um pacote Cleaner na lista.");return;}
  var packageId=devices.SelectedItem.ToString()!;
  var confirm=MessageBox.Show($"Confirma remover o pacote abaixo apenas para o usuário atual do Android?\n\n{packageId}\n\nIsso pode desativar recursos do aparelho se for um app do fabricante.","Confirmar remoção",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
  if(confirm!=DialogResult.Yes)return;
  try{Log(await CleanerService.RemoveForCurrentUserAsync(selectedAdbSerial,packageId));}
  catch(Exception ex){Log("Falha na remoção: "+ex.Message);}
 }
 async void ReadInfo(){if(devices.SelectedItem is null){Log("Selecione um dispositivo ADB na lista.");return;}var serial=devices.SelectedItem.ToString()!.Split(' ')[0];Log(await AndroidService.GetDeviceInfoAsync(serial));}
 async void ReadIPhone(){Log("Procurando iPhone confiável conectado por USB...");var found=await IPhoneService.ListDevicesAsync();if(found.Count==0){Log("iPhone não detectado. Instale libimobiledevice em tools\\libimobiledevice, conecte por cabo, desbloqueie e aceite Confiar neste computador.");return;}devices.Items.Clear();foreach(var id in found)devices.Items.Add("iPhone "+id);Log(await IPhoneService.ReadFullReportAsync(found[0]));}
}