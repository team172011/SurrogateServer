using Surrogate.Model;
using Surrogate.Model.Module;
using Surrogate.Modules;
using Surrogate.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Surrogate.Implementations.Controller.Module
{
    public class InformationsModule : VisualModule<ModulePropertiesBase, ModuleInfo>
    {
        private UserControl _view;
        private readonly IDatabaseConnection _database;

        public InformationsModule(IDatabaseConnection database) : base(new ModulePropertiesBase("Informationen","Patientendaten einsehen und Materialien dokumentieren"))
        {
            _database = database;
            _view = new InformationsModuleView(this);
            
        }

        public override IModuleProperties Properties => GetProperties();

        public override UserControl GetPage()
        {
            return _view;
        }

        public override bool IsRunning()
        {
            throw new NotImplementedException();
        }

        public override void Start(ModuleInfo info)
        {
            throw new NotImplementedException();
        }

        internal ObservableCollection<Patient> GetPatientRows()
        {
            var reader = _database.ExecuteQuery("Select * from Patients");
            ObservableCollection<Patient> rows = new ObservableCollection<Patient>();
            if (reader.HasRows)
            {                
                while (reader.Read())
                {
                    rows.Add(new Patient(reader.GetInt64(0), reader.GetString(1).Trim(), reader.GetString(2).Trim(), reader.GetDateTime(3), reader.GetString(4).Trim(), reader.GetDateTime(5), reader.GetInt64(6)));
                }
            }
            reader.Close();
            return rows;
        }

        internal ObservableCollection<History> GetHistoryRows(long id)
        {
            var reader = _database.ExecuteQuery(String.Format("Select * from PatientHistory Where CaseID = {0}", id));
            ObservableCollection<History> rows = new ObservableCollection<History>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    rows.Add(new History(reader.GetInt64(0), reader.GetInt64(1), reader.GetDateTime(2), reader.GetString(3).Trim(), reader.GetString(4).Trim()));
                }
            }
            reader.Close();
            return rows;
        }

        internal ObservableCollection<Material> GetMaterialRows()
        {
            var reader = _database.ExecuteQuery("Select * from Materials");
            ObservableCollection<Material> rows = new ObservableCollection<Material>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Material row = new Material(reader.GetInt64(0), reader.GetString(1).Trim(), reader.GetString(2).Trim(), reader.GetString(3).Trim(), reader.GetInt64(4), reader.GetInt64(5));
                    rows.Add(row);
                }
            }
            reader.Close();
            return rows;
        }

        internal void SavePatients(Collection<Patient> patients)
        {
            foreach (var row in patients)
            {
                _database.ExecuteNonQuery(String.Format("EXEC insertOrUpdatePatients {0}, '{1}', '{2}', '{3}', '{4}', '{5}', {6};", row.Id, row.Name, row.Firstname, row.Birthday, row.Adress, row.Entry, row.CaseId));
            }
        }

        internal void SaveMaterials(Collection<Material> mats)
        {
            foreach (var row in mats)
            {
                _database.ExecuteNonQuery(String.Format("EXEC insertOrUpdateMaterials {0}, '{1}', '{2}', '{3}', {4}, {5};", row.Id, row.Name, row.Description, row.Unit, row.Stock, row.MinStock));
            }
        }
    }


    /*
     * Container classes for each table column
     */
    public class Patient
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public DateTime Birthday { get; set; }
        public string Adress { get; set; }
        public DateTime Entry { get; set; }
        public long CaseId { get; set; }

        public Patient(long id, string name, string firstname, DateTime? birthday, string adress, DateTime? entry, long caseId)
        {
            Id = id;
            this.Name = name;
            this.Firstname = firstname;
            this.Birthday = birthday!=null?(DateTime)birthday:DateTime.MinValue;
            this.Adress = adress;
            this.Entry = entry != null ? (DateTime)entry : DateTime.MinValue;
            this.CaseId = caseId;
        }
    }

    public class Material
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public long Stock { get; set; }
        public long MinStock { get; set; }

        public Material(long id, string name, string description, string unit, long stock, long minStock)
        {
            Id = id;
            Name = name;
            Description = description;
            Unit = unit;
            Stock = stock;
            MinStock = minStock;
        }
    }

    public class History
    {
        public long Id { get; set; }
        public long CaseId { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Action { get; set; }

        public History(long id, long caseId, DateTime date, string title, string action)
        {
            Id = id;
            CaseId = caseId;
            Date = date;
            Title = title;
            Action = action;
        }
    }
}
