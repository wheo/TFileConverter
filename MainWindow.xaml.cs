using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using MessageBox = System.Windows.MessageBox;
using System.Threading;

namespace TFileConverter
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {        
        public MainWindow()
        {   
            InitializeComponent();
            LoadConfig();                           
        }

        private bool LoadConfig()
        {
            // config.json 읽기
            String jsonString;
            try
            {
                vo.JsonConfig jsonConfig = vo.JsonConfig.getInstance();
                if (!File.Exists(jsonConfig.configFileName))
                {
                    MessageBox.Show("config.json 파일이 없습니다.\n환경설정 파일을 읽지 못했습니다.\n기본값으로 설정합니다.", "경고", MessageBoxButton.OK);
                    
                    jsonString = JsonSerializer.Serialize(jsonConfig);
                    File.WriteAllText(jsonConfig.configFileName, jsonString);
                }
                else
                {
                    jsonString = File.ReadAllText(jsonConfig.configFileName);
                    jsonConfig = JsonSerializer.Deserialize<vo.JsonConfig>(jsonString);
                    TxtBoxInputPath.Text = jsonConfig.CurrentInputPath;
                    TxtBoxOutputPath.Text = jsonConfig.CurrentOutputPath;
                    ChkIsInputPathReculsive.IsChecked = jsonConfig.IsRecursive;
                }
            }
            catch (Exception e)
            {

            }
            
            return true;            
        }

        private void BtnInputPath_Click(object sender, RoutedEventArgs e)
        {            
            System.Windows.Forms.FolderBrowserDialog inputFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            
            inputFolderBrowserDialog.Description =
            "폴더를 선택해 주세요";

            // Do not allow the user to create new files via the FolderBrowserDialog.
            inputFolderBrowserDialog.ShowNewFolderButton = true;

            // Default to the My Documents folder.
            // 마지막에 선택한 폴더를 파일로 저장함 그렇지 않으면 실행파일의 폴더를 기본으로 함
            //this.folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Personal;
            //this.folderBrowserDialog1.RootFolder = Environment.SpecialFolder
            inputFolderBrowserDialog.SelectedPath = @"C:\";

            // Show the FolderBrowserDialog.
            DialogResult result = inputFolderBrowserDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                if ( inputFolderBrowserDialog.SelectedPath.ToLower() == @"c:\")
                {
                    MessageBox.Show(@"C:\ 는 선택 할 수 없습니다.", "경고", MessageBoxButton.OK);
                    return;
                }
                TxtBoxInputPath.Text = inputFolderBrowserDialog.SelectedPath;
            }            
        }

        private void BtnOutputPath_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog outputFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();

            outputFolderBrowserDialog.Description =
            "폴더를 선택해 주세요";

            // Do not allow the user to create new files via the FolderBrowserDialog.
            outputFolderBrowserDialog.ShowNewFolderButton = true;

            // Default to the My Documents folder.
            // 마지막에 선택한 폴더를 파일로 저장함 그렇지 않으면 실행파일의 폴더를 기본으로 함
            //this.folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Personal;
            //this.folderBrowserDialog1.RootFolder = Environment.SpecialFolder
            outputFolderBrowserDialog.SelectedPath = @"C:\";

            // Show the FolderBrowserDialog.
            DialogResult result = outputFolderBrowserDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                //좀 더 확실한 방법으로 처리해야함
                if (outputFolderBrowserDialog.SelectedPath.ToLower() == @"c:\")
                {
                    MessageBox.Show(@"C:\ 는 선택 할 수 없습니다.", "경고", MessageBoxButton.OK);
                    return;
                }
                TxtBoxOutputPath.Text = outputFolderBrowserDialog.SelectedPath;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //공백 체크
            //리스트에 같은 값이 오면 안됨
            vo.LvInfoItem item = new vo.LvInfoItem();
            List<vo.LvInfoItem> itemList = vo.LvInfoItem.getInstance();
            item.index = itemList.Count;
            item.sourceString = TxtBoxFromWord.Text;
            item.destnationString = TxtBoxToWord.Text;            
            itemList.Add(item);
            LvInfo.ItemsSource = null;
            LvInfo.ItemsSource = itemList;
        }

        private void BtnChange_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            //변환 눌렀을 때 UI Block 걸리지 않아야함
            //진행 상황을 표시해 줄 수 있으면 좋겠음

            //리스트 검증해야 함

            //경로 검증 해야함
            String srcPath = TxtBoxInputPath.Text;
            String dstPath = TxtBoxOutputPath.Text;

            if ( !Directory.Exists(srcPath))
            {             
                MessageBox.Show("원본폴더 경로가 형식에 맞지 않습니다.", "경고", MessageBoxButton.OK);
                return;
            }
            
            if ( !Directory.Exists(dstPath))
            {                
                MessageBox.Show("복사될폴더 경로가 형식에 맞지 않습니다.", "경고", MessageBoxButton.OK);
                return;
            }
            
            if ( LvInfo.Items.Count == 0)
            {
                MessageBox.Show("문자열 등록을 먼저 해주세요.", "경고", MessageBoxButton.OK);
                return;
            }

            util.DirectoryUtil.SearchOption option = util.DirectoryUtil.SearchOption.NonRecursive;

            if ( ChkIsInputPathReculsive.IsChecked == true)
            {
                option = util.DirectoryUtil.SearchOption.Recursive;
            }
            
            ModalWindow progressModal = new ModalWindow();
            progressModal.ShowDialog();
        }        

        private void TxtBoxFromWord_TextChanged(object sender, TextChangedEventArgs e)
        {
            var thisTextBox = sender as System.Windows.Controls.TextBox;            
            if (!String.IsNullOrEmpty(thisTextBox.Text) && !String.IsNullOrEmpty(TxtBoxToWord.Text))
            {
                BtnAdd.IsEnabled = true;
            }
            else
            {
                BtnAdd.IsEnabled = false;
            }
        }

        private void TxtBoxToWord_TextChanged(object sender, TextChangedEventArgs e)
        {
            var thisTextBox = sender as System.Windows.Controls.TextBox;
            if (!String.IsNullOrEmpty(thisTextBox.Text) && !String.IsNullOrEmpty(TxtBoxFromWord.Text))
            {
                BtnAdd.IsEnabled = true;
            }
            else
            {
                BtnAdd.IsEnabled = false;
            }
        }

        private void TxtBoxInputPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            var thisTextBox = sender as System.Windows.Controls.TextBox;
            if ( !String.IsNullOrEmpty(thisTextBox.Text) )
            {
                ChkIsInputPathReculsive.IsEnabled = true;
            } else
            {
                ChkIsInputPathReculsive.IsEnabled = false;
            }
            vo.JsonConfig jsonConfig = vo.JsonConfig.getInstance();
            jsonConfig.CurrentInputPath = thisTextBox.Text;
            String jsonString = JsonSerializer.Serialize(jsonConfig);
            File.WriteAllText(jsonConfig.configFileName, jsonString);
        }        

        private void TxtBoxOutputPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            var thisTextBox = sender as System.Windows.Controls.TextBox;
            vo.JsonConfig jsonConfig = vo.JsonConfig.getInstance();
            jsonConfig.CurrentOutputPath = thisTextBox.Text;
            String jsonString = JsonSerializer.Serialize(jsonConfig);
            File.WriteAllText(jsonConfig.configFileName, jsonString);
        }

        private void LvInfo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(LvInfo.SelectedItems.Count == 1)
            {
                MessageBox.Show("체크체크");
            }
        }

        private void ChkIsInputPathReculsive_Checked(object sender, RoutedEventArgs e)
        {
            var thisCheckBox = sender as System.Windows.Controls.CheckBox;
            vo.JsonConfig jsonConfig = vo.JsonConfig.getInstance();
            jsonConfig.IsRecursive = true;
            String jsonString = JsonSerializer.Serialize(jsonConfig);
            File.WriteAllText(jsonConfig.configFileName, jsonString);
        }

        private void ChkIsInputPathReculsive_Unchecked(object sender, RoutedEventArgs e)
        {
            var thisCheckBox = sender as System.Windows.Controls.CheckBox;
            vo.JsonConfig jsonConfig = vo.JsonConfig.getInstance();
            jsonConfig.IsRecursive = false;
            String jsonString = JsonSerializer.Serialize(jsonConfig);
            File.WriteAllText(jsonConfig.configFileName, jsonString);
        }
    }
}
