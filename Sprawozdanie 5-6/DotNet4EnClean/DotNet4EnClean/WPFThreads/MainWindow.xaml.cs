using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WPFThreads
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            consumers = new List<Thread>(thnumber);
            buffer = new BlockingCollection<int>(capacity);
            consumerDelaySlider.ValueChanged += ConsumerDelaySlider_ValueChanged;
            producerDelaySlider.ValueChanged += ProducerDelaySlider_ValueChanged;
        }

        /// <summary>
        /// Set the producer delay on slider value change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProducerDelaySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            producerDelay = (int)e.NewValue;
        }

        /// <summary>
        /// Set the consumer delay on slider value change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConsumerDelaySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            consumerDelay = (int)e.NewValue;
        }


        private readonly int capacity = 10;
        private readonly int thnumber = 10;
        private List<Thread> consumers;
        private Thread producer;
        private BlockingCollection<int> buffer;
        private CancellationTokenSource cts;
        private int producerDelay = 500;
        private int consumerDelay = 5000;
        private bool running = false;

        /// <summary>
        /// Event handler on click button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            if (running)
            {
                running = false;
                b.Content = "Start";
                Stop();
            }
            else
            {
                running = true;
                b.Content = "Stop";
                Start();
            }
            
        }

        /// <summary>
        /// Initiate and start threads
        /// </summary>
        private void Start()
        {
            Trace.WriteLine("Start");
            cts = new CancellationTokenSource();
            producer = new Thread(Producer);
            producer.Start(cts.Token);
            
            for (int i = 0; i < thnumber; i++)
            {
                Thread t = new Thread(Consumer);
                t.Start(cts.Token);
                consumers.Add(t);
            }
        }

        /// <summary>
        /// Finish threads
        /// </summary>
        private void Stop()
        {
            cts.Cancel(); //signal cancellation
            consumers.ForEach(c => c.Join()); //wait for all consumers
            producer.Join(); //wait for producer
            consumers.Clear(); //clear array of consummers
            cts.Dispose();
        }

        /// <summary>
        /// Consummer thread
        /// </summary>
        /// <param name="o"></param>
        private void Consumer(object o)
        {
            CancellationToken ct = (CancellationToken)o;
            Random rand = new Random(Thread.CurrentThread.ManagedThreadId + (int)DateTime.Now.Ticks);
            int product = 0;

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    ct.WaitHandle.WaitOne(TimeSpan.FromMilliseconds(rand.Next(0, consumerDelay))); //it's like a Sleep with cancellation token
                    product = buffer.Take(ct);
                    //TODO: update collectionAmountLabel with buffer.Count in thread safe way
                    Trace.WriteLine($"Consumer get {product}"); //In WPF there no console, so we use Trace to put information on output
                    //TODO: append the same info "Consumer get..." to the consumerOutput
                }
                catch (OperationCanceledException ocex)
                {
                    Trace.WriteLine(ocex.Message);
                    break;
                }
            }
        }


        /// <summary>
        /// Producer thread
        /// </summary>
        /// <param name="o"></param>
        private void Producer(object o)
        {
            CancellationToken ct = (CancellationToken)o;
            Random rand = new Random(Thread.CurrentThread.ManagedThreadId + (int)DateTime.Now.Ticks);
            int product = 0;
            int number = 0;
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    
                    ct.WaitHandle.WaitOne(TimeSpan.FromMilliseconds(rand.Next(0, producerDelay))); //it's like a Sleep with cancellation token
                    product = number++;
                    buffer.Add(product, ct);
                    //TODO: update collectionAmountLabel with buffer.Count in thread safe way
                    Trace.WriteLine($"Producer put {product}");
                    //TODO: append the same info "Producer put..." to the producerOutput
                }
                catch (OperationCanceledException ocex)
                {
                    Trace.WriteLine(ocex.Message);
                    break;
                }
            }
        }
    }
}
