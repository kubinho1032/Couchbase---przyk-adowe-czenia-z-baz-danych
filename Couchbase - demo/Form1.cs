using Couchbase;
using Couchbase.Configuration.Client;
using Couchbase.Core;
using Couchbase.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Couchbase___demo
{

    public partial class Form1 : Form
    {
        bool blad_laczenia = false;
        string wczytane = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var config = new ClientConfiguration
            {
                Servers = new List<Uri>
  {
    new Uri("http://localhost:8091/pools"),
    new Uri("http://localhost:8091/")

  },
                UseSsl = true,
                DefaultOperationLifespan = 1000,
                BucketConfigs = new Dictionary<string, BucketConfiguration>
  {
    {"beer-sample", new BucketConfiguration
    {
      BucketName = "beer-sample",
      UseSsl = false,
      Password = "admin",
      DefaultOperationLifespan = 2000,
      PoolConfiguration = new PoolConfiguration
      {
        MaxSize = 10,
        MinSize = 5,
        SendTimeout = 12000
      }
    }}
  }
            };

            using (var cluster = new Cluster(config))
            {

                IBucket bucket = null;
                bucket = cluster.OpenBucket();

                const string query = "SELECT name from `beer-sample` as name";
                var result = bucket.Query<dynamic>(query);
                foreach (var row in result.Rows)
                {
                    wczytane = row.toString();
                }


                if (bucket != null)
                {
                    cluster.CloseBucket(bucket);
                }
            }
            
            label1.Text = wczytane;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var config = new ClientConfiguration
            {
                Servers = new List<Uri>
  {
    new Uri("http://172.16.219.65:8091"),

  }
            };

            var config2 = new ClientConfiguration
            {
                Servers = new List<Uri>
  {
   new Uri("http://172.16.179.155:8091"),


  }
            };


            Cluster Cluster = new Cluster(config);
            Cluster Cluster2 = new Cluster(config2);


            try {
                var cluster = Cluster.OpenBucket("beer-sample");
            


                string query = "SELECT style from `beer-sample` where name like '%" + textBox3.Text + "%' limit 1";
                var result = cluster.Query<Object>(query);
                foreach (var row in result.Rows)
                {

                    string value = row.ToString().Substring(15);
                    label8.Text = value.Substring(0,value.IndexOf('"'));
                    label8.BackColor = Color.Yellow;
                    label12.Text = config.Servers.ElementAt(0).ToString();
                    label12.BackColor = Color.Orange;
                }

                if (result.Rows.Count == 0)
                {
                    label8.Text = "Brak wyników";
                }

                if (cluster != null)
                {
                    Cluster.CloseBucket(cluster);
                }
            
            }
            catch(Exception ez)
            {
                blad_laczenia = true;
                if(ez.Message.Contains("Could not bootstrap - check inner exceptions for details."))
                {
                    MessageBox.Show("Błąd łączenia z serwerem 1");
                }
            }


            if (blad_laczenia == false) { 
            try { 
            using (var cluster2 = Cluster2.OpenBucket("beer-sample"))
            {


                string query = "SELECT category from `beer-sample` where name like '%" + textBox3.Text + "%' limit 1";
                var result2 = cluster2.Query<Object>(query);



             

                foreach (var row in result2.Rows)
                {

                    string value = row.ToString().Substring(18);

                    label10.Text = value.Substring(0, value.IndexOf('"'));
                    label10.BackColor = Color.Yellow;
                    label13.Text = config2.Servers.ElementAt(0).ToString();
                    label13.BackColor = Color.Orange;
                }

                if (result2.Rows.Count == 0)
                {
                    label10.Text = "Brak wyników";
                }

                if (cluster2 != null)
                {
                    Cluster2.CloseBucket(cluster2);
                }
            }
            }
            catch(Exception exx)
            {
                if (exx.Message.Contains("Could not bootstrap - check inner exceptions for details."))
                {
                    MessageBox.Show("Błąd łączenia z serwerem 2");
                }
            }
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
             Cluster Cluster = new Cluster();

            using (var bucket = Cluster.OpenBucket())
            {
                var document = new Document<dynamic>
                {
                    Id = textBox1.Text,
                    Content = new
                    {
                        name = textBox2.Text
                    }
                };
                var upsert = bucket.Upsert(document);
                if (upsert.Success)
                {
                    var get = bucket.GetDocument<dynamic>(document.Id);
                    document = get.Document;
                    var msg = string.Format("{0} {1}!", document.Id, document.Content.name);
                    label1.Text = "Dodano dokument: "+msg;
                }
                
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
  //          var config = new ClientConfiguration
  //          {
  //              Servers = new List<Uri>
  //{
  //  new Uri("http://172.16.219.65:8091"),

  //}
  //          };

  //          Cluster Cluster = new Cluster(config);

  //          using (var cluster = Cluster.OpenBucket("MobileDB"))
  //          {


  //              var query = new ViewQuery().From("1","View1");
  //              var result = cluster.Query<List<String>>(query);
  //              foreach (var row in result.Rows)
  //              {

  //                  if (row.Value[1] != null)
  //                  {
  //                      label12.Text = row.Value[1];
  //                  }
  //              }



  //              if (cluster != null)
  //              {
  //                  Cluster.CloseBucket(cluster);
  //              }
  //          }
        }
    }
}
