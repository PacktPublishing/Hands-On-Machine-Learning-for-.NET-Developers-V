using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace WebcamEmotionDetector
{
    public partial class Form1 : Form
    {
        // For handling the camera
        private VideoCapture _videoCapturer;
        private Thread _cameraThread;
        private bool _isCameraRunning;

        // For thread synchronization when capturing and scoring images
        readonly object _updateImageLock = new object();

        // ONNX model scorer
        readonly FacialExpressionDetector.FacialExpressionDetector _detector = new FacialExpressionDetector.FacialExpressionDetector();

        // Labels to display results
        readonly Label[] _emotionLabels = new Label[8];


        public Form1()
        {
            InitializeComponent();

            AddResultLabels();
        }

        private void AddResultLabels()
        {
            for (var index = 0; index < _emotionLabels.Length; index++)
            {
                var emotionLabel = new Label
                {
                    AutoSize = true,
                    Font = new Font("Calibri", 13.8F, FontStyle.Bold,
                        GraphicsUnit.Point, 0),
                    Location = new System.Drawing.Point(13, 10),
                    Size = new System.Drawing.Size(200, 30),
                    TabIndex = 0,
                    Text = $@"{"0%".PadRight(4)} {FacialExpressionDetector.FacialExpressionDetector.FERPlusOnnxConfig.Labels[index]}"
                };

                _emotionLabels[index] = emotionLabel;
                flowLayoutPanel1.Controls.Add(_emotionLabels[index]);
            }
        }

        private void btnCamera_Click(object sender, EventArgs e)
        {
            if (btnCamera.Text.Equals("Start"))
            {
                _cameraThread = new Thread(CaptureCameraCallback);
                _cameraThread.Start();

                btnCamera.Text = "Stop";
                _isCameraRunning = true;
            }
            else
            {
                _videoCapturer.Release();

                btnCamera.Text = "Start";
                _isCameraRunning = false;
            }
        }

        private void CaptureCameraCallback()
        {
            var videoFrame = new Mat();
            _videoCapturer = new VideoCapture(0);
            _videoCapturer.Open(0);

            //Calculate the square in the middle of the image
            var minimumSide = Math.Min(_videoCapturer.FrameHeight, _videoCapturer.FrameWidth);
            var middleSquare = new Rectangle((_videoCapturer.FrameWidth - minimumSide) / 2, (_videoCapturer.FrameHeight - minimumSide) / 2, minimumSide, minimumSide);

            if (_videoCapturer.IsOpened())
            {
                while (_isCameraRunning)
                {
                    _videoCapturer.Read(videoFrame);

                    Bitmap capturedImage;
                    try
                    {
                        // Clone the image so it is not disposed before use
                        // (and only use the square in the middle, since the ONNX model takes a square image as input)
                        capturedImage = videoFrame.ToBitmap().Clone(middleSquare, PixelFormat.DontCare);
                    }
                    catch
                    {
                        continue;
                    }

                    // Make sure that image is not updated in the middle of scoring
                    bool lockTaken = false;
                    try
                    {
                        Monitor.TryEnter(_updateImageLock, 0, ref lockTaken);
                        if (lockTaken)
                        {
                            // Clone so it is not disposed when capturedImage is disposed
                            _lastImage = capturedImage.Clone() as Bitmap;
                        }
                    }
                    finally
                    {
                        if (lockTaken)
                            Monitor.Exit(_updateImageLock);
                    }

                    picDisplay.Image?.Dispose();

                    try
                    {
                        picDisplay.Image = capturedImage;
                    } catch { /* Better luck next frame */ }

                    Application.DoEvents();
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isCameraRunning)
                _videoCapturer.Release();
        }

        private Bitmap _lastImage;

        private void ScoreAndUpdate(Bitmap image)
        {
            var result = _detector.DetectEmotionInBitmap(image);

            // Update label texts
            for (int i = 0; i < result.EmotionProbabilities.Count; i++)
            {
                _emotionLabels[i].Text = $@"{result.EmotionProbabilities[i].probability:P} {result.EmotionProbabilities[i].emotion}";
                _emotionLabels[i].BackColor = SystemColors.Control;
                _emotionLabels[i].ForeColor = SystemColors.ControlText;

            }

            // Highlight the highest probablility that is not neutral
            var highestProbability = result.EmotionProbabilities
                                .Skip(1)
                                .Select((item, index) => (Probability: item.probability, Emotion: item.emotion, Index: index))
                                .Max();


            //Select neutral if no other emotion is above 10%
            if (highestProbability.Probability < .1)
            {
                picEmotion.Image = picNeutral.Image;
            }
            else
            {
                picEmotion.Image = new[] { picHappy, picSurprise, picSad, picAngry, picFrown, picFear, picFrown }[highestProbability.Index].Image;

                _emotionLabels[highestProbability.Index + 1].ForeColor = SystemColors.HighlightText;
                _emotionLabels[highestProbability.Index + 1].BackColor = SystemColors.Highlight;
            }

        }

        private void updateEmotionTimer_Tick(object sender, EventArgs e)
        {
            // Scoring takes some time, so to not block or pile up calls 
            // we do this in a separate timer instead of the camera callback

            if (_isCameraRunning)
            {

                // Since this is called form another thread than the UI thread
                // We need to use BeginInvoke
                BeginInvoke(new MethodInvoker(delegate
                {
                    bool lockTaken = false;

                    try
                    {
                        // Make sure the image cannot be replaced by a new one
                        // in the middle of scoring by using a lock
                        Monitor.TryEnter(_updateImageLock, 0, ref lockTaken);
                        if (lockTaken)
                        {
                            ScoreAndUpdate(_lastImage);
                        }
                    }
                    finally
                    {
                        // Always release the lock
                        if (lockTaken)
                            Monitor.Exit(_updateImageLock);
                    }
                }
                ));
            }
        }

        private void lnkCredit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.fontawesome.com");
        }

        private void lnkLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://fontawesome.com/license/free");
        }


    }
}
