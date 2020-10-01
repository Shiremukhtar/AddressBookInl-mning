using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace AddressBookInlämning
{

    public partial class Form1 : Form
    {
        private Contact[] contactBook = new Contact[1];
        string path = (@"C:\cont\Kontakts.txt");
        public Form1()
        {
            InitializeComponent();
        }

        private void Write(Contact obj)
        {
            using(StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(contactBook.Length + 1);
                sw.WriteLine(obj.Name);
                sw.WriteLine(obj.Gatuadress);
                sw.WriteLine(obj.Postnummer);
                sw.WriteLine(obj.Postort);
                sw.WriteLine(obj.Phone);
                sw.WriteLine(obj.Email);

                for (int i = 0; i < contactBook.Length; i++)
                {
                    sw.WriteLine(contactBook[i].Name);
                    sw.WriteLine(contactBook[i].Gatuadress);
                    sw.WriteLine(contactBook[i].Postnummer);
                    sw.WriteLine(contactBook[i].Postort);
                    sw.WriteLine(contactBook[i].Phone);
                    sw.WriteLine(contactBook[i].Email);
                }

              
            }
        }

        private void SaveAll(Contact[] contacts)
        {
            File.WriteAllText(path, string.Empty);
            
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(0);
                sw.WriteLine(contacts.Length);
                for (int i = 0; i < contacts.Length; i++)
                {
                    if (contactBook[i] != null) { 
                        sw.WriteLine(contacts[i].Name);
                        sw.WriteLine(contacts[i].Gatuadress);
                        sw.WriteLine(contacts[i].Postnummer);
                        sw.WriteLine(contacts[i].Postort);
                        sw.WriteLine(contacts[i].Phone);
                        sw.WriteLine(contacts[i].Email);
                    }
                }

            }
        }

        private void Read()
        {             
            using(StreamReader sr = new StreamReader(path))
            {
                int count = -1;
                int.TryParse(sr.ReadLine(), out count);

                contactBook = new Contact[count];
                for (int i = 0; i < contactBook.Length; i++)
                {
                    contactBook[i] = new Contact();
                    contactBook[i].Name = sr.ReadLine();
                    contactBook[i].Gatuadress = sr.ReadLine();
                    contactBook[i].Postnummer = sr.ReadLine();
                    contactBook[i].Postort = sr.ReadLine();
                    contactBook[i].Phone = sr.ReadLine();
                    contactBook[i].Email = sr.ReadLine();
                }
            }
        }

        private void ShowContacts()
        {
            lstBoxContact.Items.Clear();
            for (int i = 0; i < contactBook.Length; i++)
            {
                lstBoxContact.Items.Add(contactBook[i].Name);
                lstBoxContact.Items.Add(contactBook[i].Gatuadress);
                lstBoxContact.Items.Add(contactBook[i].Postnummer);
                lstBoxContact.Items.Add(contactBook[i].Postort);
                lstBoxContact.Items.Add(contactBook[i].Phone);
                lstBoxContact.Items.Add(contactBook[i].Email);
            }
        }

        //Tarbort metoden
        private void Delete(string name)
        {
            var contacts = new List<Contact>(contactBook);

            var toBeDeleted = (from contact in contacts where contact.Name == name select contact).FirstOrDefault();

            if (toBeDeleted != null)
                contacts.Remove(toBeDeleted);

            contactBook = new Contact[contacts.Count];
            int i = 0;
            foreach (var contact in contacts)
            {
                contactBook[i++] = contact;
            }
        
            SaveAll(contactBook);


        }

        private void ClearForm()
        {
            txtName.Text = String.Empty;
            txtGatuadress.Text = String.Empty;
            txtPostnummer.Text = String.Empty;
            txtPostort.Text = String.Empty;
            txtPhone.Text = String.Empty;
            txtEmail.Text = String.Empty;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Read();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Contact obj = new Contact();
            obj.Name = txtName.Text;
            obj.Gatuadress = txtGatuadress.Text;
            obj.Postnummer = txtPostnummer.Text;
            obj.Postort = txtPostort.Text;
            obj.Phone = txtPhone.Text;
            obj.Email = txtEmail.Text;

            Write(obj);
            Read();
            //ShowContacts();
            ClearForm();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                lstBoxContact.Items.Clear();

                if (txtSearch.Text != "" && contactBook.Length != 0)
                {
                    lstBoxContact.Items.Clear();
                    string searchName = txtSearch.Text;
                    bool found = false;
                    //int counter = 0;

                    for (int counter = 0; counter < contactBook.Length; counter++)                    
                    {
                        if (String.Compare(contactBook[counter].Name.ToLower(), searchName.ToLower()) == 0 || String.Compare(contactBook[counter].Postort.ToLower(), searchName.ToLower() ) == 0)
                        {
                            found = true;
                            lstBoxContact.Items.Add(contactBook[counter].Name);
                        }
                       
                    }
                   
                }
                else
                {
                    MessageBox.Show("Kan inte utföra sökningen!");
                }

            }
            catch (Exception)
            {

                MessageBox.Show("Inte giltigt");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Contact concact = e;
            //Delete(contact);
            int index = lstBoxContact.SelectedIndex;

            if (index != -1)
            {
                //lstBoxContact.Items.RemoveAt(index);
                Delete(lstBoxContact.Items[0].ToString());
                txtSearch.Text = String.Empty;
                
            }
            ClearForm();
            lblDeleteContact.Text = "Contact Deleted!";
        }

        private void lstBoxContact_MouseDown(object sender, MouseEventArgs e)
        {
            string name = "";
            var index = lstBoxContact.SelectedIndex;
            if (index != -1)
            {
                name = lstBoxContact.Items[index].ToString();

                Contact contact = contactBook.Where(c => c.Name == name).FirstOrDefault();
                txtName.Text = contact.Name;
                txtGatuadress.Text = contact.Gatuadress;
                txtPostnummer.Text = contact.Postnummer;
                txtPostort.Text = contact.Postort;
                txtPhone.Text = contact.Phone;
                txtEmail.Text = contact.Email;
            }
            
        }

        private void lstBoxContact_MouseUp(object sender, MouseEventArgs e)
        {
          
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int index = lstBoxContact.SelectedIndex;

            if (index != -1)
            {
                //lstBoxContact.Items.RemoveAt(index);
                Delete(lstBoxContact.Items[0].ToString());
                txtSearch.Text = String.Empty;


                //lstBoxContact.Items.Insert(index, txtSearch.Text);
            }
            Contact obj = new Contact();
            obj.Name = txtName.Text;
            obj.Gatuadress = txtGatuadress.Text;
            obj.Postnummer = txtPostnummer.Text;
            obj.Postort = txtPostort.Text;
            obj.Phone = txtPhone.Text;
            obj.Email = txtEmail.Text;

            Write(obj);
            Read();
            //ShowContacts();
            ClearForm();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }



        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
                // these last two lines will stop the beep sound
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave.PerformClick();
                // these last two lines will stop the beep sound
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }
    }
}
