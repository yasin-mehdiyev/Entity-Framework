using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _10_11_2018.Models;

namespace _10_11_2018
{
    public partial class Form1 : Form
    {
        private CaClassEntities db = new CaClassEntities();
        private Student selectedId;

        public Form1()
        {
            InitializeComponent();
            FillGetData();
            FillComboGroup();
        }


        private void FillGetData()
        {
            dgwStudents.Rows.Clear();
            List<Student> students = db.Students.ToList();
            foreach (Student stu in students)
            {
                dgwStudents.Rows.Add(stu.Id,
                                     stu.Name + " " + stu.Surname,
                                     stu.Phone,
                                     stu.Gender ? "Kişi" : "Qadın",
                                     stu.Group.Name + " (" + stu.Group.Teacher.FullName + ")",
                                     stu.Birthday.ToString("dd.MM.yyyy"));
            }
        }

        private void FillComboGroup()
        {
            List<Group> groups = db.Groups.OrderBy(g => g.Name).ToList();
            foreach (Group group in groups)
            {
                cmbGroups.Items.Add(group.Name);
            }
        }


        private void Reset()
        {
            FillGetData();
            TxtName.Text = "";
            TxtPhone.Text = "";
            TxtSurname.Text = "";
            rdbMan.Checked = true;
            dtpBirthday.Value = DateTime.Now;
            cmbGroups.Text = "";

            btnupdate.Visible = false;
            btndelete.Visible = false;
            BtnAdd.Visible = true;

        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            Student student = new Student
            {
                Name = TxtName.Text,
                Surname = TxtSurname.Text,
                Phone = TxtPhone.Text,
                Gender = Convert.ToBoolean(rdbMan.Checked ? 1 : 0),
                Birthday = dtpBirthday.Value,
                GroupId = db.Groups.FirstOrDefault(g => g.Name == cmbGroups.Text).Id,
            };


            db.Students.Add(student);
            db.SaveChanges();

            Reset();
    }

        private void dgwStudents_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            btnupdate.Visible = true;
            btndelete.Visible = true;
            BtnAdd.Visible = false;

            int id = Convert.ToInt32(dgwStudents.Rows[e.RowIndex].Cells[0].Value.ToString());

            this.selectedId = db.Students.Find(id);

            TxtName.Text = selectedId.Name;
            TxtSurname.Text = selectedId.Surname;
            TxtPhone.Text = selectedId.Phone;
            cmbGroups.Text = selectedId.Group.Name;
            dtpBirthday.Value = selectedId.Birthday;
            if (selectedId.Gender)
            {
                rdbMan.Checked = true;
                rdbWoman.Checked = false;
            }
            else
            {
                rdbMan.Checked = false;
                rdbWoman.Checked = true;
            }
        }


        private void btndelete_Click(object sender,EventArgs e)
        {
            DialogResult r = MessageBox.Show("Silmək istəyirsinizmi?", "Silmə işləmi", MessageBoxButtons.OKCancel);
            if (r == DialogResult.OK)
            {
                db.Students.Remove(this.selectedId);
                db.SaveChanges();
                Reset();
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            selectedId.Name = TxtName.Text;
            selectedId.Surname = TxtSurname.Text;
            selectedId.Phone = TxtPhone.Text;
            selectedId.Gender = Convert.ToBoolean(rdbMan.Checked ? 1 : 0);
            selectedId.Birthday = dtpBirthday.Value;
            selectedId.GroupId = db.Groups.FirstOrDefault(g => g.Name == cmbGroups.Text).Id;

            db.SaveChanges();
            Reset();
        }
    }
}
