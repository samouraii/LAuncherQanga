using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using laucherQange;
using Newtonsoft.Json;
using System.IO.Compression;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;

namespace LauncherQangaClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.ComponentModel.BackgroundWorker backgroundWorker1 = new BackgroundWorker();
        private System.ComponentModel.BackgroundWorker backgroundWorker2 = new BackgroundWorker();
        private const string serveurftp =  @"ftp://163.172.121.13";
        private const string serveur = "163.172.121.13";

        public MainWindow()
        {
            InitializeComponent();
            
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Process t = Process.Start("SurvivalGame.exe");

            Process.GetCurrentProcess().Kill();


        }
        int compteur = 0;
        private void miseAJour(string update,string version,string url)

        {
            operation.Content = "installation de la mise à jour";
            //string update = @"C:\Users\adrie\OneDrive\Documents\DiffEngine_src\movie.json";

            string[] lines = System.IO.File.ReadAllLines(update);


            List<filupdate> deserializedProduct = (List<filupdate>)JsonConvert.DeserializeObject(lines[0], typeof(List<filupdate>));
            
            foreach(filupdate file in deserializedProduct)
            {

                if(file.Type == 0)
                {
                    ftp ftpClient = new ftp(serveurftp, "launcher", "123456");
                    ftpClient.download(url+"\\jeu\\normal"+file.Fichier , @"."+file.Fichier, null);

                }
                else if(file.Type == 1)
                {
                    try
                    {
                        string oldFile = "." + file.Fichier;
                        string newFile = "." + file.Fichier + ".new";
                        string patchFile = "." + file.Updateur;
                        using (FileStream input = new FileStream(oldFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                        using (FileStream output = new FileStream(newFile, FileMode.Create))
                            BinaryPatchUtility.Apply(input, () => new FileStream(patchFile, FileMode.Open, FileAccess.Read, FileShare.Read), output);
                        File.Delete("." + file.Fichier);
                        File.Move(newFile, "." + file.Fichier);


                    }
                    catch (FileNotFoundException ex)
                    {
                        Console.Error.WriteLine("Could not open '{0}'.", ex.FileName);
                    }
                }
                else if(file.Type == 2)
                {

                }
                else
                {
                    MessageBox.Show("Error 203");
                }
                compteur++;
                progressabar2.Value = compteur * 100 / deserializedProduct.Count;

            }

            FileStream ver = new FileStream("version.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            ver.Write(System.Text.Encoding.UTF8.GetBytes(version), 0, System.Text.Encoding.UTF8.GetBytes(version).Length);
            ver.Close();
            Directory.Delete("./patch", true);
            File.Delete("update.zip");

        }


        private Boolean run;
        private string reponse = "";
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           

        }

        private void backgroundWorker1_change(object sender,  ProgressChangedEventArgs e)
        {
            operation.Content = "Téléchargement de la mise à jour";
            progressabar1.Value = e.ProgressPercentage;
            LbTéléchargement.Content = e.ProgressPercentage;
        }

        private void backgroundWorker1_work(object sender, DoWorkEventArgs e)
        {
            ftp ftpClient = new ftp(serveurftp, "launcher", "123456");


            ftpClient.download(reponse.Split(' ')[0] + "\\jeu\\zip\\jeu.zip", "Jeu.zip", backgroundWorker1);
            
        }
        private void backgroundWorker1_RunWorkerCompleted(
           object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
          
            else
            {
                // Finally, handle the case where the operation 
                // succeeded.
                ZipFile.ExtractToDirectory(@"Jeu.zip", @"./");
                File.Delete(@"Jeu.zip");
                string version = reponse.Split(' ')[1];
                FileStream ver = new FileStream("versionM.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                ver.Write(System.Text.Encoding.UTF8.GetBytes(version), 0, System.Text.Encoding.UTF8.GetBytes(version).Length);
                ver.Close();
                update();

            }

            // Enable the UpDown control.
          
        }
        private void backgroundWorker1_workUpdate(object sender, DoWorkEventArgs e)
        {
            ftp ftpClient = new ftp(serveurftp, "launcher", "123456");
            ftpClient.download(reponse.Split(' ')[0] + "\\update.zip", @".\update.zip", backgroundWorker1);
           
        }
        private void backgroundWorker1_RunWorkerCompletedUpdate(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }

            else
            {
                if (reponse != "ok" && reponse !="-1")
                {
                    string version = reponse.Split(' ')[1];
                    ZipFile.ExtractToDirectory(@"update.zip", @".\patch");
                    miseAJour("patch\\updateLauncherLauncher.json", version, reponse.Split(' ')[0]);

                    // 
                    //ecrire la nouvelle version

                   // File.Delete("updateLauncherLauncher.json");
                    FileStream ver = new FileStream("versionM.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    ver.Write(System.Text.Encoding.UTF8.GetBytes(version), 0, System.Text.Encoding.UTF8.GetBytes(version).Length);
                    ver.Close();


                    while (backgroundWorker1.IsBusy) { Thread.Sleep(1000); }
                    ConnectServeur s = new ConnectServeur();
                    try
                    {
                        string[] lines = System.IO.File.ReadAllLines("versionM.txt");
                        reponse = s.connect(serveur, 6000, "M " + version);
                        //envoie ligne 0 launcher, ligne 1 jeu
                        if (reponse != "ok")
                        {
                            backgroundWorker1 = new BackgroundWorker();
                            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_workUpdate);
                            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompletedUpdate);
                            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_change);
                            backgroundWorker1.WorkerReportsProgress = true;

                            backgroundWorker1.RunWorkerAsync();

                        }

                        else
                        {
                            //this.start();
                            this.update();

                        }

                        }
                    catch (Exception ex)
                    {
                        MessageBox.Show("une erreur c'est produite erreur 423" + ex.Message);
                    }
                }
                else if (reponse == "-1")
                {
                    MessageBox.Show("problème de connexion!");
                }
            }

            // Enable the UpDown control.

        }


        private void start()
        {
            BtnStart.IsEnabled = true;
            operation.Content = "Jeu à jour";
        }

        private void update()
        {
            if (File.Exists("versionJ.txt"))
            {

                string[] lines = System.IO.File.ReadAllLines("versionJ.txt");
                ConnectServeur s = new ConnectServeur();
                try
                {
                    reponse = s.connect(serveur, 6000, "J " + lines[0]);
                    
                    //envoie ligne 0 launcher, ligne 1 jeu
                    if (reponse != "ok" && reponse != "-1")
                    {
                        backgroundWorker1 = new BackgroundWorker();
                        backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_workUpdate2);
                        backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompletedUpdate2);
                        backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_change);
                        backgroundWorker1.WorkerReportsProgress = true;
                       

                        backgroundWorker1.RunWorkerAsync();
                        string version = reponse.Split(' ')[1];
                        FileStream ver = new FileStream("versionJ.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        ver.Write(System.Text.Encoding.UTF8.GetBytes(version), 0, System.Text.Encoding.UTF8.GetBytes(version).Length);
                        ver.Close();


                    }
                    else
                    {
                        this.start();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("une erreur c'est produite erreur 44" + ex.Message);
                }


            }
            else
            {
                ConnectServeur s = new ConnectServeur();
                reponse = s.connect(serveur, 6000, "J 0.0.0.0.0.0");
                if (reponse != "-1")
                {
                    // recuperer l'emplacement
                    backgroundWorker1 = new BackgroundWorker();
                    backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_work);
                    backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted2);
                    backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_change);
                    backgroundWorker1.WorkerReportsProgress = true;
                  
                    backgroundWorker1.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show("Serveur Non dipsognible");
                }
            }
        }

        private void backgroundWorker1_RunWorkerCompletedUpdate2(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }

            else
            {
                if (reponse != "ok" && reponse != "-1")
                {
                    string version = reponse.Split(' ')[1];
                    ZipFile.ExtractToDirectory(@"update.zip", @".\patch");
                    miseAJour("patch\\updateJeu.json", version, reponse.Split(' ')[0]);

                    // 
                    //ecrire la nouvelle version

                    File.Delete("updateJeu.json");
                    FileStream ver = new FileStream("versionJ.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    ver.Write(System.Text.Encoding.UTF8.GetBytes(version), 0, System.Text.Encoding.UTF8.GetBytes(version).Length);
                    ver.Close();


                    while (backgroundWorker1.IsBusy) { Thread.Sleep(1000); }
                    ConnectServeur s = new ConnectServeur();
                    try
                    {
                        string[] lines = System.IO.File.ReadAllLines("versionJ.txt");
                        reponse = s.connect(serveur, 6000, "J " + version);
                        //envoie ligne 0 launcher, ligne 1 jeu
                        if (reponse != "ok")
                        {
                            backgroundWorker1 = new BackgroundWorker();
                            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_workUpdate2);
                            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompletedUpdate2);
                            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_change);
                            backgroundWorker1.WorkerReportsProgress = true;

                            backgroundWorker1.RunWorkerAsync();

                        }

                        else
                        {
                            this.start();

                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("une erreur c'est produite erreur 45" + ex.Message);
                    }
                }
                else if (reponse == "-1")
                {
                    MessageBox.Show("problème de connexion!");
                }
            }

            // Enable the UpDown control.

        }
        private void backgroundWorker1_workUpdate2(object sender, DoWorkEventArgs e)
        {
            ftp ftpClient = new ftp(serveurftp, "launcher", "123456");
            ftpClient.download(reponse.Split(' ')[0] + "\\update.zip", @".\update.zip", backgroundWorker1);

        }
        private void backgroundWorker2_unzip(object sender, DoWorkEventArgs e)
        {
            ZipFile.ExtractToDirectory(@"Jeu.zip", @".\");

        }
        private void backgroundWorker2_unzipFin(
          object sender, RunWorkerCompletedEventArgs e)
        {
            string version = reponse.Split(' ')[1];
            FileStream ver = new FileStream("versionJ.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            ver.Write(System.Text.Encoding.UTF8.GetBytes(version), 0, System.Text.Encoding.UTF8.GetBytes(version).Length);
            ver.Close();
            File.Delete(@"Jeu.zip");
            update();

        }
            private void backgroundWorker1_RunWorkerCompleted2(
          object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }

            else
            {
                // Finally, handle the case where the operation 
                // succeeded.
                backgroundWorker2 = new BackgroundWorker();
                backgroundWorker2.DoWork += new DoWorkEventHandler(backgroundWorker2_unzip);
                backgroundWorker2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker2_unzipFin);
                backgroundWorker2.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_change);
                backgroundWorker2.WorkerReportsProgress = true;

                backgroundWorker2.RunWorkerAsync();

               

            }
            
            // Enable the UpDown control.
        }



        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            startLoaded();
        }


        private void startLoaded()
        {
            
            if (File.Exists("versionM.txt"))
            {

                string[] lines = System.IO.File.ReadAllLines("versionM.txt");
                ConnectServeur s = new ConnectServeur();
                try
                {
                    reponse = s.connect(serveur, 6000, "M " + lines[0]);
                    //envoie ligne 0 launcher, ligne 1 jeu
                    if (reponse != "ok")
                    {
                        backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_workUpdate);
                        backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompletedUpdate);
                        backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_change);
                        backgroundWorker1.WorkerReportsProgress = true;

                        backgroundWorker1.RunWorkerAsync();


                    }
                    else
                    {
                        this.update();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("une erreur c'est produite erreur 42" + ex.Message);
                }


            }
            else
            {
                ConnectServeur s = new ConnectServeur();
                reponse = s.connect(serveur, 6000, "M 0.0.0.0.0.0");
                if (reponse != "-1")
                {
                    // recuperer l'emplacement
                    backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_work);
                    backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
                    backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_change);
                    backgroundWorker1.WorkerReportsProgress = true;

                    backgroundWorker1.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show("Serveur Non dipsognible");
                }
            }
        }
    }
}
