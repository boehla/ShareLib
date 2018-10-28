using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lib
{
    public partial class DebugForm : Form
    {
        LogEntries newTexts = new LogEntries();

        LogEntries oldTexts = new LogEntries();

        private string curItem = Logging.allkey;
        private bool showform = false;
        private object l = 1;
        private int maxChars = 100000;
        public DebugForm()
        {
            InitializeComponent();
        }

        public void showForm() {
            showform = true;
        }
        public void log(LogEntry loge) {
            lock (l) {
                newTexts.put(loge);
            }
        }

        private void addTextToBox(string text, Color col) {
            try {
                if (rtbDebug.Text.Length > maxChars) rtbDebug.Text = "";
                
                int start = rtbDebug.TextLength;

                bool autoscroll = false;
                if (rtbDebug.SelectionStart == start) {
                    autoscroll = true;
                }
                rtbDebug.AppendText(text);
                int end = rtbDebug.TextLength;
                //rtb.Select(start, end - start);
                //rtb.SelectionColor = col;
                //rtb.SelectionLength = 0;

                if (autoscroll) {
                    rtbDebug.SelectionStart = rtbDebug.Text.Length;
                    rtbDebug.ScrollToCaret();
                }
            } catch { }
        }

        private void DebugForm_FormClosing(object sender, FormClosingEventArgs e) {
            this.Hide();
            e.Cancel = true; // this cancels the close event.
        }

        private void DebugForm_Load(object sender, EventArgs e) {
        }

        private void timer_Tick(object sender, EventArgs e) {
            try {
                timer.Stop();
                while (true) {
                    if (!cbRun.Checked) break;
                    LogEntry loge = null;
                    lock (l) {
                        loge = newTexts.pull();
                    }
                    if (loge == null) break;
                    handleNewEntry(loge);
                    if (loge.Filename.Equals(Logging.allkey)) continue;  
                }
                if(showform) this.Show();
                showform = false;
                if(cbAutorefresh.Checked) dgvPerfomance.DataSource = Performance.getTable();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            } finally {
                timer.Start();
            }
        }
        private void handleNewEntry(LogEntry loge) {
            if (!oldTexts.Items.ContainsKey(loge.Filename)) {
                lvLogFiles.Items.Add(loge.Filename);
            }
            oldTexts.put(loge);
            int textlength =  oldTexts.Items[loge.Filename].Text.Length;
            if (textlength > maxChars) {
                oldTexts.Items[loge.Filename].Text.Remove(0, textlength - maxChars);
            }
            if(loge.Filename.Equals(curItem)){
                addTextToBox(loge.Text.ToString() + "\r\n", Color.Black);
            }
        }

        
        private void lvLogFiles_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
            try { 
                string newItem = e.Item.Text;
                if(newItem.Equals(curItem)) return;
                curItem = newItem;
                rtbDebug.Clear();
                addTextToBox(oldTexts.Items[curItem].Text.ToString() + "\r\n", Color.Black);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void bPerfRefresh_Click(object sender, EventArgs e) {
            try { 
                dgvPerfomance.DataSource = Performance.getTable();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void bPerfReset_Click(object sender, EventArgs e) {
            try { 
                Performance.resetWatches();
                dgvPerfomance.DataSource = Performance.getTable();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void bClear_Click(object sender, EventArgs e) {
            try { 
                rtbDebug.Clear();
                oldTexts.Items[curItem].Text.Clear();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void bClearAll_Click(object sender, EventArgs e) {
            try { 
                rtbDebug.Clear();
                oldTexts.ClearTexts();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                if (tabControl1.SelectedTab == tpPerformance) {
                    cbAutorefresh.Checked = true;
                }
            }catch(Exception ex) {
                Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            }
        }
    }
}
