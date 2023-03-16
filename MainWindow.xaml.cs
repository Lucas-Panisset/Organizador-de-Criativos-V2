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
        public MainWindow()
        {
            InitializeComponent();
            txtBPath.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "POLOZI");
            csvPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Organizador");


            var lista = new List<string>();

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
                    switch (imageNumber)
                    {
                        case 1:
                        case 2:
                            subfolder = "Alerta";
                            break;
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                            subfolder = "Arco";
                            break;
                        case 7:
                        case 8:
                            subfolder = "Descritivo";
                            break;
                        case 9:
                        case 10:
                            subfolder = "Evento";
                            break;
                        case 11:
                        case 12:
                            subfolder = "Folha";
                            break;
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                            subfolder = "Força";
                            break;
                        case 17:
                        case 18:
                            subfolder = "Janela";
                            break;
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                            subfolder = "Masterclass";
                            break;
                        case 23:
                        case 24:
                            subfolder = "Mental";
                            break;
                        case 25:
                        case 26:
                            subfolder = "Padrão";
                            break;
                        case 27:
                        case 28:
                            subfolder = "Palestrante";
                            break;
                        case 29:
                            subfolder = "Palestrante2";
                            break;
                        case 30:
                        case 31:
                        case 32:
                            subfolder = "Pessoas";
                            break;
                        case 33:
                        case 34:
                            subfolder = "Plateia";
                            break;
                        case 35:
                        case 36:
                        case 37:
                        case 38:
                        case 39:
                        case 40:
                            subfolder = "Polozi";
                            break;
                        default:
                            subfolder = "indefinidos";
                            break;
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
            var lista = new List<string>();

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
