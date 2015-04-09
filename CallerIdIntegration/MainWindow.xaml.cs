using CallerIdIntegration.Common;
using CallerIdIntegration.EntityModel;
using CallerIdIntegration.UserControl;
using CID_Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CallerIdIntegration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Instance of classes
        public static UdpReceiverClass UdpReceiver = new UdpReceiverClass();
        Thread UdpReceiveThread = new Thread(UdpReceiver.UdpIdleReceive);
        string ApplicationPath = string.Empty;
        Icon _objImageModel = new Icon();
        #endregion

        #region Variables
        // Public variables
        public static string StoredName;
        public static Boolean UseStoredName;
        public static Boolean SearchContactFound;
        public static string SearchContact;
        public static string MyLine;
        public static string MyType;
        public static string MyDate;
        public static string MyTime;
        public static string MyCheckSum;
        public static string MyRings;
        public static string MyDuration;
        public static string MyIndicator;
        public static string MyNumber;
        public static string MyName;
        public static int FoundIndex;

        // Start record variables
        public static string SMyLine;
        public static string SMyTime;
        public static string SMyDate;
        public static string SMyNumber;

        // Database 

        private PCApp_TaxitechEntities _objEntities = null;
        #endregion

        #region IntializeMainWindow
        public MainWindow()
        {
            RefreshWindow();
            InitializeComponent();
            try
            {
                //System.Windows.MessageBox.Show("Hello");
                // Start listener for UDP traffic
                //ApplicationPath = System.Windows.Forms.Application.StartupPath + "\\";
                ApplicationPath = System.AppDomain.CurrentDomain.BaseDirectory;
                //  System.Windows.Forms.MessageBox.Show(ApplicationPath);
                Subscribe(UdpReceiver);
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }
        #endregion

        public void RefreshWindow()
        {
            try
            {
                DispatcherTimer localDataTimer = new DispatcherTimer();

                localDataTimer.Tick += new EventHandler(Refresh_Tick);

                localDataTimer.Interval = TimeSpan.FromMinutes(10);

                localDataTimer.Start();
            }
            catch (Exception ex)
            {
            }
        }


        public void Refresh_Tick(object sender, EventArgs e)
        {
            this.UpdateLayout();
            
        }
        #region Wait For some time
        // Wait for a certain amount of time
        private void WaitFor(int milliSeconds)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.ElapsedMilliseconds < milliSeconds)
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                sw.Stop();
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }
        #endregion

        #region Subscribe
        public void Subscribe(UdpReceiverClass u)
        {
            try
            {
                // If UDP event occurs run HeardIt method
                u.DataReceived += new UdpReceiverClass.UdpEventHandler(HeardIt);
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }
        #endregion

        #region Heard it
        private void HeardIt(UdpReceiverClass u, EventArgs e)
        {
            try
            {
                // Extract all variables from incoming data string            
                ///  this.Invoke((MethodInvoker)(() => setVars()));
                setVars();

                // ----------THIS SECTION HANDLES ALLL THE CALLER ID WINDOW VISUALS--------------
                // The code below could easily be condensed into one method handling different line numbers.
                // We used 4 occurances of the same method for 4 lines hoping that clarity could be provided.
                switch (MyLine)
                {
                    case "01":

                        switch (MyType + MyIndicator)
                        {
                            case "R":

                                //-----------------------INCOMING CALL--------------------
                                //--------------------------------------------------------
                                // Change picture of phone to ringing
                                //  picPhoneLine1.Source = new BitmapImage(new Uri(ApplicationPath + "phoneRing.png"));

                                // Light-up panel green for incoming call
                                //  GridLine1.Background = Brushes.LightGreen;

                                // Show time on panel
                                ///     lbLine1Time.Invoke((MethodInvoker)(() => lbLine1Time.Text = MyTime));
                                //  lbLine1Time.Text = MyTime;
                                //  lbLine1Number.Text = MyNumber;
                                // lbLine1Name.Text = MyName;

                                // Show name and number of panel
                                ///lbLine1Number.Invoke((MethodInvoker)(() => lbLine1Number.Text = MyNumber));
                                ///lbLine1Name.Invoke((MethodInvoker)(() => lbLine1Name.Text = MyName));
                                ///


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    picPhoneLine1.Source = new BitmapImage(new Uri(ApplicationPath + "phoneRing.png"));
                                    GridLine1.Background = Brushes.LightGreen;
                                    lbLine1Time.Text = MyTime;
                                    lbLine1Number.Text = MyNumber;
                                    lbLine1Name.Text = MyName;

                                }));







                                break;
                            case "IS":

                                // ----------------------Ring answered------------------
                                //------------------------------------------------------
                                // Light-up panel green for incoming call
                                // GridLine1.Background = Brushes.LightGreen;

                                // Show time on panel
                                /// lbLine1Time.Invoke((MethodInvoker)(() => lbLine1Time.Text = MyTime));
                                //lbLine1Time.Text = MyTime;
                                //lbLine1Number.Text = MyNumber;
                                //lbLine1Name.Text = MyName;

                                // Show name and number of panel
                                /// lbLine1Number.Invoke((MethodInvoker)(() => lbLine1Number.Text = MyNumber));
                                /// lbLine1Name.Invoke((MethodInvoker)(() => lbLine1Name.Text = MyName));
                                /// 

                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    GridLine1.Background = Brushes.LightGreen;
                                    lbLine1Time.Text = MyTime;
                                    lbLine1Number.Text = MyNumber;
                                    lbLine1Name.Text = MyName;

                                }));



                                break;
                            case "F":

                                //-----------------------Phone pickup-------------------
                                //------------------------------------------------------

                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    picPhoneLine1.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                }));

                                break;
                            case "N":

                                //-----------------------Phone hangup--------------------
                                //-------------------------------------------------------



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change panel color
                                    GridLine1.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine1.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));



                                break;
                            case "IE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change panel color
                                    GridLine1.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine1.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));
                                break;
                            case "OS":

                                //-----------------------Outgoing CALL--------------------
                                //--------------------------------------------------------

                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change picture of phone to ringing
                                    picPhoneLine1.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                    // Light-up panel blue for incoming call
                                    GridLine1.Background = Brushes.LightBlue;

                                    // Show time on panel
                                    ///  lbLine1Time.Invoke((MethodInvoker)(() => lbLine1Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///  lbLine1Number.Invoke((MethodInvoker)(() => lbLine1Number.Text = MyNumber));
                                    /// lbLine1Name.Invoke((MethodInvoker)(() => lbLine1Name.Text = MyName));
                                    /// 
                                    lbLine1Time.Text = MyTime;
                                    lbLine1Number.Text = MyNumber;
                                    lbLine1Name.Text = MyName;

                                }));




                                break;
                            case "OE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change panel color
                                    GridLine1.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine1.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));
                                break;
                            default:
                                break;
                        }

                        break;

                    case "02":

                        switch (MyType + MyIndicator)
                        {
                            case "R":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------INCOMING CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine2.Source = new BitmapImage(new Uri(ApplicationPath + "phoneRing.png"));

                                    // Light-up panel green for incoming call
                                    GridLine2.Background = Brushes.LightGreen;

                                    // Show time on panel
                                    /// lbLine2Time.Invoke((MethodInvoker)(() => lbLine2Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///  lbLine2Number.Invoke((MethodInvoker)(() => lbLine2Number.Text = MyNumber));
                                    ///  lbLine2Name.Invoke((MethodInvoker)(() => lbLine2Name.Text = MyName));
                                    ///  
                                    lbLine2Time.Text = MyTime;
                                    lbLine2Number.Text = MyNumber;
                                    lbLine2Name.Text = MyName;

                                }));




                                break;
                            case "IS":

                                // ----------------------Ring answered------------------
                                //------------------------------------------------------
                                // Light-up panel green for incoming call
                                //  GridLine2.Background = Brushes.LightGreen;

                                // Show time on panel
                                ///  lbLine2Time.Invoke((MethodInvoker)(() => lbLine2Time.Text = MyTime));

                                // Show name and number of panel
                                ///  lbLine2Number.Invoke((MethodInvoker)(() => lbLine2Number.Text = MyNumber));
                                ///  lbLine2Name.Invoke((MethodInvoker)(() => lbLine2Name.Text = MyName));
                                ///  
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    GridLine2.Background = Brushes.LightGreen;
                                    lbLine2Time.Text = MyTime;

                                }));

                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    lbLine2Number.Text = MyNumber;
                                }));

                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    lbLine2Name.Text = MyName;

                                }));
                                //lbLine2Time.Text = MyTime;
                                //  lbLine2Number.Text = MyNumber;
                                // lbLine2Name.Text = MyName;

                                break;
                            case "F":

                                //-----------------------Phone pickup-------------------
                                //------------------------------------------------------

                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    picPhoneLine2.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                }));

                                break;
                            case "N":

                                //-----------------------Phone hangup--------------------
                                //-------------------------------------------------------



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change panel color
                                    GridLine2.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine2.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));

                                break;
                            case "IE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change panel color
                                    GridLine2.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine2.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));
                                break;
                            case "OS":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Outgoing CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine2.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                    // Light-up panel blue for incoming call
                                    GridLine2.Background = Brushes.LightBlue;

                                    // Show time on panel
                                    /// lbLine2Time.Invoke((MethodInvoker)(() => lbLine2Time.Text = MyTime));

                                    ///   // Show name and number of panel
                                    ///  lbLine2Number.Invoke((MethodInvoker)(() => lbLine2Number.Text = MyNumber));
                                    /// lbLine2Name.Invoke((MethodInvoker)(() => lbLine2Name.Text = MyName));
                                    /// 
                                    lbLine2Time.Text = MyTime;
                                    lbLine2Number.Text = MyNumber;
                                    lbLine2Name.Text = MyName;

                                }));

                                break;
                            case "OE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change panel color
                                    GridLine2.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine2.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));
                                break;
                            default:
                                break;
                        }

                        break;

                    case "03":

                        switch (MyType + MyIndicator)
                        {
                            case "R":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------INCOMING CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine3.Source = new BitmapImage(new Uri(ApplicationPath + "phoneRing.png"));

                                    // Light-up panel green for incoming call
                                    GridLine3.Background = Brushes.LightGreen;

                                    // Show time on panel
                                    /// lbLine3Time.Invoke((MethodInvoker)(() => lbLine3Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///   lbLine3Number.Invoke((MethodInvoker)(() => lbLine3Number.Text = MyNumber));
                                    ///  lbLine3Name.Invoke((MethodInvoker)(() => lbLine3Name.Text = MyName));

                                    lbLine3Time.Text = MyTime;
                                    lbLine3Number.Text = MyNumber;
                                    lbLine3Name.Text = MyName;

                                }));

                                break;
                            case "IS":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // ----------------------Ring answered------------------
                                    //------------------------------------------------------
                                    // Light-up panel green for incoming call
                                    GridLine3.Background = Brushes.LightGreen;

                                    // Show time on panel
                                    ///  lbLine3Time.Invoke((MethodInvoker)(() => lbLine3Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///   lbLine3Number.Invoke((MethodInvoker)(() => lbLine3Number.Text = MyNumber));
                                    ///  lbLine3Name.Invoke((MethodInvoker)(() => lbLine3Name.Text = MyName));
                                    lbLine3Time.Text = MyTime;
                                    lbLine3Number.Text = MyNumber;
                                    lbLine3Name.Text = MyName;


                                }));

                                break;
                            case "F":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone pickup-------------------
                                    //------------------------------------------------------
                                    picPhoneLine3.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                }));

                                break;
                            case "N":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone hangup--------------------
                                    //-------------------------------------------------------

                                    // Change panel color
                                    GridLine3.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine3.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));


                                }));

                                break;
                            case "IE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone hangup--------------------
                                    //-------------------------------------------------------

                                    // Change panel color
                                    GridLine3.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine3.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));


                                }));
                                break;
                            case "OS":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Outgoing CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine3.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                    // Light-up panel blue for incoming call
                                    GridLine3.Background = Brushes.LightBlue;

                                    // Show time on panel
                                    ///   lbLine3Time.Invoke((MethodInvoker)(() => lbLine3Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///    lbLine3Number.Invoke((MethodInvoker)(() => lbLine3Number.Text = MyNumber));
                                    ///   lbLine3Name.Invoke((MethodInvoker)(() => lbLine3Name.Text = MyName));
                                    ///   
                                    lbLine3Time.Text = MyTime;
                                    lbLine3Number.Text = MyNumber;
                                    lbLine3Name.Text = MyName;

                                }));

                                break;
                            case "OE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone hangup--------------------
                                    //-------------------------------------------------------

                                    // Change panel color
                                    GridLine3.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine3.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));


                                }));
                                break;
                            default:
                                break;
                        }

                        break;

                    case "04":

                        switch (MyType + MyIndicator)
                        {
                            case "R":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------INCOMING CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine4.Source = new BitmapImage(new Uri(ApplicationPath + "phoneRing.png"));

                                    // Light-up panel green for incoming call
                                    GridLine4.Background = Brushes.LightGreen;

                                    // Show time on panel
                                    ///   lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///    lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///   lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///   
                                    lbLine4Time.Text = MyTime;
                                    lbLine4Number.Text = MyNumber;
                                    lbLine4Name.Text = MyName;

                                }));

                                break;
                            case "IS":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // ----------------------Ring answered------------------
                                    //------------------------------------------------------
                                    // Light-up panel green for incoming call
                                    GridLine4.Background = Brushes.LightGreen;

                                    // Show time on panel
                                    ///  lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///  lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///    lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///    
                                    lbLine4Time.Text = MyTime;
                                    lbLine4Number.Text = MyNumber;
                                    lbLine4Name.Text = MyName;

                                }));

                                break;
                            case "F":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone pickup-------------------
                                    //------------------------------------------------------
                                    picPhoneLine4.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                }));

                                break;
                            case "N":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone hangup--------------------
                                    //-------------------------------------------------------

                                    // Change panel color
                                    GridLine4.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine4.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));

                                break;
                            case "IE":

                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone hangup--------------------
                                    //-------------------------------------------------------

                                    // Change panel color
                                    GridLine4.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine4.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));
                                break;
                            case "OS":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Outgoing CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine4.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                    // Light-up panel blue for incoming call
                                    GridLine4.Background = Brushes.LightBlue;

                                    // Show time on panel
                                    ///      lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///     lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///    lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///    
                                    lbLine4Time.Text = MyTime;
                                    lbLine4Number.Text = MyNumber;
                                    lbLine4Name.Text = MyName;

                                }));

                                break;
                            case "OE":

                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone hangup--------------------
                                    //-------------------------------------------------------

                                    // Change panel color
                                    GridLine4.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine4.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));
                                break;
                            default:
                                break;
                        }

                        break;


                    case "05":

                        switch (MyType + MyIndicator)
                        {
                            case "R":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------INCOMING CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine5.Source = new BitmapImage(new Uri(ApplicationPath + "phoneRing.png"));

                                    // Light-up panel green for incoming call
                                    GridLine5.Background = Brushes.LightGreen;

                                    // Show time on panel
                                    ///   lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///    lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///   lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///   
                                    lbLine5Time.Text = MyTime;
                                    lbLine5Number.Text = MyNumber;
                                    lbLine5Name.Text = MyName;

                                }));

                                break;
                            case "IS":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // ----------------------Ring answered------------------
                                    //------------------------------------------------------
                                    // Light-up panel green for incoming call
                                    GridLine5.Background = Brushes.LightGreen;

                                    // Show time on panel
                                    ///  lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///  lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///    lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///    
                                    lbLine5Time.Text = MyTime;
                                    lbLine5Number.Text = MyNumber;
                                    lbLine5Name.Text = MyName;

                                }));

                                break;
                            case "F":

                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {

                                    //-----------------------Phone pickup-------------------
                                    //------------------------------------------------------
                                    picPhoneLine5.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));


                                }));

                                break;
                            case "N":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone hangup--------------------
                                    //-------------------------------------------------------

                                    // Change panel color
                                    GridLine5.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine5.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));

                                break;
                            case "IE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                            {
                                //-----------------------Phone hangup--------------------
                                //-------------------------------------------------------

                                // Change panel color
                                GridLine5.Background = Brushes.Silver;

                                // Change picture of phone to not-ringing
                                picPhoneLine5.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                            }));

                                break;
                            case "OS":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Outgoing CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine5.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                    // Light-up panel blue for incoming call
                                    GridLine5.Background = Brushes.LightBlue;

                                    // Show time on panel
                                    ///      lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///     lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///    lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///    
                                    lbLine5Time.Text = MyTime;
                                    lbLine5Number.Text = MyNumber;
                                    lbLine5Name.Text = MyName;

                                }));

                                break;
                            case "OE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                             {
                                 //-----------------------Phone hangup--------------------
                                 //-------------------------------------------------------

                                 // Change panel color
                                 GridLine5.Background = Brushes.Silver;

                                 // Change picture of phone to not-ringing
                                 picPhoneLine5.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                             }));

                                break;
                            default:
                                break;
                        }

                        break;


                    case "06":

                        switch (MyType + MyIndicator)
                        {
                            case "R":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------INCOMING CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine6.Source = new BitmapImage(new Uri(ApplicationPath + "phoneRing.png"));

                                    // Light-up panel green for incoming call
                                    GridLine6.Background = Brushes.LightGreen;

                                    // Show time on panel
                                    ///   lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///    lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///   lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///   
                                    lbLine6Time.Text = MyTime;
                                    lbLine6Number.Text = MyNumber;
                                    lbLine6Name.Text = MyName;


                                }));

                                break;
                            case "IS":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // ----------------------Ring answered------------------
                                    //------------------------------------------------------
                                    // Light-up panel green for incoming call
                                    GridLine6.Background = Brushes.LightGreen;

                                    // Show time on panel
                                    ///  lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///  lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///    lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///    
                                    lbLine6Time.Text = MyTime;
                                    lbLine6Number.Text = MyNumber;
                                    lbLine6Name.Text = MyName;

                                }));

                                break;
                            case "F":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone pickup-------------------
                                    //------------------------------------------------------
                                    picPhoneLine6.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                }));

                                break;
                            case "N":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone hangup--------------------
                                    //-------------------------------------------------------

                                    // Change panel color
                                    GridLine6.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine6.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));

                                break;
                            case "IE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                  {
                                      //-----------------------Phone hangup--------------------
                                      //-------------------------------------------------------

                                      // Change panel color
                                      GridLine6.Background = Brushes.Silver;

                                      // Change picture of phone to not-ringing
                                      picPhoneLine6.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                  }));

                                break;
                            case "OS":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Outgoing CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine6.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                    // Light-up panel blue for incoming call
                                    GridLine6.Background = Brushes.LightBlue;

                                    // Show time on panel
                                    ///      lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///     lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///    lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///    
                                    lbLine6Time.Text = MyTime;
                                    lbLine6Number.Text = MyNumber;
                                    lbLine6Name.Text = MyName;


                                }));

                                break;
                            case "OE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                               {
                                   //-----------------------Phone hangup--------------------
                                   //-------------------------------------------------------

                                   // Change panel color
                                   GridLine6.Background = Brushes.Silver;

                                   // Change picture of phone to not-ringing
                                   picPhoneLine6.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                               }));

                                break;
                            default:
                                break;
                        }

                        break;


                    case "07":

                        switch (MyType + MyIndicator)
                        {
                            case "R":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------INCOMING CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine7.Source = new BitmapImage(new Uri(ApplicationPath + "phoneRing.png"));

                                    // Light-up panel green for incoming call
                                    GridLine7.Background = Brushes.LightGreen;

                                    // Show time on panel
                                    ///   lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///    lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///   lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///   
                                    lbLine7Time.Text = MyTime;
                                    lbLine7Number.Text = MyNumber;
                                    lbLine7Name.Text = MyName;

                                }));

                                break;
                            case "IS":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // ----------------------Ring answered------------------
                                    //------------------------------------------------------
                                    // Light-up panel green for incoming call
                                    GridLine7.Background = Brushes.LightGreen;

                                    // Show time on panel
                                    ///  lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///  lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///    lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///    
                                    lbLine7Time.Text = MyTime;
                                    lbLine7Number.Text = MyNumber;
                                    lbLine7Name.Text = MyName;

                                }));

                                break;
                            case "F":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone pickup-------------------
                                    //------------------------------------------------------
                                    picPhoneLine7.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                }));

                                break;
                            case "N":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone hangup--------------------
                                    //-------------------------------------------------------

                                    // Change panel color
                                    GridLine7.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine7.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));

                                break;
                            case "IE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone hangup--------------------
                                    //-------------------------------------------------------

                                    // Change panel color
                                    GridLine7.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine7.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));

                                break;
                            case "OS":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Outgoing CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine7.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                    // Light-up panel blue for incoming call
                                    GridLine7.Background = Brushes.LightBlue;

                                    // Show time on panel
                                    ///      lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///     lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///    lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///    
                                    lbLine7Time.Text = MyTime;
                                    lbLine7Number.Text = MyNumber;
                                    lbLine7Name.Text = MyName;

                                }));

                                break;
                            case "OE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone hangup--------------------
                                    //-------------------------------------------------------

                                    // Change panel color
                                    GridLine7.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine7.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));

                                break;
                            default:
                                break;
                        }

                        break;


                    case "08":

                        switch (MyType + MyIndicator)
                        {
                            case "R":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------INCOMING CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine8.Source = new BitmapImage(new Uri(ApplicationPath + "phoneRing.png"));

                                    // Light-up panel green for incoming call
                                    GridLine8.Background = Brushes.LightGreen;

                                    // Show time on panel
                                    ///   lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///    lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///   lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///   
                                    lbLine8Time.Text = MyTime;
                                    lbLine8Number.Text = MyNumber;
                                    lbLine8Name.Text = MyName;

                                }));

                                break;
                            case "IS":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // ----------------------Ring answered------------------
                                    //------------------------------------------------------
                                    // Light-up panel green for incoming call
                                    GridLine8.Background = Brushes.LightGreen;

                                    // Show time on panel
                                    ///  lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///  lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///    lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///    
                                    lbLine8Time.Text = MyTime;
                                    lbLine8Number.Text = MyNumber;
                                    lbLine8Name.Text = MyName;

                                }));

                                break;
                            case "F":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone pickup-------------------
                                    //------------------------------------------------------
                                    picPhoneLine8.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                }));

                                break;
                            case "N":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone hangup--------------------
                                    //-------------------------------------------------------

                                    // Change panel color
                                    GridLine8.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine8.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));

                                break;
                            case "IE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone hangup--------------------
                                    //-------------------------------------------------------

                                    // Change panel color
                                    GridLine8.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine8.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));

                                break;
                            case "OS":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Outgoing CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine8.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                    // Light-up panel blue for incoming call
                                    GridLine8.Background = Brushes.LightBlue;

                                    // Show time on panel
                                    ///      lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///     lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///    lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///    
                                    lbLine8Time.Text = MyTime;
                                    lbLine8Number.Text = MyNumber;
                                    lbLine8Name.Text = MyName;

                                }));

                                break;
                            case "OE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone hangup--------------------
                                    //-------------------------------------------------------

                                    // Change panel color
                                    GridLine8.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine8.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));

                                break;
                            default:
                                break;
                        }

                        break;

                    case "09":

                        switch (MyType + MyIndicator)
                        {
                            case "R":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------INCOMING CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine9.Source = new BitmapImage(new Uri(ApplicationPath + "phoneRing.png"));

                                    // Light-up panel green for incoming call
                                    GridLine9.Background = Brushes.LightGreen;

                                    // Show time on panel
                                    ///   lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///    lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///   lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///   
                                    lbLine9Time.Text = MyTime;
                                    lbLine9Number.Text = MyNumber;
                                    lbLine9Name.Text = MyName;


                                }));

                                break;
                            case "IS":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // ----------------------Ring answered------------------
                                    //------------------------------------------------------
                                    // Light-up panel green for incoming call
                                    GridLine9.Background = Brushes.LightGreen;

                                    // Show time on panel
                                    ///  lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///  lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///    lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///    
                                    lbLine9Time.Text = MyTime;
                                    lbLine9Number.Text = MyNumber;
                                    lbLine9Name.Text = MyName;

                                }));

                                break;
                            case "F":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone pickup-------------------
                                    //------------------------------------------------------
                                    picPhoneLine9.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                }));

                                break;
                            case "N":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone hangup--------------------
                                    //-------------------------------------------------------

                                    // Change panel color
                                    GridLine9.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine9.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));

                                break;
                            case "IE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                              {
                                  //-----------------------Phone hangup--------------------
                                  //-------------------------------------------------------

                                  // Change panel color
                                  GridLine9.Background = Brushes.Silver;

                                  // Change picture of phone to not-ringing
                                  picPhoneLine9.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                              }));

                                break;
                            case "OS":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Outgoing CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine9.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                    // Light-up panel blue for incoming call
                                    GridLine9.Background = Brushes.LightBlue;

                                    // Show time on panel
                                    ///      lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///     lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///    lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///    
                                    lbLine9Time.Text = MyTime;
                                    lbLine9Number.Text = MyNumber;
                                    lbLine9Name.Text = MyName;

                                }));

                                break;
                            case "OE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                              {
                                  //-----------------------Phone hangup--------------------
                                  //-------------------------------------------------------

                                  // Change panel color
                                  GridLine9.Background = Brushes.Silver;

                                  // Change picture of phone to not-ringing
                                  picPhoneLine9.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                              }));

                                break;
                            default:
                                break;
                        }

                        break;

                    case "10":

                        switch (MyType + MyIndicator)
                        {
                            case "R":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------INCOMING CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine10.Source = new BitmapImage(new Uri(ApplicationPath + "phoneRing.png"));

                                    // Light-up panel green for incoming call
                                    GridLine10.Background = Brushes.LightGreen;

                                    // Show time on panel
                                    ///   lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///    lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///   lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///   
                                    lbLine10Time.Text = MyTime;
                                    lbLine10Number.Text = MyNumber;
                                    lbLine10Name.Text = MyName;

                                }));

                                break;
                            case "IS":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {

                                    // ----------------------Ring answered------------------
                                    //------------------------------------------------------
                                    // Light-up panel green for incoming call
                                    GridLine10.Background = Brushes.LightGreen;

                                    // Show time on panel
                                    ///  lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///  lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///    lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///    
                                    lbLine10Time.Text = MyTime;
                                    lbLine10Number.Text = MyNumber;
                                    lbLine10Name.Text = MyName;

                                }));

                                break;
                            case "F":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone pickup-------------------
                                    //------------------------------------------------------
                                    picPhoneLine10.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                }));

                                break;
                            case "N":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone hangup--------------------
                                    //-------------------------------------------------------

                                    // Change panel color
                                    GridLine10.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine10.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));

                                break;
                            case "IE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone hangup--------------------
                                    //-------------------------------------------------------

                                    // Change panel color
                                    GridLine10.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine10.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));

                                break;
                            case "OS":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Outgoing CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine10.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                    // Light-up panel blue for incoming call
                                    GridLine10.Background = Brushes.LightBlue;

                                    // Show time on panel
                                    ///      lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///     lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///    lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///    
                                    lbLine10Time.Text = MyTime;
                                    lbLine10Number.Text = MyNumber;
                                    lbLine10Name.Text = MyName;


                                }));

                                break;
                            case "OE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                 {
                                     //-----------------------Phone hangup--------------------
                                     //-------------------------------------------------------

                                     // Change panel color
                                     GridLine10.Background = Brushes.Silver;

                                     // Change picture of phone to not-ringing
                                     picPhoneLine10.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                 }));

                                break;
                            default:
                                break;
                        }

                        break;

                    case "11":

                        switch (MyType + MyIndicator)
                        {
                            case "R":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------INCOMING CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine11.Source = new BitmapImage(new Uri(ApplicationPath + "phoneRing.png"));

                                    // Light-up panel green for incoming call
                                    GridLine11.Background = Brushes.LightGreen;

                                    // Show time on panel
                                    ///   lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///    lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///   lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///   
                                    lbLine11Time.Text = MyTime;
                                    lbLine11Number.Text = MyNumber;
                                    lbLine11Name.Text = MyName;

                                }));

                                break;
                            case "IS":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // ----------------------Ring answered------------------
                                    //------------------------------------------------------
                                    // Light-up panel green for incoming call
                                    GridLine11.Background = Brushes.LightGreen;

                                    // Show time on panel
                                    ///  lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///  lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///    lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///    
                                    lbLine11Time.Text = MyTime;
                                    lbLine11Number.Text = MyNumber;
                                    lbLine11Name.Text = MyName;

                                }));

                                break;
                            case "F":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone pickup-------------------
                                    //------------------------------------------------------
                                    picPhoneLine11.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                }));

                                break;
                            case "N":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone hangup--------------------
                                    //-------------------------------------------------------

                                    // Change panel color
                                    GridLine11.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine11.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));

                                break;
                            case "IE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                            {
                                //-----------------------Phone hangup--------------------
                                //-------------------------------------------------------

                                // Change panel color
                                GridLine11.Background = Brushes.Silver;

                                // Change picture of phone to not-ringing
                                picPhoneLine11.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                            }));

                                break;
                            case "OS":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Outgoing CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine11.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                    // Light-up panel blue for incoming call
                                    GridLine11.Background = Brushes.LightBlue;

                                    // Show time on panel
                                    ///      lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///     lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///    lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///    
                                    lbLine11Time.Text = MyTime;
                                    lbLine11Number.Text = MyNumber;
                                    lbLine11Name.Text = MyName;

                                }));

                                break;
                            case "OE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                            {
                                //-----------------------Phone hangup--------------------
                                //-------------------------------------------------------

                                // Change panel color
                                GridLine11.Background = Brushes.Silver;

                                // Change picture of phone to not-ringing
                                picPhoneLine11.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                            }));

                                break;
                            default:
                                break;
                        }

                        break;

                    case "12":

                        switch (MyType + MyIndicator)
                        {
                            case "R":




                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------INCOMING CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine12.Source = new BitmapImage(new Uri(ApplicationPath + "phoneRing.png"));

                                    // Light-up panel green for incoming call
                                    GridLine12.Background = Brushes.LightGreen;

                                    // Show time on panel
                                    ///   lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///    lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///   lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///   
                                    lbLine12Time.Text = MyTime;
                                    lbLine12Number.Text = MyNumber;
                                    lbLine12Name.Text = MyName;

                                }));
                                break;
                            case "IS":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // ----------------------Ring answered------------------
                                    //------------------------------------------------------
                                    // Light-up panel green for incoming call
                                    GridLine12.Background = Brushes.LightGreen;

                                    // Show time on panel
                                    ///  lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///  lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///    lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///    
                                    lbLine12Time.Text = MyTime;
                                    lbLine12Number.Text = MyNumber;
                                    lbLine12Name.Text = MyName;

                                }));

                                break;
                            case "F":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone pickup-------------------
                                    //------------------------------------------------------
                                    picPhoneLine12.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                }));

                                break;
                            case "N":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Phone hangup--------------------
                                    //-------------------------------------------------------

                                    // Change panel color
                                    GridLine12.Background = Brushes.Silver;

                                    // Change picture of phone to not-ringing
                                    picPhoneLine12.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                }));

                                break;
                            case "IE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                  {
                                      //-----------------------Phone hangup--------------------
                                      //-------------------------------------------------------

                                      // Change panel color
                                      GridLine12.Background = Brushes.Silver;

                                      // Change picture of phone to not-ringing
                                      picPhoneLine12.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                  }));

                                break;
                            case "OS":



                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    //-----------------------Outgoing CALL--------------------
                                    //--------------------------------------------------------
                                    // Change picture of phone to ringing
                                    picPhoneLine12.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOffHook.png"));

                                    // Light-up panel blue for incoming call
                                    GridLine12.Background = Brushes.LightBlue;

                                    // Show time on panel
                                    ///      lbLine4Time.Invoke((MethodInvoker)(() => lbLine4Time.Text = MyTime));

                                    // Show name and number of panel
                                    ///     lbLine4Number.Invoke((MethodInvoker)(() => lbLine4Number.Text = MyNumber));
                                    ///    lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = MyName));
                                    ///    
                                    lbLine12Time.Text = MyTime;
                                    lbLine12Number.Text = MyNumber;
                                    lbLine12Name.Text = MyName;

                                }));

                                break;
                            case "OE":
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                 {
                                     //-----------------------Phone hangup--------------------
                                     //-------------------------------------------------------

                                     // Change panel color
                                     GridLine12.Background = Brushes.Silver;

                                     // Change picture of phone to not-ringing
                                     picPhoneLine12.Source = new BitmapImage(new Uri(ApplicationPath + "phoneOnHook.png"));

                                 }));

                                break;
                            default:
                                break;
                        }

                        break;



                    default:
                        break;
                }
                // ---------------------------------------------
                //         START & END RECORD PROCESSING 
                // ---------------------------------------------
                // Start Records will be processed below for 3 reasons:
                //   1. To perform a customer lookup.
                //	 2. Change visuals on the Caller ID main screen.
                //   3. Add phone call records to the log file properly.
                //		- Data will be stored for Start Records such that  
                //		  corresponding End records will replace them in log file. 

                // Combine MyType and MyIndicator to create a 'command' variable that allows us to 
                // determine if the call is an Incoming or Outgoing Start record.
                string command = MyType + MyIndicator;

                // If 'command' is Start record
                if (command == "IS" || command == "OS")
                {
                    // Set values to be checked against database during end record
                    SMyLine = MyLine;
                    SMyTime = MyTime;
                    SMyDate = MyDate;
                    SMyNumber = MyNumber;

                    // Look for phone number in database
                    ///  this.Invoke((MethodInvoker)(() => searchContacts()));
                    ///  

                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    searchContacts();
                                }));



                    // If found in database
                    if (SearchContactFound == true)
                    {
                        // Number found: change icon for contacts to found
                        switch (MyLine)
                        {
                            case "01":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show that contact was found
                                    picDatabaseLine1.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound.png"));

                                    // Change tag to 'change' so when clicked we update contact
                                    // instead of creating new one
                                    picDatabaseLine1.Tag = "change";

                                    // Change name on display for current line
                                    ///   lbLine1Name.Invoke((MethodInvoker)(() => lbLine1Name.Text = SearchContact));
                                    ///   
                                    lbLine1Name.Text = SearchContact;
                                    lbLine1.Text = MyLine;

                                }));
                                break;

                            case "02":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show that contact was found
                                    picDatabaseLine2.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound.png"));

                                    // Change tag to 'change' so when clicked we update contact
                                    // instead of creating new one
                                    picDatabaseLine2.Tag = "change";

                                    // Change name on display for current line
                                    ///  lbLine2Name.Invoke((MethodInvoker)(() => lbLine2Name.Text = SearchContact));
                                    ///  
                                    lbLine2Name.Text = SearchContact;
                                    lbLine2.Text = MyLine;

                                }));
                                break;

                            case "03":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show that contact was found
                                    picDatabaseLine3.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound.png"));

                                    // Change tag to 'change' so when clicked we update contact
                                    // instead of creating new one
                                    picDatabaseLine3.Tag = "change";

                                    // Change name on display for current line
                                    ///   lbLine3Name.Invoke((MethodInvoker)(() => lbLine3Name.Text = SearchContact));
                                    ///   
                                    lbLine3Name.Text = SearchContact;
                                    lbLine3.Text = MyLine;

                                }));
                                break;

                            case "04":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show that contact was found
                                    picDatabaseLine4.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound.png"));

                                    // Change tag to 'change' so when clicked we update contact
                                    // instead of creating new one
                                    picDatabaseLine4.Tag = "change";

                                    // Change name on display for current line
                                    ///   lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = SearchContact));
                                    ///   
                                    lbLine4Name.Text = SearchContact;
                                    lbLine4.Text = MyLine;

                                }));
                                break;

                            case "05":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show that contact was found
                                    picDatabaseLine5.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound.png"));

                                    // Change tag to 'change' so when clicked we update contact
                                    // instead of creating new one
                                    picDatabaseLine5.Tag = "change";

                                    // Change name on display for current line
                                    ///   lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = SearchContact));
                                    ///   
                                    lbLine5Name.Text = SearchContact;
                                    lbLine5.Text = MyLine;

                                }));
                                break;

                            case "06":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show that contact was found
                                    picDatabaseLine6.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound.png"));

                                    // Change tag to 'change' so when clicked we update contact
                                    // instead of creating new one
                                    picDatabaseLine6.Tag = "change";

                                    // Change name on display for current line
                                    ///   lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = SearchContact));
                                    ///   
                                    lbLine6Name.Text = SearchContact;
                                    lbLine6.Text = MyLine;

                                }));
                                break;

                            case "07":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show that contact was found
                                    picDatabaseLine7.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound.png"));

                                    // Change tag to 'change' so when clicked we update contact
                                    // instead of creating new one
                                    picDatabaseLine7.Tag = "change";

                                    // Change name on display for current line
                                    ///   lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = SearchContact));
                                    ///   
                                    lbLine7Name.Text = SearchContact;
                                    lbLine7.Text = MyLine;

                                }));
                                break;

                            case "08":

                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show that contact was found
                                    picDatabaseLine8.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound.png"));

                                    // Change tag to 'change' so when clicked we update contact
                                    // instead of creating new one
                                    picDatabaseLine8.Tag = "change";

                                    // Change name on display for current line
                                    ///   lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = SearchContact));
                                    ///   
                                    lbLine8Name.Text = SearchContact;
                                    lbLine8.Text = MyLine;


                                }));
                                break;

                            case "09":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show that contact was found
                                    picDatabaseLine9.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound.png"));

                                    // Change tag to 'change' so when clicked we update contact
                                    // instead of creating new one
                                    picDatabaseLine9.Tag = "change";

                                    // Change name on display for current line
                                    ///   lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = SearchContact));
                                    ///   
                                    lbLine9Name.Text = SearchContact;
                                    lbLine9.Text = MyLine;

                                }));
                                break;

                            case "10":

                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show that contact was found
                                    picDatabaseLine10.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound.png"));

                                    // Change tag to 'change' so when clicked we update contact
                                    // instead of creating new one
                                    picDatabaseLine10.Tag = "change";

                                    // Change name on display for current line
                                    ///   lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = SearchContact));
                                    ///   
                                    lbLine10Name.Text = SearchContact;
                                    lbLine10.Text = MyLine;

                                }));

                                break;

                            case "11":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show that contact was found
                                    picDatabaseLine11.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound.png"));

                                    // Change tag to 'change' so when clicked we update contact
                                    // instead of creating new one
                                    picDatabaseLine11.Tag = "change";

                                    // Change name on display for current line
                                    ///   lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = SearchContact));
                                    ///   
                                    lbLine11Name.Text = SearchContact;
                                    lbLine11.Text = MyLine;

                                }));
                                break;

                            case "12":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show that contact was found
                                    picDatabaseLine12.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound.png"));

                                    // Change tag to 'change' so when clicked we update contact
                                    // instead of creating new one
                                    picDatabaseLine12.Tag = "change";

                                    // Change name on display for current line
                                    ///   lbLine4Name.Invoke((MethodInvoker)(() => lbLine4Name.Text = SearchContact));
                                    ///   
                                    lbLine12Name.Text = SearchContact;
                                    lbLine12.Text = MyLine;

                                }));
                                break;

                        }

                        // Change Name to name found in database
                        MyName = SearchContact;
                    }
                    else
                    {
                        // Not found: change icon for contacts to insert
                        switch (MyLine)
                        {
                            case "01":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show contact not found
                                    picDatabaseLine1.Source = new BitmapImage(new Uri(ApplicationPath + "databaseInsert.png"));

                                    // Change tag to 'insert' so program knows to add new contact
                                    // if button is clicked.
                                    picDatabaseLine1.Tag = "insert";
                                    lbLine1.Text = MyLine;

                                }));


                                break;

                            case "02":
                                // Change image to show contact not found
                                //picDatabaseLine2.Source = new BitmapImage(new Uri(ApplicationPath + "databaseInsert.png"));

                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    picDatabaseLine2.Source = new BitmapImage(new Uri(ApplicationPath + "databaseInsert.png"));
                                    picDatabaseLine2.Tag = "insert";
                                    lbLine2.Text = MyLine;

                                }));


                                // Change tag to 'insert' so program knows to add new contact
                                // if button is clicked.
                                //picDatabaseLine2.Tag = "insert";
                                break;

                            case "03":

                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show contact not found
                                    picDatabaseLine3.Source = new BitmapImage(new Uri(ApplicationPath + "databaseInsert.png"));

                                    // Change tag to 'insert' so program knows to add new contact
                                    // if button is clicked.
                                    picDatabaseLine3.Tag = "insert";
                                    lbLine3.Text = MyLine;


                                }));
                                break;

                            case "04":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show contact not found
                                    picDatabaseLine4.Source = new BitmapImage(new Uri(ApplicationPath + "databaseInsert.png"));

                                    // Change tag to 'insert' so program knows to add new contact
                                    // if button is clicked.
                                    picDatabaseLine4.Tag = "insert";
                                    lbLine4.Text = MyLine;

                                }));
                                break;

                            case "05":

                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show contact not found
                                    picDatabaseLine5.Source = new BitmapImage(new Uri(ApplicationPath + "databaseInsert.png"));

                                    // Change tag to 'insert' so program knows to add new contact
                                    // if button is clicked.
                                    picDatabaseLine5.Tag = "insert";
                                    lbLine5.Text = MyLine;

                                }));
                                break;

                            case "06":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show contact not found
                                    picDatabaseLine6.Source = new BitmapImage(new Uri(ApplicationPath + "databaseInsert.png"));

                                    // Change tag to 'insert' so program knows to add new contact
                                    // if button is clicked.
                                    picDatabaseLine6.Tag = "insert";
                                    lbLine6.Text = MyLine;

                                }));
                                break;

                            case "07":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show contact not found
                                    picDatabaseLine7.Source = new BitmapImage(new Uri(ApplicationPath + "databaseInsert.png"));

                                    // Change tag to 'insert' so program knows to add new contact
                                    // if button is clicked.
                                    picDatabaseLine7.Tag = "insert";
                                    lbLine7.Text = MyLine;

                                }));
                                break;

                            case "08":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show contact not found
                                    picDatabaseLine8.Source = new BitmapImage(new Uri(ApplicationPath + "databaseInsert.png"));

                                    // Change tag to 'insert' so program knows to add new contact
                                    // if button is clicked.
                                    picDatabaseLine8.Tag = "insert";
                                    lbLine8.Text = MyLine;

                                }));
                                break;

                            case "09":

                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show contact not found
                                    picDatabaseLine9.Source = new BitmapImage(new Uri(ApplicationPath + "databaseInsert.png"));

                                    // Change tag to 'insert' so program knows to add new contact
                                    // if button is clicked.
                                    picDatabaseLine9.Tag = "insert";
                                    lbLine9.Text = MyLine;


                                }));
                                break;

                            case "10":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show contact not found
                                    picDatabaseLine10.Source = new BitmapImage(new Uri(ApplicationPath + "databaseInsert.png"));

                                    // Change tag to 'insert' so program knows to add new contact
                                    // if button is clicked.
                                    picDatabaseLine10.Tag = "insert";
                                    lbLine10.Text = MyLine;

                                }));
                                break;

                            case "11":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show contact not found
                                    picDatabaseLine11.Source = new BitmapImage(new Uri(ApplicationPath + "databaseInsert.png"));

                                    // Change tag to 'insert' so program knows to add new contact
                                    // if button is clicked.
                                    picDatabaseLine11.Tag = "insert";
                                    lbLine11.Text = MyLine;

                                }));
                                break;

                            case "12":


                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                {
                                    // Change image to show contact not found
                                    picDatabaseLine12.Source = new BitmapImage(new Uri(ApplicationPath + "databaseInsert.png"));

                                    // Change tag to 'insert' so program knows to add new contact
                                    // if button is clicked.
                                    picDatabaseLine12.Tag = "insert";
                                    lbLine12.Text = MyLine;

                                }));
                                break;

                        }
                    }

                    // Log start record into database
                    ///  this.Invoke((MethodInvoker)(() => logCall()));
                    ///  

                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                    {

                        logCall();
                    }));

                }

                    // ----------END RECORD PROCESSING----------

                    // if incoming or outgoing end record
                else if (command == "IE" || command == "OE")
                {

                    // Look for phone number in database
                    ///  this.Invoke((MethodInvoker)(() => searchContacts()));
                    ///  
                    searchContacts();

                    // If found in database
                    if (SearchContactFound == true)
                    {
                        // Take database name and use it for updating
                        MyName = SearchContact;
                    }

                    // On deluxe unit the End Record replaces the
                    // Start record to provide database with more
                    // accurate information (duration,rings,name,etc.)

                    // Updates record to new duration count for deluxe units
                    /// this.Invoke((MethodInvoker)(() => updateRecord()));
                    /// 

                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                    {
                        //Your Code Goes Here
                        updateRecord();
                    }));


                }
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }

        }
        #endregion

        #region Set Variables
        public void setVars()
        {
            try
            {
                // Clear all variables
                SearchContactFound = false;
                SearchContact = "";
                MyLine = "";
                MyType = "";
                MyDate = "";
                MyTime = "";
                MyCheckSum = "";
                MyRings = "";
                MyDuration = "";
                MyIndicator = "";
                MyNumber = "";
                MyName = "";
                FoundIndex = -1;

                // Get UDP received message from event
                string receivedMessage = UdpReceiverClass.ReceivedMessage;

                CallRecord currentRecord = new CallRecord(receivedMessage);
                MyLine = currentRecord.Line.ToString("00");
                MyType = string.IsNullOrEmpty(currentRecord.IOType) ? currentRecord.DetailType : currentRecord.IOType;
                string st = currentRecord.CallTime.ToShortTimeString();
                MyDate = currentRecord.CallTime.Month.ToString("00") + "/" + currentRecord.CallTime.Day.ToString("00");
                MyTime = st.Split()[0] + st.Split()[1]; //currentRecord.CallTime.Hour + ":" + currentRecord.CallTime.Minute;
                MyCheckSum = currentRecord.Checksum;
                MyRings = currentRecord.Ring;
                MyDuration = currentRecord.Duration.ToString("0000");
                MyIndicator = currentRecord.SEType;
                MyNumber = currentRecord.Phone;
                MyName = currentRecord.Name;
                string callerIDName = currentRecord.Name;


                //// Remove header
                //string rawData = receivedMessage.Substring(21, receivedMessage.Length - 21);
                //int index = rawData.IndexOf(" ");

                ////---- EXTRACTING INDIVIDUAL FIELDS FROM CALL RECORDS -------                   

                //// Deluxe units are capable of sending both Start and End Records on Incoming and Outgoing calls
                //// Deluxe units can also send detail records such as Ring, On-Hook and Off Hook.
                //// This section extracts data from fields and places it into varibles
                //// The code allows for all types of call records that can be sent.  			
                //// 
                //// Note: Basic unit units only send Start Records on Incoming Calls.


                //// Get Line number from string
                //MyLine = rawData.Substring(0, index);

                //// Update Raw data
                //rawData = rawData.Substring(index, rawData.Length - index);
                //index = rawData.IndexOf(" ");

                //while (rawData.IndexOf(" ") == 0)
                //{
                //    rawData = rawData.Substring(1, rawData.Length - 1);
                //}

                //// Get type from string.  
                //// For Deluxe units, the Data type can be either [I]nbound, [O]utbound, [R]ing, o[N]-hook, of[F]-hook
                //// For Basic units, the only Data type will be "I".

                //index = rawData.IndexOf(" ");
                //MyType = rawData.Substring(0, index);

                //// Update Raw data
                //rawData = rawData.Substring(index, rawData.Length - index);
                //index = rawData.IndexOf(" ");

                //while (rawData.IndexOf(" ") == 0)
                //{
                //    rawData = rawData.Substring(1, rawData.Length - 1);
                //}

                //// Check whether the record is a Incoming/Outgoing.  If not, it is a detail record
                //// which needs to be processed diffrently below.
                //if (MyType == "I" || MyType == "O")
                //{

                //    // Get start or end indicator
                //    index = rawData.IndexOf(" ");
                //    MyIndicator = rawData.Substring(0, index);

                //    // Update Raw data
                //    rawData = rawData.Substring(index, rawData.Length - index);
                //    index = rawData.IndexOf(" ");

                //    while (rawData.IndexOf(" ") == 0)
                //    {
                //        rawData = rawData.Substring(1, rawData.Length - 1);
                //    }

                //    // Get duration from string
                //    index = rawData.IndexOf(" ");
                //    MyDuration = rawData.Substring(0, index);

                //    // Update Raw data
                //    rawData = rawData.Substring(index, rawData.Length - index);
                //    index = rawData.IndexOf(" ");

                //    while (rawData.IndexOf(" ") == 0)
                //    {
                //        rawData = rawData.Substring(1, rawData.Length - 1);
                //    }

                //    // Get checksum from string
                //    index = rawData.IndexOf(" ");
                //    MyCheckSum = rawData.Substring(0, index);

                //    // Update Raw data
                //    rawData = rawData.Substring(index, rawData.Length - index);
                //    index = rawData.IndexOf(" ");

                //    while (rawData.IndexOf(" ") == 0)
                //    {
                //        rawData = rawData.Substring(1, rawData.Length - 1);
                //    }

                //    // Get rings from string
                //    index = rawData.IndexOf(" ");
                //    MyRings = rawData.Substring(0, index);

                //    // Update Raw data
                //    rawData = rawData.Substring(index, rawData.Length - index);
                //    index = rawData.IndexOf(" ");

                //    while (rawData.IndexOf(" ") == 0)
                //    {
                //        rawData = rawData.Substring(1, rawData.Length - 1);
                //    }

                //    // Get date from string
                //    index = rawData.IndexOf(" ");
                //    MyDate = rawData.Substring(0, index);

                //    // Update Raw data
                //    rawData = rawData.Substring(index, rawData.Length - index);
                //    index = rawData.IndexOf(" ");

                //    while (rawData.IndexOf(" ") == 0)
                //    {
                //        rawData = rawData.Substring(1, rawData.Length - 1);
                //    }

                //    // Get time from string
                //    index = rawData.IndexOf(" ");
                //    MyTime = rawData.Substring(0, index);

                //    // Update Raw data
                //    rawData = rawData.Substring(index, rawData.Length - index);
                //    index = rawData.IndexOf(" ");

                //    while (rawData.IndexOf(" ") == 0)
                //    {
                //        rawData = rawData.Substring(1, rawData.Length - 1);
                //    }

                //    // Get am/pm from string
                //    index = rawData.IndexOf(" ");
                //    MyTime += rawData.Substring(0, index);

                //    // Update Raw data
                //    rawData = rawData.Substring(index, rawData.Length - index);
                //    index = rawData.IndexOf(" ");

                //    while (rawData.IndexOf(" ") == 0)
                //    {
                //        rawData = rawData.Substring(1, rawData.Length - 1);
                //    }

                //    // Get number from string
                //    index = rawData.IndexOf(" ");
                //    MyNumber = rawData.Substring(0, index);

                //    // Update Raw data
                //    rawData = rawData.Substring(index, rawData.Length - index);
                //    index = rawData.IndexOf(" ");

                //    while (rawData.IndexOf(" ") == 0)
                //    {
                //        rawData = rawData.Substring(1, rawData.Length - 1);
                //    }

                //    // Get name from string
                //    MyName = rawData;

                //}
                ////----------------EXTRACT DATA FROM ON-HOOK, OFF-HOOK, AND RING DATA TYPES-----------
                //else if (MyType == "N" || MyType == "F" || MyType == "R")
                //{
                //    // Get date from string
                //    index = rawData.IndexOf(" ");
                //    MyDate = rawData.Substring(0, index);

                //    // Update Raw data
                //    rawData = rawData.Substring(index, rawData.Length - index);
                //    index = rawData.IndexOf(" ");

                //    while (rawData.IndexOf(" ") == 0)
                //    {
                //        rawData = rawData.Substring(1, rawData.Length - 1);
                //    }

                //    // Get time from string (in case we need the time from these real-time events)
                //    if ((rawData.IndexOf(" ")) == -1)
                //    {
                //        MyTime = rawData;
                //    }
                //    else
                //    {
                //        MyTime = rawData.Substring(0, index);
                //    }

                //}
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }
        #endregion

        #region LogCall
        public void logCall()
        {

            try
            {
                _objEntities = new PCApp_TaxitechEntities();
                int status = 0;

                url_sends objCallModel = new url_sends();
                objCallModel.SE = MyIndicator;
                objCallModel.Line = MyLine;
                objCallModel.RingType = MyType;
                objCallModel.Duration = MyDuration;
                //objCallModel.c = MyCheckSum;
                //objCallModel.Rings = MyRings;
                //objCallModel.Date = MyDate;
                objCallModel.Time = MyTime;
                objCallModel.Phone = MyNumber;
                objCallModel.Name = MyName;
                objCallModel.StartId = 0;
                objCallModel.Date = String.Format("{0:MM/dd}", DateTime.Now);
                _objEntities.url_sends.Add(objCallModel);
                status = _objEntities.SaveChanges();

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }

            // Update logfile form
            /// frmLog.refreshDGV();

        }
        #endregion

        #region Search Contact
        private void searchContacts()
        {
            try
            {
                _objEntities = new PCApp_TaxitechEntities();
                List<contact> listObjModel = new List<contact>();
                listObjModel = _objEntities.contacts.Where(x => x.Phone == MyNumber).ToList();

                // If phone found return the name matching the number
                if (listObjModel != null && listObjModel.Count > 0)
                {
                    SearchContactFound = true;
                    foreach (var item in listObjModel)
                    {
                        SearchContact = item.Name;
                    }

                }
                else
                {
                    SearchContactFound = false;
                }



            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }

        }
        #endregion

        #region Update Record
        public void updateRecord()
        {
            try
            {
                // Create database connection
                _objEntities = new PCApp_TaxitechEntities();

                ///Code is commented By Vidya Vikas mishra
                ///Purpose:- Now we don't update same call will do a new entry regarding same
                //int id = _objEntities.Function_USP_UpdateCalls(MyDuration, MyRings, MyIndicator, MyName, SMyLine, SMyTime, SMyDate, SMyNumber);
                //if (id >= 1)
                //{
                //}
                //else
                //{
                //}

                _objEntities = new PCApp_TaxitechEntities();
                int status = 0;

                url_sends objCallModel = new url_sends();
                objCallModel.SE = MyIndicator;
                objCallModel.Line = MyLine;
                objCallModel.RingType = MyType;
                objCallModel.Duration = MyDuration;
                //objCallModel.c = MyCheckSum;
                //objCallModel.Rings = MyRings;
                //objCallModel.Date = MyDate;
                objCallModel.Time = MyTime;
                objCallModel.Phone = MyNumber;
                objCallModel.Name = MyName;
                objCallModel.StartId = 1;
                objCallModel.Date = String.Format("{0:MM/dd}", DateTime.Now);
                _objEntities.url_sends.Add(objCallModel);
                status = _objEntities.SaveChanges();
                UpdateRecordInDatabase(MyNumber, MyLine);


            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }

            // Update logfile form
            ///  frmLog.refreshDGV();

        }
        #endregion

        public void UpdateRecordInDatabase(string phoneNumber, string line)
        {
            _objEntities = new PCApp_TaxitechEntities();
            var UpdateRecord = _objEntities.url_sends.Where(x => x.Phone == phoneNumber && x.StartId == 0 && x.Line == line).FirstOrDefault();
            if (UpdateRecord != null)
                UpdateRecord.StartId = 1;
            try
            {
                _objEntities.SaveChanges();
            }
            catch (Exception ex)
            {
            }
        }



        #region Window Load and Window Close Events
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                // Check whether two or more instances of the application are running
                // and close out of opening app if already running
                if (AlreadyRunning() == true)
                {
                    const string messageString = "Application already running...";
                    System.Windows.MessageBox.Show(messageString);
                    System.Windows.Forms.Application.Exit();
                }

                // Check if ELPopup is running and suspend if so.  
                // (ELPopup is a CallerID.com free software app that users sometimes inadvertantly load)
                Boolean elpopupFound = false;
                // Get all processes with name of 'elpopup'
                Process[] procList = Process.GetProcessesByName("elpopup");
                if (procList.Length > 0)
                {
                    string strProcName = procList[0].ProcessName;
                    int iProcID = procList[0].Id;

                    if (File.Exists(procList[0].MainModule.FileName))
                    {
                        elpopupFound = true;
                        ProcessStartInfo processProperties = new ProcessStartInfo();
                        processProperties.FileName = procList[0].MainModule.FileName;
                        processProperties.Arguments = "-pause";
                        Process myProcess = Process.Start(processProperties);
                    }

                }

                // IF ELPopup is found give program 2 seconds to attempt to pause ELPopup then show message
                if (elpopupFound == true)
                {
                    WaitFor(2000);
                    const string messageToShow =
                        "ELPopup found and will be suspended if possible. It is important that these two programs do not" +
                        " run at the same time. Failure to do this can cause this program to crash. Make sure ELPopup is closed.";
                    System.Windows.MessageBox.Show(messageToShow);
                }

                // Start Receiver thread
                UdpReceiveThread.IsBackground = true;
                UdpReceiveThread.Start();
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {

            Process[] procList = Process.GetProcessesByName("elpopup");
            if (procList.Length > 0)
            {
                string strProcName = procList[0].ProcessName;
                int iProcID = procList[0].Id;

                if (File.Exists(procList[0].MainModule.FileName))
                {
                    ProcessStartInfo processProperties = new ProcessStartInfo();
                    processProperties.FileName = procList[0].MainModule.FileName;
                    processProperties.Arguments = "-resume";
                    Process myProcess = Process.Start(processProperties);
                }

            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // End Receiver thread
            if (UdpReceiveThread.IsAlive)
            {
                UdpReceiveThread.Abort();
            }
        }

        #endregion

        #region Does Already running this process
        private Boolean AlreadyRunning()
        {

            // Get current process
            Process myProc = Process.GetCurrentProcess();
            // Get current process name
            string myProcessName = myProc.ProcessName;
            // Get all process of current process name
            Process[] procs = Process.GetProcessesByName(myProcessName);

            // If only one process exist return true
            if (procs.Length == 1)
            {
                return false;
            }

            // Loop through all processes to find if process started first
            for (int i = 0; i < procs.Length; i++)
            {
                // If process started first, return true
                if (procs[i].StartTime < myProc.StartTime)
                {
                    return true;
                }
            }

            // If code gets to here then return false
            return false;

        }
        #endregion

        #region Database line clicks

        private void picDatabaseLine1_MouseUp(object sender, MouseButtonEventArgs e)
        {


        }

        #endregion

        #region Change Icon If Chnage by User
        // ------UPDATE ICON IF CALLER ID NAME HAS BEEN CHANGED BY USER---------
        public void changeIcons(string myLine)
        {
            // After adding into database, change icon to found icon
            switch (myLine)
            {
                case "01":
                    picDatabaseLine1.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound"));
                    picDatabaseLine1.Tag = "change";
                    lbLine1Name.Text = StoredName;
                    break;
                case "02":
                    picDatabaseLine2.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound"));
                    picDatabaseLine2.Tag = "change";
                    lbLine2Name.Text = StoredName;
                    break;
                case "03":
                    picDatabaseLine3.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound"));
                    picDatabaseLine3.Tag = "change";
                    lbLine3Name.Text = StoredName;
                    break;
                case "04":
                    picDatabaseLine4.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound"));
                    picDatabaseLine4.Tag = "change";
                    lbLine4Name.Text = StoredName;
                    break;
                case "05":
                    picDatabaseLine5.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound"));
                    picDatabaseLine5.Tag = "change";
                    lbLine5Name.Text = StoredName;
                    break;
                case "06":
                    picDatabaseLine6.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound"));
                    picDatabaseLine6.Tag = "change";
                    lbLine6Name.Text = StoredName;
                    break;
                case "07":
                    picDatabaseLine7.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound"));
                    picDatabaseLine7.Tag = "change";
                    lbLine7Name.Text = StoredName;
                    break;
                case "08":
                    picDatabaseLine8.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound"));
                    picDatabaseLine8.Tag = "change";
                    lbLine8Name.Text = StoredName;
                    break;
                case "09":
                    picDatabaseLine9.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound"));
                    picDatabaseLine9.Tag = "change";
                    lbLine9Name.Text = StoredName;
                    break;
                case "10":
                    picDatabaseLine10.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound"));
                    picDatabaseLine10.Tag = "change";
                    lbLine10Name.Text = StoredName;
                    break;
                case "11":
                    picDatabaseLine11.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound"));
                    picDatabaseLine11.Tag = "change";
                    lbLine11Name.Text = StoredName;
                    break;
                case "12":
                    picDatabaseLine12.Source = new BitmapImage(new Uri(ApplicationPath + "databaseFound"));
                    picDatabaseLine12.Tag = "change";
                    lbLine12Name.Text = StoredName;
                    break;

            }
        }
        #endregion

        #region Popup For Add Contacts

        private void ButtonAddContact_Click(object sender, RoutedEventArgs e)
        {
            PopupCallerId.IsOpen = true;
            AddContactsUserControl objUc = new AddContactsUserControl(PopupCallerId);
            GridPopupCallerId.Children.Clear();
            GridPopupCallerId.Children.Add(objUc);
        }


        private void ButtonPopupClose_Click(object sender, RoutedEventArgs e)
        {
            PopupCallerId.IsOpen = false;
        }
        #endregion

        #region Log in a file if any error comes
        public void LogException(string content)
        {
            //   string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            FileStream fs = new FileStream(@"C:\CallerIdLog.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sm = new StreamWriter(fs);
            sm.BaseStream.Seek(0, SeekOrigin.End);
            StringBuilder objBulider = new StringBuilder();
            objBulider.Append("/*****************************" + DateTime.UtcNow + "******************************/" + Environment.NewLine + content + Environment.NewLine);
            sm.WriteLine(objBulider);
            sm.Flush();
            sm.Close();
        }
        #endregion


    }
}
