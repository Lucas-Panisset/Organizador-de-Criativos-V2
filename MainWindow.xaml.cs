using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;

namespace Polozi_Criativo
{
    public partial class MainWindow : Window
    {
        string csvPath;
        List<string> lista = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            txtBPath.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "POLOZI");
            csvPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Organizador");


            spreadSheetSelector();


        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            ArchiveReorganize(txtBArchiveLink.Text);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            folderDialog.ShowDialog();
            var folderPath = folderDialog.SelectedPath;

            txtBArchiveLink.Text = folderPath;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            folderDialog.ShowDialog();
            var folderPath = folderDialog.SelectedPath;

            txtBPath.Text = folderPath;
        }

        private void ArchiveReorganize(string sourcePath)
        {
            // Set the destination path
            string destinationPath = Path.Combine(txtBPath.Text, "POLOZI");


            // Create the destination folder if it doesn't already exist
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            // Get all image files in the source folder and its subfolders
            var imageFiles = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories)
                .Where(file => file.EndsWith(".jpg") || file.EndsWith(".png") || file.EndsWith(".bmp"))
                .ToList();


            var filepath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            if (!Directory.Exists(filepath))     
            {
                Directory.CreateDirectory(filepath);
            
            }

            // Iterate through each image file and move it to the corresponding subfolder in the destination path
            foreach (var imageFile in imageFiles)
            {
                try
                {
                    // Extract the city and state names from the image file name
                    var fileName = Path.GetFileNameWithoutExtension(imageFile);
                    var cityState = fileName.Substring(0, fileName.IndexOf("-") + 4).Trim();

                    StringBuilder sb = new(fileName);

                    



                    // Extract the image number from the image file name
                    var imageNumber = 0;
                    if (!int.TryParse(fileName.Substring(fileName.Length - 2), out imageNumber))
                    {
                        int.TryParse(fileName.Substring(fileName.Length - 1), out imageNumber);
                    }

                    var subfolder ="";
                    foreach(string line in lista)
                    {
                        string[] dados = line.Split(';');
                        int min = int.Parse(dados[0]);
                        int max = int.Parse(dados[1]);
                        string name = dados[2];
                        if(imageNumber>= min && imageNumber<= max)
                        {
                            subfolder = name;
                        }
                        
                    }


                    // Create the destination folder path and subfolder paths
                    var destinationFolderPath = Path.Combine(destinationPath, cityState);
                    var contentSubfolderPath = Path.Combine(destinationFolderPath, subfolder);
                    var additionalContentSubfolderPath = Path.Combine(destinationFolderPath,subfolder);

                    // Create the destination folders if they don't already exist
                    Directory.CreateDirectory(destinationFolderPath);
                    Directory.CreateDirectory(contentSubfolderPath);
                    
                        File.Move(imageFile, Path.Combine(additionalContentSubfolderPath, Path.GetFileName(imageFile)));
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error processing image file {imageFile}: {ex.Message}");
                }
            }

            Application.Current.Shutdown();
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void spreadsheetSelector_Click(object sender, RoutedEventArgs e)
        {
            spreadSheetSelector();
        }
        private void spreadSheetSelector()
        {
            using (var stream = new FileStream(Path.Combine(csvPath, "standard.csv"), FileMode.OpenOrCreate))
            {
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lista.Add(line);
                    }
                }
            }
        }
    }
}
