﻿using Microsoft.Win32;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;

namespace Polozi_Criativo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txtBPath.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "POLOZI");

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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

                    
                    var subfolder = sb.Remove(0, sb.ToString().IndexOf("-") + 4).ToString();



                    // Extract the image number from the image file name
                    var imageNumber = 0;
                    if (!int.TryParse(fileName.Substring(fileName.Length - 2), out imageNumber))
                    {
                        int.TryParse(fileName.Substring(fileName.Length - 1), out imageNumber);
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
    }
}
