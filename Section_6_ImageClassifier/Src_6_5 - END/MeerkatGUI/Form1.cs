using MeerkatModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MeerkatGUI
{
    public partial class Form1 : Form
    {
        private readonly Classifier _classifier = new Classifier();

        private Dictionary<string, (string name, string url)> _attributionLookup
            = new Dictionary<string, (string name, string url)>();

        public Form1()
        {
            InitializeComponent();

            // Load image files
            var imageFolder = Path.Combine(AppContext.BaseDirectory,
                @"..\..\..\images");
            
            var files = Directory.GetFiles(path: imageFolder, 
                                           searchPattern: "*.jpg", 
                                           searchOption: SearchOption.AllDirectories)
                                 .Select(f => new ImageWrapper() { Fullpath = f })
                                 .ToArray();
            
            lstFiles.Items.AddRange(files);


            // Load image attributions
            var attributions = File.ReadAllLines(Path.Combine(imageFolder, "attributions.tsv"));

            foreach (var attribution in attributions)
            {
                var parts = attribution.Split('\t');

                _attributionLookup.Add(parts[0], (parts[1], parts[2]));
            }
        }

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear current results
            lblClassification.Text = "...";
            picDisplay.Image = null;

            Application.DoEvents();

            //Show image and attribution
            ImageWrapper selectedItem = (ImageWrapper)lstFiles.SelectedItem;
            var attribution = _attributionLookup[selectedItem.FileName];

            linkLabel.Text = $"Photo by {attribution.name} on Unsplash.com";
            linkLabel.Links.Clear();
            linkLabel.Links.Add(9, attribution.name.Length, attribution.url);
            linkLabel.Links.Add(linkLabel.Text.Length - 12, 12, "https://www.unsplash.com");

            picDisplay.ImageLocation = selectedItem.Fullpath;

            Application.DoEvents();

            // Show classification
            lblClassification.Text = _classifier.Classify(selectedItem.Fullpath);
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = e.Link.LinkData as string;
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }

    }
}
