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

        private void ProducerDelaySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            producerDelay = (int)e.NewValue;
        }

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

        private void Stop()
        {
            cts.Cancel();
            consumers.ForEach(c => c.Join());
            producer.Join();
            consumers.Clear();
            cts.Dispose();
        }

        private void Consumer(object o)
        {
            CancellationToken ct = (CancellationToken)o;
            Random rand = new Random(Thread.CurrentThread.ManagedThreadId + (int)DateTime.Now.Ticks);
            int product = 0;

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    ct.WaitHandle.WaitOne(TimeSpan.FromMilliseconds(rand.Next(0, consumerDelay)));
                    product = buffer.Take(ct);
                    Dispatcher.Invoke(() => {
                        collectionAmountLabel.Content = buffer.Count.ToString();
                        consumerOutput.AppendText("Consumer get " + product + "\r");
                        consumerOutput.ScrollToEnd();
                    });
                }
                catch (OperationCanceledException ocex)
                {
                    Trace.WriteLine(ocex.Message);
                    break;
                }
            }
        }


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
                    
                    ct.WaitHandle.WaitOne(TimeSpan.FromMilliseconds(rand.Next(0, producerDelay)));
                    product = number++;
                    buffer.Add(product, ct);
                    Dispatcher.Invoke(() => {
                        collectionAmountLabel.Content = buffer.Count.ToString();
                        producerOutput.AppendText("Producer put " + product + "\r");
                        producerOutput.ScrollToEnd();
                    });
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
